using Application.Models;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMatchServices
    {
        
        Task<MatchResponse2> CreateMatch(MatchRequest request);
        Task<bool> UpdateMatch(MatchRequestUpdate request);
        Task DeleteMatch(int id);
        Task<MatchResponse> GetById(int id);
        Task<IList<MatchResponse>> GetByUserId(int userId);
        Task<MatchResponse> GetByUsersIds(int userId1, int userId2);
        Task<IList<MatchResponse>> GetAll();
        Task<bool> ExistMatch(int userId1, int userId2);

    }
}
