using System.Text;
using Azure.Storage.Blobs;
using Cloud.Data;
using Cloud.Interfaces;
using Cloud.Repository;
using Cloud.Configuration;
using Cloud.FileSizeConverter;
using Cloud.Interfaces.BlobStorage;
using Cloud.Interfaces.FileSizeConverter;
using Cloud.Interfaces.PasswordHasher;
using Cloud.Interfaces.Repositories;
using Cloud.Interfaces.Services;
using Cloud.Jwt;
using Cloud.JwtProvider;
using Cloud.Services;
using Cloud.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.Configure<AzureStorageOptions>(
    builder.Configuration.GetSection(nameof(AzureStorageOptions)));

builder.Services.AddSwaggerGen();

var jwtSettings = builder.Configuration.GetSection("JwtOptions");

var key = Encoding.UTF8.GetBytes(jwtSettings["TokenKey"] ?? string.Empty);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["token"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireRole("Admin");
    });
    
    options.AddPolicy("User", policy =>
    {
        policy.RequireRole("User");
    });
    
});

builder.Services.AddDbContext<DataContextEF>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});


builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureStorageOptions>>().Value;
    return new BlobServiceClient(options.ConnectionString);
});



builder.Services.AddScoped<IBlobStorage, BlobStorage>();
builder.Services.AddScoped<IFileServices, FileServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddSingleton<IFileSizeConverter, FileSizeConverter>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddAutoMapper(typeof(ApplicationProfile));


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

