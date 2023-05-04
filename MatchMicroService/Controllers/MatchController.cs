using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Microsoft.OpenApi.Models;
using Application.Models;

namespace MatchMicroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly IMatchServices _matchServices;

        public MatchController(ITokenServices tokenServices, IMatchServices matchServices)
        {
            _tokenServices = tokenServices;
            _matchServices = matchServices;
        }


        [HttpGet("{id}")]
        [Authorize]
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
                    // Cambiar en algun momento 403
                    return Unauthorized();
                }

                return new JsonResult(new { Message = "Se ha encontrado al Match con exito", Response = response }) { StatusCode = 200 };

            }
            catch (Exception ex)
            {
                return new JsonResult(new {Message =  ex.Message}) { StatusCode = 500};
            }
        }
        
        /////////////////USER MATCH///////////////
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserMatch(UserMatchRequest request)
        {
            //user token, user2, bool like/dislike ->userMatch
            //add
            //update
            //exist u1, u2 -> match
                //request.User2
                //request.LikeUser1

            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                Guid id = _tokenServices.GetUserId(identity);


                //id esta en User2?
                //Si
                //Match
                //No Match
                //id esta en User1?
                //si -> Update
                //no -> add

                //UserMatchResponse  bool isLike
                return new JsonResult(new());
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }
    }
}
