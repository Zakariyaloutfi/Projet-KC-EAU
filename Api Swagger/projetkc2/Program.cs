using Microsoft.OpenApi.Models;
using MySql.EntityFrameworkCore.Extensions;
using projetkc2.Entities;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkMySQL().AddDbContext<ApikcContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "ToDo API", Description = "An ASP.NET Core Web API for managing ToDo items", TermsOfService = new Uri("https://example.com/terms"), Contact = new OpenApiContact { Name = "Example Contact", Url = new Uri("https://example.com/contact") }, License = new OpenApiLicense { Name = "Example License", Url = new Uri("https://example.com/license") } });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
