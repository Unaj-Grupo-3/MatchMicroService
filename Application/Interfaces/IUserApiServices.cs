using Application.Models;

namespace Application.Interfaces
{
    public interface IUserApiServices
    {
        Task<List<UserResponse>> GetUsers(IList<int> userIds);
        Task<List<UserResponseAPI>> GetUsersApi(IList<int> userIds);
    }
}
