using Application.Models;

namespace Application.Interfaces
{
    public interface IMatchServices
    {
        Task<MatchResponse> CreateMatch(MatchRequest request);
        Task DeleteMatch(int id);
        Task<MatchResponse> GetById(int id);
        Task<IList<MatchResponse>> GetByUserId(Guid userId);
        Task<MatchResponse> GetByUsersIds(Guid userId1, Guid userId2);
    }
}
