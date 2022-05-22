using System.Text.Json;
using DatingApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    //this is data seeding class
    public class Seed
    {   
        public static async Task SeedAppUsers(UserManager<ApplicationUser> userManager, RoleManager<AppRole> roleManager)
        {
            // check if already seed
            if (await userManager.Users.AnyAsync()) return;

            //reading data from a json file
            var appUserData = await System.IO.File.ReadAllTextAsync("Data/SeedData/ApplicationUserSeedData.json");
            var appUsers = JsonSerializer.Deserialize<List<ApplicationUser>>(appUserData);
            if (appUsers is null) return;

            var roles = new  List<AppRole>
            {
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
                new AppRole{Name = "Member"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            //adding to database schema
            foreach (var appUser in appUsers)
            {   appUser.UserName = appUser.UserName.ToLower();                                                             
                await userManager.CreateAsync(appUser,"Example8");
                await userManager.AddToRoleAsync(appUser, "Member");
            }            

            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com"                
            };
            await userManager.CreateAsync(admin, "Example8");
            await userManager.AddToRolesAsync(admin, new[] {"Admin","Moderator"});    

            var moderator = new ApplicationUser
            {
                UserName = "moderator",
                Email = "moderator@example.com"
            };
            await userManager.CreateAsync(moderator, "Example8");
            await userManager.AddToRoleAsync(moderator, "Moderator");
        }

        // ==========================================================================================================
        public static async Task SeedUserProfiles(DataContext _context)
        {
            // check if already seed
            if (await _context.UserProfile.AnyAsync()) return;

            //reading data from a json file
            var userProfileData = await System.IO.File.ReadAllTextAsync("Data/SeedData/UserProfileSeedData.json");
            var userProfiles = JsonSerializer.Deserialize<List<UserProfile>>(userProfileData);

            var AppUserId = 1;
            //adding to database schema
            foreach (var userProfile in userProfiles)
            {                                                
                userProfile.DateOfBirth = RandomDayFunc()();
                userProfile.LastActive = RandomDayFunc()();
                userProfile.ApplicationUserId = AppUserId++;

                _context.UserProfile.Add(userProfile);
            }

            await _context.SaveChangesAsync();
        }

        //generate random datetime format date    
        public static Func<DateTime> RandomDayFunc()
        {
            DateTime start = new DateTime(1980, 1, 1);
            Random gen = new Random();
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return () => start.AddDays(gen.Next(range));
        }
    }
}