using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Message;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Notification.Commands;
public class SendMessageCommand : CommandBase<CommandResult<MessageDTO>>
{
    public MessageDTO Model{ get; set; }

    public SendMessageCommand(MessageDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<SendMessageCommand, CommandResult<MessageDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Message> _repository;

        public Handler(IMapper mapper, IGenericRepository<Message> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<MessageDTO>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _mapper.Map<Domain.Models.Message>(request.Model);
            if (message == null)
                return CommandResult<MessageDTO>.GetFailed("Mesaj oluşturulamadı.");

            var addResult = await _repository.Add(message);

            return addResult.IsSucceed
                ? CommandResult<MessageDTO>.GetSucceed(_mapper.Map<MessageDTO>(addResult.Data))
                : CommandResult<MessageDTO>.GetFailed("Mesaj oluşturulamadı");
        }
    }
}
