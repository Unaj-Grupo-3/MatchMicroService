using Application.Interfaces;
using Application.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class UserApiServices : IUserApiServices
    {
        private readonly HttpClient _httpClient;
        public string? _response;

        public UserApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Traer foto, nombre y apellido
        public async Task<List<UserResponse>> GetMatchUsers(int userId1, int userId2)
        {
            try
            {
                List<UserResponse> userList = new List<UserResponse>();
                var qParams = _httpClient.BaseAddress + "User/userByIds?usersId=" + userId1 + "&usersId=" + userId2;

                var response = await _httpClient.GetAsync(qParams);

                if (response != null && response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JArray array = JArray.Parse(content);
                    foreach(var i in array)
                    {
                        UserResponse userResp = new UserResponse();
                        userResp.UserId = (int)i.SelectToken("userId");
                        userResp.Name = (string)i.SelectToken("name");
                        userResp.LastName = (string)i.SelectToken("lastName");
                        JArray jArrays = (JArray)i.SelectToken("images");
                        if (!jArrays.IsNullOrEmpty())
                        {
                            userResp.Images = jArrays[0].SelectToken("url").ToString();
                        }
                        else
                        {
                            userResp.Images = null;
                        }

                        userList.Add(userResp);
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
