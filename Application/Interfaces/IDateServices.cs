using Application.Models;

namespace Application.Interfaces
{
    public interface IDateServices
    {
        Task<IList<DateResponse>> GetDatesByUserId(int userId);
        Task<IList<DateResponse>> GetDatesByMatchId(int matchId);
        Task<DateResponse> CreateDate(DateRequest2 req);
        Task<DateResponse> EditDate(DateEditRequest req);
    }
}
