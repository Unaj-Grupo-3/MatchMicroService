using Microsoft.AspNetCore.Mvc;
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
    public class MatchController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly IMatchServices _matchServices;
        private readonly IUserMatchServices _userMatchServices;
        private readonly IUserApiServices _userApiServices;
        private readonly IConfiguration _configuration;

        public MatchController(ITokenServices tokenServices, IMatchServices matchServices, IUserMatchServices userMatchServices, IUserApiServices userApiServices, IConfiguration configuration)
        {
            _tokenServices = tokenServices;
            _matchServices = matchServices;
            _userMatchServices = userMatchServices;
            _userApiServices = userApiServices;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMatchById(int id)
        {
            try
            {
                // Ejemplo de uso del token
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                MatchResponse response = await _matchServices.GetById(id);

                if (response == null)
                {
                    // Cambiar en algun momento
                    return NotFound();
                }

                // Obtener el id de los users del match
                if (!_tokenServices.ValidateUserId(identity, response.User1) & !_tokenServices.ValidateUserId(identity, response.User2))
                {
                    // Cambiar en algun momento  -> 403
                    return Forbid();
                }

                return new JsonResult(new { Message = "Se ha encontrado al Match con exito", Response = response }) { StatusCode = 200 };

            }
            catch (Exception ex)
            {
                return new JsonResult(new {Message =  ex.Message}) { StatusCode = 500};
            }
        }

        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserMatchesMe()
        {
            IList<int> userIds = new List<int>();
            IList<UserMatchResp2> respListUser = new List<UserMatchResp2>();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId = _tokenServices.GetUserId(identity);

            // De cara al otro sprint solamente deberia aparecer la info del otro usuario.
            IList<UserMatch> response = await _userMatchServices.GetMatchesByUserId(userId);

            var matchUser = await _matchServices.GetByUserId(userId);

            foreach (var match in response)
            {
                if (!userIds.Contains(match.User2))
                    userIds.Add(match.User2);

                if (!userIds.Contains(match.User1))
                    userIds.Add(match.User1);
            }

            if (userIds.Count == 0)
            {
                return new JsonResult(new { Count = respListUser.Count, Response = userIds }) { StatusCode = 200 };
            }

            List<UserResponse> usersInfo = await _userApiServices.GetUsers(userIds);

            // Devuelve los match pero sin la info de los usuarios matcheados.
            if (usersInfo == null)
            {
                foreach (var i in response)
                {
                    UserMatchResp2 resp2 = new UserMatchResp2()
                    {
                        UserMatchId = i.UserMatchId,
                        MatchId = matchUser.FirstOrDefault(x => x.User2 == i.User2 || x.User2 == i.User1).Id,
                        CreatedAt = i.CreatedAt,
                        UpdatedAt = i.UpdatedAt,
                        userInfo = null,
                    };

                    respListUser.Add(resp2);
                }

                return new JsonResult(new { Message = "Hubo un problema al conectarse con otras APIs", Response = respListUser }) { StatusCode = 502 };
            }

            foreach (var i in response)
            {
                UserMatchResp2 resp2 = new UserMatchResp2()
                {
                    UserMatchId = i.UserMatchId,
                    MatchId= matchUser.FirstOrDefault(x => x.User2 == i.User2 || x.User2 == i.User1).Id,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt,
                    userInfo = usersInfo.FirstOrDefault(s => s.UserId == (i.User1 == userId? i.User2 : i.User1) )
                };

                respListUser.Add(resp2);
            }

            UserMatchesFullResponse resposeMatches = new UserMatchesFullResponse()
            {
                UserMe = usersInfo.FirstOrDefault(x => x.UserId == userId),
                Matches = respListUser
            };

            return new JsonResult(new { Count = respListUser.Count, Response = resposeMatches }) { StatusCode = 200 };


        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.Scheme)]
        public async Task<IActionResult> GetMatches()
        {
            try
            {
                var apikey = _configuration.GetSection("ApiKey").Get<string>();
                var key = HttpContext.User.Identity.Name;

                if (key != null && key != apikey)
                {
                    return new JsonResult(new { Message = "No se puede acceder. La Key es inválida" }) { StatusCode = 401 };
                }

                IList<MatchResponse> response = await _matchServices.GetAll();
                return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

    }
}


