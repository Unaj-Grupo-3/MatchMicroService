using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserApiServices
    {
        Task<List<UserResponse>> GetMatchUsers(int userId1, int userId2);
    }
}
