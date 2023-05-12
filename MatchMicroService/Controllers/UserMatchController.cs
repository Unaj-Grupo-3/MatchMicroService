using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Azure;

namespace MatchMicroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserMatchController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly IMatchServices _matchServices;
        private readonly IUserMatchServices _userMatchServices;

        public UserMatchController(ITokenServices tokenServices, IMatchServices matchServices, IUserMatchServices userMatchServices)
        {
            _tokenServices = tokenServices;
            _matchServices = matchServices;
            _userMatchServices = userMatchServices;
        }


       /* Comportamiento actual del endpoint
        * 
        * No hay dependencia con el micro de chat, se crea el match aunque halla problemas al crear el chat
        * ChatID se muestra como -1 cuando no se crea
        * Si hay problemas de conexion con el microservicios Chat tira un 502
        *  
        */

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UserLike(UserMatchRequest request)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _tokenServices.GetUserId(identity);
                int like = request.Like ? 1 : -1;
                var response = await _userMatchServices.AddOrUpdate(userId, request.User2, like);

                if (response.IsMatch)
                {
                    var match = await _matchServices.CreateMatch(new MatchRequest
                    {
                        User1 = response.User1,
                        User2 = response.User2
                    });

                    if (match != null)
                    {
                        var response3 = new UserMatchResponse
                        {
                            User1 = response.User1,
                            User2 = response.User2,
                            IsMatch = true,
                            Match = new MatchResponse2
                            {
                                Id = match.Id,
                                ChatId = match.ChatId
                            }
                        };

                        return new JsonResult(new { Message = "Se ha agregado interaccion.", Response = response3 }) { StatusCode = 201 };
                    }
                    else
                    {
                        return new JsonResult(new { Message = "Hubo un problema con la conexión a otras APIs. ", Response = response }) { StatusCode = 502 };
                    }
                }
                else
                {
                    return new JsonResult(new { Message = "Se ha agregado interaccion.", Response = response }) { StatusCode = 201 };
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserMatchesMe()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _tokenServices.GetUserId(identity);

                IList<UserMatch> response = await _userMatchServices.GetMatchesByUserId(userId);
                return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMatches()
        {
            try
            {
                IList<UserMatch> response = await _userMatchServices.GetAll();
                return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }
    }
}


