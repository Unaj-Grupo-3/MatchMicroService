using Application.Models;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserMatchCommands
    {
        Task<UserMatchResponse> Like(UserMatch userMatch);
        Task<UserMatchResponse> UpdateRow(UserMatch userMatch);

    }
}
