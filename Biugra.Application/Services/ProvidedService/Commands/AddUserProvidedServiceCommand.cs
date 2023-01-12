using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.ProvidedService.Commands;
public class AddUserProvidedServiceCommand : CommandBase<CommandResult<UserProvidedServiceDTO>>
{
    public Guid Id { get; set; }

    public AddUserProvidedServiceCommand(Guid id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<AddUserProvidedServiceCommand, CommandResult<UserProvidedServiceDTO>>
    {
        private readonly IGenericRepository<Vallet> _walletRepository;
        private readonly IGenericRepository<Domain.Models.ProvidedService> _providedServiceRepository;
        private readonly IGenericRepository<Domain.Models.UserProvidedService> _userProvidedServiceRepository;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IGenericRepository<Vallet> walletRepository, IGenericRepository<Domain.Models.ProvidedService> providedServiceRepository, IGenericRepository<UserProvidedService> userProvidedServiceRepository, ICurrentUserService currentUserService)
        {
            _walletRepository = walletRepository;
            _providedServiceRepository = providedServiceRepository;
            _userProvidedServiceRepository = userProvidedServiceRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResult<UserProvidedServiceDTO>> Handle(AddUserProvidedServiceCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var userWallet = await _walletRepository.GetFirst(p => p.AppUserId == userId);
            var service = await _providedServiceRepository.GetFirst(p => p.Id == request.Id);
            if(userWallet.Balance >= service.Price)
            {
                var response = await _userProvidedServiceRepository.Add(new UserProvidedService { AppUserId = userId, ProvidedServiceId = service.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30) });
                userWallet.Balance -= (decimal)service.Price;
                await _walletRepository.Update(userWallet);
                service.StudentCount++;
                _providedServiceRepository.Update(service);
                return CommandResult<UserProvidedServiceDTO>.GetSucceed(new UserProvidedServiceDTO { Id=response.Data.Id});
            }
            return CommandResult<UserProvidedServiceDTO>.GetFailed("Yetersiz Bakiye");
        }
    }
}
