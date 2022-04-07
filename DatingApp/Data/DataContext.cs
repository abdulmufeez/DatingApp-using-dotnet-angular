using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }        
        public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();
        public DbSet<UserProfile> UserProfile { get; set; }
    }
}