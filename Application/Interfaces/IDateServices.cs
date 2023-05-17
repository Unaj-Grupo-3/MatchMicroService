using Application.Models;

namespace Application.Interfaces
{
    public interface IDateServices
    {
        Task<IList<DateResponse>> GetDatesByUserId(int userId);
        Task<DateResponse> CreateDate(DateRequest2 req);
        Task<DateResponse> EditDate(DateEditRequest req);
    }
}
