using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Comment;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Comment.Commands;
public class AddCommentCommand : CommandBase<CommandResult<AddCommentResponseDTO>>
{
    public AddCommentRequestDTO Model { get; set; }

    public AddCommentCommand(AddCommentRequestDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<AddCommentCommand, CommandResult<AddCommentResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Comment> _repository;
        private readonly ICurrentUserService _currentUserService;
        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Comment> repository, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResult<AddCommentResponseDTO>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Domain.Models.Comment>(request.Model);
            if (comment == null)
                return CommandResult<AddCommentResponseDTO>.GetFailed("Yorum oluşturulamadı.");

            comment.AppUserId = _currentUserService.GetUserId();
            //comment.AppUserId = "934c8b09-3a06-4a65-8542-3021ab63412e";
            var addResult = await _repository.Add(comment);

            return addResult.IsSucceed
                ? CommandResult<AddCommentResponseDTO>.GetSucceed(_mapper.Map<AddCommentResponseDTO>(addResult.Data))
                : CommandResult<AddCommentResponseDTO>.GetFailed("Yorum oluşturulamadı");
        }
    }
}
