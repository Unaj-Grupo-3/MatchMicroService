using Application.Interfaces;
using Application.Models;
using Application.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public DateController(IDateServices services, ITokenServices tokenServices, IMatchServices matchServices)
        {
            _services = services;
            _tokenServices = tokenServices;
            _matchServices = matchServices;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateDate(DateRequest req)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userId = _tokenServices.GetUserId(identity);

            var match = await _matchServices.GetById(req.MatchId);
            if(match.User1 == userId)
            {
                var response = await _services.CreateDate(req);

                return new JsonResult(response) { StatusCode = 201 };
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("/me")]
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
    }
}
//put changestate by user2()auth
