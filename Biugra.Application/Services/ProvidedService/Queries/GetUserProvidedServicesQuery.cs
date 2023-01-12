using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biugra.Service.Services.ProvidedService.Queries;

public class GetUserProvidedServicesQuery:CommandBase<CommandResult<List<UserProvidedServiceDTO>>>
{
    public Guid Id { get; set; }

    public GetUserProvidedServicesQuery(Guid id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetUserProvidedServicesQuery, CommandResult<List<UserProvidedServiceDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.UserProvidedService> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.UserProvidedService> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<List<UserProvidedServiceDTO>>> Handle(GetUserProvidedServicesQuery request, CancellationToken cancellationToken)
        {
            var providedServices = await _repository.GetAllFiltered(p=>p.AppUserId == request.Id.ToString()).Include(p=>p.ProvidedService).ThenInclude(p => p.Teacher).ToListAsync();
            return providedServices != null
                ? CommandResult<List<UserProvidedServiceDTO>>.GetSucceed(_mapper.Map<List<UserProvidedServiceDTO>>(providedServices))
                : CommandResult<List<UserProvidedServiceDTO>>.GetSucceed(new List<UserProvidedServiceDTO>());

        }
    }

}
