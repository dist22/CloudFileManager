using Cloud.Data;
using Cloud.Interfaces;
using Cloud.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DataContextEF>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IBlobStorage, BlobStorage>();
builder.Services.AddScoped<IFileServices, FileServices>();
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

