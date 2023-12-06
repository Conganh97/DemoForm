using Application.Features.Customers.Command;
using Application.Features.Customers.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public CustomersController(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterCommand command)
        {
            return Ok(await _mediatr.Send(command));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            return Ok(await _mediatr.Send(command));
        }
    }
}