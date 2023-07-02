using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatchMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateController : ControllerBase
    {
        private readonly IDateServices _services;
        private readonly ITokenServices _tokenServices;
        private readonly IMatchServices _matchServices;
        private readonly IDateValidations _validations;

        public DateController(IDateServices services, ITokenServices tokenServices, IMatchServices matchServices, IDateValidations validations)
        {
            _services = services;
            _tokenServices = tokenServices;
            _matchServices = matchServices;
            _validations = validations;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateDate(DateRequest2 req)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId = _tokenServices.GetUserId(identity);

            var match = await _matchServices.GetById(req.MatchId);
            if(match.User1 == userId || match.User2 == userId)
            {
                var response = await _services.CreateDate(req);

                return new JsonResult(response) { StatusCode = 201 };
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDatesMe()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _tokenServices.GetUserId(identity);

                IList<DateResponse> response = await _services.GetDatesByUserId(userId);
                return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpGet("DateDetails")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDatesDetailsMe()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _tokenServices.GetUserId(identity);

                IList<DatesDetailsResponse> response = await _services.GetDatesDetails(userId);
                return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpGet("{matchId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDatesByMatchId(int matchId)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _tokenServices.GetUserId(identity);

                IList<DateResponse> response = await _services.GetDatesByMatchId(matchId);

                if(response.Count > 0)
                {
                    return new JsonResult(new { Count = response.Count, Response = response }) { StatusCode = 200 };
                }
                return new JsonResult(new { Count = response.Count, Message= "No existen citas para el match solicitado." }) { StatusCode = 200 };
            }   
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangeStateByUser2(DateEditRequest req)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _tokenServices.GetUserId(identity);

                var isInDate = await _validations.IsInDate(userId, req);

                if (isInDate)
                {
                    var response = await _services.EditDate(req);

                    if (response != null)
                    {
                        return new JsonResult(new { Message = "La cita fue modificada exitosamente.", Response = response }) { StatusCode = 201 };
                    }
                    else
                    {
                        return new JsonResult(new { Message = "Hubo un problema durante el proceso. Vuelva a intetarlo mas tarde." }) { StatusCode = 500 };
                    }  
                }
                else
                {
                    return new JsonResult(new { Message = "No autorizado." }) { StatusCode = 403 };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }
    }
}
