using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Message;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Notification.Queries;

public class GetMessagesQuery : CommandBase<CommandResult<List<MessageDTO>>>
{
    public class Handler : IRequestHandler<GetMessagesQuery, CommandResult<List<MessageDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Message> _repository;

        public Handler(IMapper mapper, IGenericRepository<Message> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<List<MessageDTO>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _repository.GetAllFiltered().Include(p=>p.AppUser).ToListAsync();
            return messages != null
                ? CommandResult<List<MessageDTO>>.GetSucceed(_mapper.Map<List<MessageDTO>>(messages))
                : CommandResult<List<MessageDTO>>.GetSucceed(new List<MessageDTO>());
        }
    }
}
