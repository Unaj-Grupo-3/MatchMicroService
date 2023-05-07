using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatApiController : ControllerBase
    {
        //Solo para pruebas

        private readonly IChatApiServices _chatApiServices;

        public ChatApiController(IChatApiServices chatApiServices)
        {
            _chatApiServices = chatApiServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(ChatRequest req)
        {
            var response = await _chatApiServices.CreateChat(req.UserId1, req.UserId2);

            return new JsonResult(response) { StatusCode = 201};
        }
    }
}
