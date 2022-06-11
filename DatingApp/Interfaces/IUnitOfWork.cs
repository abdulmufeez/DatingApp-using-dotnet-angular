namespace DatingApp.Interfaces
{
    public interface IUnitOfWork
    {
        IUserProfileRepository UserProfileRepository { get;}
        IMessageRepository MessageRepository { get;}
        ILikesRepository LikeRepository { get;}

        Task<bool> Complete();
        bool HasChanges();
    }
}