using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Message;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Domain.Models.Dtos.User;
using Biugra.Service.Services.Admin.Queries;
using Biugra.Service.Services.Category.Commands;
using Biugra.Service.Services.Notification.Queries;
using Biugra.Service.Services.ProvidedService.Commands;
using Biugra.Service.Services.Teacher.Commands;

namespace Biugra.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme/*, Roles ="SuperAdmin")*/)]
public class AdminController : ControllerBase
{
    protected readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("GetDashboard")]
    public async Task<CommandResult<DashboardDTO>> GetDashboard() => await _mediator.Send(new GetDashboardQuery());

    [HttpGet]
    [Route("GetUser/{id}")]
    public async Task<CommandResult<UserDTO>> GetUser(Guid id) => await _mediator.Send(new GetUserQuery(id));

    [HttpGet]
    [Route("GetUsers")]
    public async Task<CommandResult<List<UserDTO>>> GetUsers() => await _mediator.Send(new GetUsersQuery());

    [HttpGet]
    [Route("GetMessages")]
    public async Task<CommandResult<List<MessageDTO>>> GetMessages() => await _mediator.Send(new GetMessagesQuery());

    [HttpPost]
    [Route("AddUser")]
    public async Task<CommandResult> AddUser([FromBody] AddUserDTO model) => await _mediator.Send(new AddUserCommand(model));

    [HttpPost]
    [Route("AddCategory")]
    public async Task<CommandResult<AddCategoryResponseDTO>> AddCategory([FromBody] AddCategoryRequestDTO model) => await _mediator.Send(new AddCategoryCommand(model));

    [HttpPost]
    [Route("AddTeacher")]
    public async Task<CommandResult<AddTeacherResponseDTO>> AddTeacher([FromBody] AddTeacherRequestDTO model) => await _mediator.Send(new AddTeacherCommand(model));

    [HttpPost]
    [Route("AddProvidedService")]
    public async Task<CommandResult<AddProvidedServiceResponseDTO>> AddProvidedService([FromBody] AddProvidedServiceRequestDTO model) => await _mediator.Send(new AddProvidedServiceCommand(model));

    [HttpPost]
    [Route("UpdateCategory")]
    public async Task<CommandResult<CategoryDTO>> UpdateCategory([FromBody] CategoryDTO model) => await _mediator.Send(new UpdateCategoryCommand(model));

    [HttpPost]
    [Route("UpdateTeacher")]
    public async Task<CommandResult<TeacherDTO>> UpdateTeacher([FromBody] TeacherDTO model) => await _mediator.Send(new UpdateTeacherCommand(model));

    [HttpPost]
    [Route("UpdateProvidedService")]
    public async Task<CommandResult<ProvidedServiceDTO>> UpdateProvidedService([FromBody] ProvidedServiceDTO model) => await _mediator.Send(new UpdateProvidedServiceCommand(model));

    [HttpPost]
    [Route("UpdateUser")]
    public async Task<CommandResult<UserDTO>> UpdateUser([FromBody] UserDTO model) => await _mediator.Send(new UpdateUserCommand(model));

    [HttpDelete]
    [Route("DeleteUser/{id}")]
    public async Task<CommandResult> DeleteUser(Guid id) => await _mediator.Send(new DeleteUserCommand(id));

    [HttpDelete]
    [Route("DeleteCategory/{id}")]
    public async Task<CommandResult<CategoryDTO>> DeleteCategory(Guid id) => await _mediator.Send(new DeleteCategoryCommand(id));

    [HttpDelete]
    [Route("DeleteTeacher/{id}")]
    public async Task<CommandResult<TeacherDTO>> DeleteTeacher(Guid id) => await _mediator.Send(new DeleteTeacherCommand(id));


}
