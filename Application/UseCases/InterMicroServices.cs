using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class InterMicroServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _chatUrl = "https://localhost:7165/api/v1/";

        public InterMicroServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /*public async Task<ChatResponse> createChat(int user1, int user2)
        {
            var url = _chatUrl + "";
        }*/
    }
}
