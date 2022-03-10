using DatingApp.Data;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Reading data from appsetting.json file
var externalUrl = builder.Configuration.GetValue<string>("angularApplicationUrl");

// Add services to the container.

builder.Services.AddControllers();
//Connecting Interface to its implementation
builder.Services.AddScoped<ITokenService, TokenService>();
//Adding database configurations
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//enabling project to get response from the external application call
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//assigning policy for Cross Origin Response
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(externalUrl));

app.UseAuthorization();

app.MapControllers();

app.Run();
