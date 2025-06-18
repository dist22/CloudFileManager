using Azure.Storage.Blobs;
using Cloud.Data;
using Cloud.Interfaces;
using Cloud.Repository;
using Cloud.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DataContextEF>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.Configure<AzureStorageOptions>(
    builder.Configuration.GetSection(nameof(AzureStorageOptions)));

// Реєструємо BlobServiceClient як Singleton
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureStorageOptions>>().Value;
    return new BlobServiceClient(options.ConnectionString);
});



builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IBlobStorage, BlobStorage>();
builder.Services.AddScoped<IFileServices, FileServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddSingleton<IFileSizeConverter, FileSizeConverter>();
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


app.MapControllers();
app.Run();

