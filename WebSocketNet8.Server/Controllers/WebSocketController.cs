using Microsoft.AspNetCore.Mvc;
using WebSocketNet8Library;

namespace WebSocketNet8.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        SocketServerService _socketService;
        public WebSocketController(SocketServerService socketService)
        {
            _socketService = socketService;
        }

        [HttpGet]
        public async Task ws()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                await _socketService.connect(await HttpContext.WebSockets.AcceptWebSocketAsync(), Request.Headers["name"].ToString());

            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        [HttpGet]
        public async Task Stop()
        {
            await _socketService.StopServerAsync();
        }


    }
}
