﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Presentation.Authorization;

namespace MatchMicroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserMatchController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly IMatchServices _matchServices;
        private readonly IUserMatchServices _userMatchServices;
        private readonly IConfiguration _configuration;

        public UserMatchController(ITokenServices tokenServices, IMatchServices matchServices, IUserMatchServices userMatchServices, IConfiguration configuration)
        {
            _tokenServices = tokenServices;
            _matchServices = matchServices;
            _userMatchServices = userMatchServices;
            _configuration = configuration;
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
                bool exist = await _matchServices.ExistMatch(userId, request.User2);
                if (exist)
                {
                    return new JsonResult(new { Message = "No se puede modificar un match existente. " }) { StatusCode = 400 };
                }
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

        [HttpGet]
        [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.Scheme)]
        public async Task<IActionResult> GetUserMatches()
        {
            try
            {
                var apikey = _configuration.GetSection("ApiKey").Get<string>();
                var key = HttpContext.User.Identity.Name;

                if (key != null && key != apikey)
                {
                    return new JsonResult(new { Message = "No se puede acceder. La Key es inválida" }) { StatusCode = 401 };
                }


                IList<UserMatch> response = await _userMatchServices.GetAll();
                return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpGet("Worker")]
        [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.Scheme)]
        public async Task<IActionResult> GetUserMatchesWorker()
        {
            try
            {
                var apikey = _configuration.GetSection("ApiKey").Get<string>();
                var key = HttpContext.User.Identity.Name;

                if (key != null && key != apikey)
                {
                    return new JsonResult(new { Message = "No se puede acceder. La Key es inválida" }) { StatusCode = 401 };
                }


                IList<UserMatch> response = await _userMatchServices.GetAllWorker();
                return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }
    }
}


