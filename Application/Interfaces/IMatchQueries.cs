
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMatchQueries
    {
        Task<Match> GetById(int id);
        Task<Match> GetByUserId(int userId);
        Task<Match> GetByUsersIds(int userId1, int userId2);
    }
}
