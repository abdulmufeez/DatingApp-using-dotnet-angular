using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Data;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Extensions
{
    //static class means there is only one initiliazing of these class
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, 
            IConfiguration config)
        {
            //Connecting Interface to its implementation
            //ITokenService Interface => TokenService Class 
            //Only created when its called and terminated afterward
            services.AddScoped<ITokenService, TokenService>();
            //Adding database configurations
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}