using Microsoft.AspNetCore.Mvc;
using NetMQ;
using NetMQ.Sockets;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    [HttpGet]
    public ActionResult<String> Get(String message)
    {
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://localhost:44376");
            requester.SendFrame(message);

            var response = requester.ReceiveFrameString();

            return response;
        }
    }
}
