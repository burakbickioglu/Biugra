using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Comment;
using Biugra.Domain.Models.Dtos.Forum;
using Biugra.Domain.Models.Dtos.Message;
using Biugra.Service.Services.Comment.Commands;
using Biugra.Service.Services.Forum.Commands;
using Biugra.Service.Services.Forum.Queries;
using Biugra.Service.Services.Notification.Commands;

namespace Biugra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ForumController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ForumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetForum/{id}")]
        public async Task<CommandResult<ForumResponseDTO>> GetForum(Guid id) => await _mediator.Send(new GetForumQuery(id));

        
        [HttpPost]
        [Route("MarkUsefull")]
        public async Task<CommandResult> MarkUsefull(MarkUsefullDTO model) => await _mediator.Send(new MarkUsefullCommand(model));

        [HttpGet]
        [Route("GetAllForums")]
        public async Task<CommandResult<List<ForumResponseDTO>>> GetAllForums() => await _mediator.Send(new GetForumsQuery());

        [HttpGet]
        [Route("GetUserForums/{id}")]
        public async Task<CommandResult<List<ForumResponseDTO>>> GetUserForums(Guid id) => await _mediator.Send(new GetUserForumsQuery(id));

        [HttpGet]
        [Route("GetCategoryForums/{CategoryName}")]
        public async Task<CommandResult<List<ForumResponseDTO>>> GetCategoryForums(string CategoryName) => await _mediator.Send(new GetCategoryForumsQuery(CategoryName));

        [HttpGet]
        [Route("GetLikedForums")]
        public async Task<CommandResult<List<ForumResponseDTO>>> GetLikedForums() => await _mediator.Send(new GetLikedForumsQuery());

        [HttpPost]
        [Route("AddForum")]
        public async Task<CommandResult<AddForumResponseDTO>> AddForum([FromBody] AddForumRequestDTO model) => await _mediator.Send(new AddForumCommand(model));

        [HttpPost]
        [Route("AddComment")]
        public async Task<CommandResult<AddCommentResponseDTO>> AddComment([FromBody] AddCommentRequestDTO model) => await _mediator.Send(new AddCommentCommand(model));

        [HttpPost]
        [Route("SendMessage")]
        public async Task<CommandResult<MessageDTO>> SendMessage([FromBody] MessageDTO model) => await _mediator.Send(new SendMessageCommand(model));

    }
}
