using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Service.Services.Teacher.Commands;

namespace Biugra.Service.Services.ProvidedService.Commands;
public class UpdateProvidedServiceCommand : CommandBase<CommandResult<ProvidedServiceDTO>>
{
    public ProvidedServiceDTO Model { get; set; }

    public UpdateProvidedServiceCommand(ProvidedServiceDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<UpdateProvidedServiceCommand, CommandResult<ProvidedServiceDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.ProvidedService> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.ProvidedService> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<ProvidedServiceDTO>> Handle(UpdateProvidedServiceCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.Price == null)
                request.Model.Price = 0;
            var providedService = _mapper.Map<Domain.Models.ProvidedService>(request.Model);
            if (providedService == null)
                return CommandResult<ProvidedServiceDTO>.GetFailed("Hizmet bulunamadı");

            var model = await _repository.GetFirst(s => s.Id == request.Model.Id);
            if (model == null)
            {
                return CommandResult<ProvidedServiceDTO>.GetFailed("Hizmet bulunamadı");
            }
            model.Price = providedService.Price;
            model.Title = providedService.Title;
            model.Description = providedService.Description;
            model.TeacherId = providedService.TeacherId;
            var updateResult = await _repository.Update(model);

            return updateResult.IsSucceed
                ? CommandResult<ProvidedServiceDTO>.GetSucceed(_mapper.Map<ProvidedServiceDTO>(updateResult.Data))
                : CommandResult<ProvidedServiceDTO>.GetFailed("Hizmet güncellenemedi");
        }
    }
}
