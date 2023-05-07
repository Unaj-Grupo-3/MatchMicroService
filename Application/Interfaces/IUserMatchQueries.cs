using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserMatchQueries
    {
        Task<IList<UserMatch>> GetAllMatch();

    }
}
