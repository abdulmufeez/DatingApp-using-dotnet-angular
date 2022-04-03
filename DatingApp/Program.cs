using System.Text;
using DatingApp.Data;
using DatingApp.Extensions;
using DatingApp.Interfaces;
using DatingApp.Middlewares;
using DatingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//Application related services extension
builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddControllers();

//enabling project to get response from the external application call
//Cross Origin Resource Sharing, used when the backend is different from the frontend
builder.Services.AddCors();

//Identity related service extension
builder.Services.AddIdentityService(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

//using custom middleware for exceptions
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

//Reading data from appsetting.json file
var externalUrl = builder.Configuration.GetValue<string>("angularApplicationUrl");
//assigning policy for Cross Origin Response
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(externalUrl));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
