using Application.Models;

namespace Application.Interfaces
{
    public interface IChatApiServices
    {
        Task<ChatResponse> CreateChat(int user1, int user2);
    }
}
