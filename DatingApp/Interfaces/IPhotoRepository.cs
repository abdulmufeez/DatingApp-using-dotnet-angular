using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<PhotoApprovalDto>> GetUnapprovedPhoto();
        Task<Photo> GetPhotoById(int id);
        void RemovePhoto(Photo photo);
    }
}