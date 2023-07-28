using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Configuration;
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
                        user.Images = (string)i.SelectToken("image");

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

        public async Task<List<UserResponseAPI>> GetUsersApi(IList<int> userIds)
        {
            try
            {
                List<UserResponseAPI> userList = new List<UserResponseAPI>();
                var qParams = _httpClient.BaseAddress + "User/true?usersId=" + userIds[0];

                for (int i = 1; i < userIds.Count; i++)
                {
                    qParams += "&usersId=" + userIds[i];
                }

                _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
                var response = await _httpClient.GetAsync(qParams);

                if (response != null && response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JArray array = JArray.Parse(content);
                    foreach (var i in array)
                    {
                        LocationResponse location = new LocationResponse()
                        {
                            Id = (int)i.SelectToken("location").SelectToken("id"),
                            Latitude = (double)i.SelectToken("location").SelectToken("latitude"),
                            Longitude = (double)i.SelectToken("location").SelectToken("longitude"),
                            Address = (string)i.SelectToken("location").SelectToken("address")
                        };
                        GenderResponse gender = new GenderResponse()
                        {
                            GenderId = (int)i.SelectToken("gender").SelectToken("genderId"),
                            Description = (string)i.SelectToken("gender").SelectToken("description")
                        };

                        IList<ImageResponse> images = new List<ImageResponse>();

                        foreach(var item in i.SelectToken("images"))
                        {
                            images.Add(new ImageResponse()
                            {
                                Id= (int)item.SelectToken("id"),
                                Url= (string)item.SelectToken("url")
                            });
                        }

                        UserResponseAPI user = new UserResponseAPI();
                        user.UserId = (int)i.SelectToken("userId");
                        user.Name = (string)i.SelectToken("name");
                        user.LastName = (string)i.SelectToken("lastName");
                        user.Birthday = (DateTime)i.SelectToken("birthday");
                        user.Description = (string)i.SelectToken("description");
                        user.Location = location;
                        user.Images = images;
                        user.Gender = gender;

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
