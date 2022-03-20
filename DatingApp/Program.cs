using System.Text;
using DatingApp.Data;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Connecting Interface to its implementation
//ITokenService Interface => TokenService Class 
//Only created when its called and terminated afterward
builder.Services.AddScoped<ITokenService, TokenService>();     
//Adding database configurations
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();
//enabling project to get response from the external application call
//Cross Origin Resource Sharing, used when the backend is different from the frontend
builder.Services.AddCors();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenKey"))),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
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

//Reading data from appsetting.json file
var externalUrl = builder.Configuration.GetValue<string>("angularApplicationUrl");
//assigning policy for Cross Origin Response
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(externalUrl));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
