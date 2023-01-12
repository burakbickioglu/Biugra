using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Service.Services.Teacher.Queries;

namespace Biugra.Service.Services.ProvidedService.Queries;
public class GetProvidedServicesQuery : CommandBase<CommandResult<List<ProvidedServiceDTO>>>
{
    public class Handler : IRequestHandler<GetProvidedServicesQuery, CommandResult<List<ProvidedServiceDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.ProvidedService> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.ProvidedService> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<List<ProvidedServiceDTO>>> Handle(GetProvidedServicesQuery request, CancellationToken cancellationToken)
        {
            var providedServices = await _repository.GetAllFiltered().Include(p=>p.Teacher).ToListAsync();
            return providedServices != null
                ? CommandResult<List<ProvidedServiceDTO>>.GetSucceed(_mapper.Map<List<ProvidedServiceDTO>>(providedServices))
                : CommandResult<List<ProvidedServiceDTO>>.GetSucceed(new List<ProvidedServiceDTO>());

        }
    }
}
