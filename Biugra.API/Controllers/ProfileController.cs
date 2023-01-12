using Biugra.Domain.Models.Dtos.User;
using Biugra.Domain.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biugra.Service.Services.Wallet.Queries;
using Biugra.Domain.Models.Dtos.Wallet;
using Biugra.Domain.Models.Dtos.Notification;
using Biugra.Service.Services.Notification.Queries;

namespace Biugra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetUser/{id}")]
        public async Task<CommandResult<UserDTO>> GetUser(Guid id) => await _mediator.Send(new GetUserQuery(id));

        [HttpGet]
        [Route("GetUserNotifications/{id}")]
        public async Task<CommandResult<List<NotificationDTO>>> GetUserNotifications(Guid id) => await _mediator.Send(new GetUserNotificationsQuery(id));


        [HttpGet]
        [Route("GetWallet/{id}")]
        public async Task<CommandResult<WalletDTO>> GetWallet(Guid id) => await _mediator.Send(new GetWalletQuery(id));

        [HttpPost]
        [Route("EditProfile")]
        public async Task<CommandResult<UserDTO>> EditProfile([FromBody] UserDTO model) => await _mediator.Send(new UpdateUserCommand(model));
    }
}
