﻿using Application.Interfaces;
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
        public async Task<IActionResult> GetMatchUsers([FromQuery] List<int> usersId)
        {
            var response = await _services.GetUsers(usersId);

            return Ok(response);
        }
    }
}
