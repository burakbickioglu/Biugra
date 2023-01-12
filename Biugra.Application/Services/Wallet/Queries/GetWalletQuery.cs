using Biugra.Domain.Models.Dtos;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Domain.Models.Dtos.Wallet;

namespace Biugra.Service.Services.Wallet.Queries;

public class GetWalletQuery : CommandBase<CommandResult<WalletDTO>>
{
    public Guid Id { get; set; }

    public GetWalletQuery(Guid id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetWalletQuery, CommandResult<WalletDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Vallet> _repository;
        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Vallet> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<WalletDTO>> Handle(GetWalletQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _repository.GetFirst(p => p.AppUserId == request.Id.ToString());
            return wallet != null
                ? CommandResult<WalletDTO>.GetSucceed(_mapper.Map<WalletDTO>(wallet))
                : CommandResult<WalletDTO>.GetFailed("Cüzdan bulunamadı.");
        }
    }
}
