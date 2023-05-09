using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;

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
                    await _matchServices.CreateMatch(new MatchRequest
                    {
                        User1 = response.User1,
                        User2 = response.User2,
                    });
                }

                return new JsonResult(new { Message = "Se ha agregado interaccion.", Response = response }) { StatusCode = 201 };
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


