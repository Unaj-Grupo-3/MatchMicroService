using Application.Models;
using Domain.Entities;


namespace Application.Interfaces
{
    public interface IDateQueries
    {
        Task<Date> GetDateById(int id);
        Task<IList<DateResponse>> GetDatesByUserId(int userId);
        Task<IList<DateResponse>> GetDatesByMatchId(int macthId);
    }
}
