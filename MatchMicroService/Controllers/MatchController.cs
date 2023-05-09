using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Application.UseCases;

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
        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            try
            {
                IList<Match> response = await _matchServices.GetAll();
                return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }


    }
}


