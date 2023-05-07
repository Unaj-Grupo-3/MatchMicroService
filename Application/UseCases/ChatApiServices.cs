using Application.Interfaces;
using Application.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Application.UseCases
{
    public class ChatApiServices : IChatApiServices
    {
        private readonly HttpClient _httpClient;
        public string? _response;

        public ChatApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ChatResponse> CreateChat(int user1, int user2)
        {
            try
            {
                var url = _httpClient.BaseAddress + "Chat";

                ChatRequest req = new ChatRequest
                {
                    UserId1 = user1,
                    UserId2 = user2
                };


                var response = await _httpClient.PostAsJsonAsync(url, req);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var obj = JsonDocument.Parse(content).RootElement;
                    ChatResponse chatRes = new ChatResponse
                    {
                        User1Id = int.Parse(obj.GetProperty("user1Id").ToString()),
                        User2Id = int.Parse(obj.GetProperty("user2Id").ToString()),
                        ChatId = int.Parse(obj.GetProperty("chatId").ToString())
                    };

                    return chatRes;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
