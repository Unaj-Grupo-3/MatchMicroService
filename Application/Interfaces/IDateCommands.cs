using Domain.Entities;


namespace Application.Interfaces
{
    public interface IDateCommands
    {
        Task InsertDate(Date date);
    }
}
