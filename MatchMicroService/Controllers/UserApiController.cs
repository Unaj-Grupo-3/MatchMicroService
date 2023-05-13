using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    /* (SOLO PARA PRUEBAS) API para probar el servicio de UserAPI */
    public class UserApiController : ControllerBase
    {
        private readonly IUserApiServices _services;

        public UserApiController(IUserApiServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetMatchUsers(int userId1, int userId2)
        {
            var response = await _services.GetMatchUsers(userId1, userId2);

            return Ok(response);
        }
    }
}
