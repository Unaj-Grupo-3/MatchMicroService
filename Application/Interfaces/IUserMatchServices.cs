using Application.Models;

namespace Application.Interfaces
{
    public interface IUserMatchServices
    {
        public Task<UserMatchResponse> AddOrUpdate(int userId1, int userId2, bool LikeUser1);

    }
}
