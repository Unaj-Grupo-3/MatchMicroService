
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMatchQueries
    {
        Task<Match> GetById(int id);
        Task<IList<Match>> GetByUserId(Guid userId);
        Task<Match> GetByUsersIds(Guid userId1, Guid userId2);
    }
}
