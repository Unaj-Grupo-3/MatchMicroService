using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Application.UseCases
{
    public class UserApiServices : IUserApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public string? _response;

        public UserApiServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKey"];
        }

        //Traer foto, nombre y apellido
        public async Task<List<UserResponse>> GetUsers(IList<int> userIds)
        {
            try
            {
                List<UserResponse> userList = new List<UserResponse>();
                var qParams = _httpClient.BaseAddress + "User/false?usersId=" + userIds[0];

                for (int i = 1; i<userIds.Count; i++)
                {
                    qParams += "&usersId=" + userIds[i];
                }

                _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
                var response = await _httpClient.GetAsync(qParams);

                if (response != null && response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JArray array = JArray.Parse(content);
                    foreach(var i in array)
                    {
                        UserResponse user = new UserResponse();
                        user.UserId = (int)i.SelectToken("userId");
                        user.Name = (string)i.SelectToken("name");
                        user.LastName = (string)i.SelectToken("lastName");
                        user.Images = (string)i.SelectToken("images");

                        userList.Add(user);
                    }

                    return userList;
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
        }
    }
}
