using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos;
using Biugra.Service.Services.Category.Queries;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Service.Services.ProvidedService.Queries;
using Biugra.Service.Services.ProvidedService.Commands;

namespace Biugra.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProvidedServiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProvidedServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("GetProvidedService/{id}")]
    public async Task<CommandResult<ProvidedServiceDTO>> GetProvidedService(Guid id) => await _mediator.Send(new GetProvidedServiceQuery(id));

    [HttpGet]
    [Route("GetProvidedServices")]
    public async Task<CommandResult<List<ProvidedServiceDTO>>> GetProvidedServices() => await _mediator.Send(new GetProvidedServicesQuery());

    [HttpGet]
    [Route("GetUserProvidedServices/{id}")]
    public async Task<CommandResult<List<UserProvidedServiceDTO>>> GetUserProvidedServices(Guid id) => await _mediator.Send(new GetUserProvidedServicesQuery(id));

    [HttpGet]
    [Route("AddUserProvidedService/{id}")]
    public async Task<CommandResult<UserProvidedServiceDTO>> AddUserProvidedService(Guid id) => await _mediator.Send(new AddUserProvidedServiceCommand(id));

}
