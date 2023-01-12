using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Service.Services.Teacher.Commands;

namespace Biugra.Service.Services.ProvidedService.Commands;
public class AddProvidedServiceCommand : CommandBase<CommandResult<AddProvidedServiceResponseDTO>>
{
    public AddProvidedServiceRequestDTO Model { get; set; }

    public AddProvidedServiceCommand(AddProvidedServiceRequestDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<AddProvidedServiceCommand, CommandResult<AddProvidedServiceResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.ProvidedService> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.ProvidedService> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<AddProvidedServiceResponseDTO>> Handle(AddProvidedServiceCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.Price == null)
                request.Model.Price = 0;
            var ProvidedService = _mapper.Map<Domain.Models.ProvidedService>(request.Model);
            if (ProvidedService == null)
                return CommandResult<AddProvidedServiceResponseDTO>.GetFailed("Hizmet oluşturulamadı.");
            ProvidedService.StudentCount = 0;
            var addResult = await _repository.Add(ProvidedService);

            return addResult.IsSucceed
                ? CommandResult<AddProvidedServiceResponseDTO>.GetSucceed(_mapper.Map<AddProvidedServiceResponseDTO>(addResult.Data))
                : CommandResult<AddProvidedServiceResponseDTO>.GetFailed("Hizmet oluşturulamadı");
        }
    }

}
