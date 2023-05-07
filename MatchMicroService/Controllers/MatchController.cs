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
    public class MatchController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly IMatchServices _matchServices;
        private readonly IUserMatchServices _userMatchServices;

        public MatchController(ITokenServices tokenServices, IMatchServices matchServices, IUserMatchServices userMatchServices)
        {
            _tokenServices = tokenServices;
            _matchServices = matchServices;
            _userMatchServices = userMatchServices;
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
                    // Cambiar en algun momento
                    return Unauthorized();
                }

                return new JsonResult(new { Message = "Se ha encontrado al Match con exito", Response = response }) { StatusCode = 200 };

            }
            catch (Exception ex)
            {
                return new JsonResult(new {Message =  ex.Message}) { StatusCode = 500};
            }
        }
        /// <summary>
        /// Get All Matches {id u1 u2 create date}
        /// </summary>
        /// <returns> List<Match> o List<MatchGetResponse> </returns> 
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

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UserLike(UserMatchRequest request)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _tokenServices.GetUserId(identity);
                var response = await _userMatchServices.AddOrUpdate(userId, request.User2, request.LikeUser2);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

        //public async Task<IActionResult> GetMatchesByUserId() auth

        //getdatesbyuserid() auth
        //post dates()auth
        //put changestate by user2()auth
    }
}


