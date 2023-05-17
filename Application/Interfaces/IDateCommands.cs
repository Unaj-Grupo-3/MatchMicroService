using Application.Models;
using Domain.Entities;


namespace Application.Interfaces
{
    public interface IDateCommands
    {
        Task InsertDate(Date date);
        Task<Date> AlterDate(DateEditRequest req);
    }
}
