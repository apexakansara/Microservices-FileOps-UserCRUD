using ConsumerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly RabbitMQConsumer _consumer;
    public UserController(RabbitMQConsumer consumer)
    {
        this._consumer = consumer;
    }

    [HttpPost]
    [ApiVersion("1.0")]
    public async Task<IActionResult> AddAsync()
    {
        await this._consumer.ConsumeAsync();

        return Ok();
    }

}