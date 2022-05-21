using System.Text.Json;
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    //this is data seeding class
    public class Seed
    {   
        // public static async Task SeedAppUsers(UserManager<ApplicationUser> userManager)
        // {
        //     // check if already seed
        //     if (await userManager.Users.AnyAsync()) return;

        //     //reading data from a json file
        //     var appUserData = await System.IO.File.ReadAllTextAsync("Data/SeedData/ApplicationUserSeedData.json");
        //     var appUsers = JsonSerializer.Deserialize<List<ApplicationUser>>(appUserData);
        //     if (appUsers is null) return;

        //     //adding to database schema
        //     foreach (var appUser in appUsers)
        //     {                                                                
        //         await userManager.CreateAsync(appUser,"Example8");
        //     }            
        // }

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
                //userProfile.DateOfBirth = RandomDayFunc()();
                userProfile.LastActive = RandomDayFunc()();
                userProfile.ApplicationUserId = AppUserId++;

                _context.UserProfile.Add(userProfile);
            }

            await _context.SaveChangesAsync();
        }

        //generate random datetime format date    
        public static Func<DateTime> RandomDayFunc()
        {
            DateTime start = new DateTime(1995, 1, 1);
            Random gen = new Random();
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return () => start.AddDays(gen.Next(range));
        }
    }
}