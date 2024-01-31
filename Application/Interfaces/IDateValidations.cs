using Application.Models;

namespace Application.Interfaces
{
    public interface IDateValidations
    {
        Task<bool> IsInDate(int userId, DateEditRequest req);
    }
}
