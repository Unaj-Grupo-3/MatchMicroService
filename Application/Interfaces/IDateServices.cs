using Application.Models;

namespace Application.Interfaces
{
    public interface IDateServices
    {
        Task<DateResponse> CreateDate(DateRequest req);
        public Task<IList<DateResponse>> GetDatesByUserId(int userId);

    }
}
