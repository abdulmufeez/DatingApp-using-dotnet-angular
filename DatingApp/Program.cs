using DatingApp.Data;
using DatingApp.Extensions;
using DatingApp.Middlewares;
using Microsoft.EntityFrameworkCore;


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


//======================================================================
//this section is used for data seeding 
//
//creating a service container which call any service running already in application
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
//
try
{
    //calling datacontect service
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    //seeding data 
    await Seed.SeedUserProfiles(context);
}
catch (Exception ex)
{
    //calling logger service
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while seeding user profile");
}

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
