using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Service.Services.Teacher.Queries;

namespace Biugra.Service.Services.ProvidedService.Queries;
public class GetProvidedServiceQuery : CommandBase<CommandResult<ProvidedServiceDTO>>
{
    public Guid Id { get; set; }

    public GetProvidedServiceQuery(Guid ıd)
    {
        Id = ıd;
    }

    public class Handler : IRequestHandler<GetProvidedServiceQuery, CommandResult<ProvidedServiceDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.ProvidedService> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.ProvidedService> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<ProvidedServiceDTO>> Handle(GetProvidedServiceQuery request, CancellationToken cancellationToken)
        {
            var providedService = await _repository.GetAllFiltered(p => p.Id == request.Id).Include(p => p.Teacher).FirstAsync();

            return providedService != null
                ? CommandResult<ProvidedServiceDTO>.GetSucceed(_mapper.Map<ProvidedServiceDTO>(providedService))
                : CommandResult<ProvidedServiceDTO>.GetFailed("Hizmet bulunamadı.");
        }
    }
}
