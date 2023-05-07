using Application.Models;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserMatchServices
    {
        public Task<IList<UserMatch>> GetAll();
        public Task<IList<UserMatch>> GetMatchesByUserId(int userId);
        public Task<UserMatchResponse> AddOrUpdate(int userId1, int userId2, int LikeUser2);

    }
}
