using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Microsoft.OpenApi.Models;

namespace MatchMicroService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;

        public MatchController(ITokenServices tokenServices) 
        {   
            _tokenServices = tokenServices;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetMatchById(string id)
        {
            // Ejemplo de uso del token
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            _tokenServices.IsExpiredToken(identity);

            // Obtener el id de los users del match
            int userId = 0;
            _tokenServices.ValidateUserId(identity, userId);

            return Ok();
        }
    }
}
