using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        public PhotoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Photo> GetPhotoById(int id)
        {
            return await _context.Photos
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PhotoApprovalDto>> GetUnapprovedPhoto()
        {
            return await _context.Photos
                .IgnoreQueryFilters()
                .Where(p => p.IsApprove == false)
                .Select(u => new PhotoApprovalDto 
                {
                    Id = u.Id,
                    Url = u.Url,
                    IsApproved = u.IsApprove,
                    KnownAs = u.UserProfile.KnownAs,
                    UserProfileId = u.UserProfileId
                }).ToListAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
        }
    }
}