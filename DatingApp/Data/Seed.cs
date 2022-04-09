using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    //this is data seeding class
    public class Seed
    {   
        public static async Task SeedUserProfiles(DataContext _context)
        {
            // check if already seed
            if (await _context.UserProfile.AnyAsync()) return;

            //reading data from a json file
            var userProfileData = await System.IO.File.ReadAllTextAsync("Data/SeedData/UserProfileSeedData.json");
            var userProfiles = JsonSerializer.Deserialize<List<UserProfile>>(userProfileData);

            //adding to database schema
            foreach (var userProfile in userProfiles)
            {                                
                // userProfile.ProfileCreatedAt = DateTime.Now;
                // userProfile.DateOfBirth = RandomDayFunc()();
                // userProfile.LastActive = RandomDayFunc()();

                userProfile.ApplicationUserId = 1;
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