using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class LikeRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int LikeUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, LikeUserId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userProfileId)
        {
            var users = _context.UserProfile.OrderBy(u => u.Id).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if (predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == userProfileId);
                users = likes.Select(like => like.LikedUser);
            }

            if (predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == userProfileId);
                users = likes.Select(like => like.SourceUser);
            }

            return await users.Select(user => new LikeDto
            {
                Id = user.Id,
                Age = user.DateOfBirth.CalculateAge(),
                KnownAs = user.KnownAs,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City
            }).ToListAsync();
        }

        public async Task<UserProfile> GetUserProfileWithLikes(int userProfileId)
        {
            return await _context.UserProfile
                .Include(l => l.LikedUsers)
                .SingleOrDefaultAsync(u => u.Id == userProfileId);
        }
    }
}