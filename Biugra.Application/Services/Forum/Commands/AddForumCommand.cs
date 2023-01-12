using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biugra.Service.Services.Forum.Commands;
public class AddForumCommand : CommandBase<CommandResult<AddForumResponseDTO>>
{
    public AddForumRequestDTO Model { get; set; }

    public AddForumCommand(AddForumRequestDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<AddForumCommand, CommandResult<AddForumResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Forum> _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager;
        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Forum> repository, ICurrentUserService currentUserService, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<CommandResult<AddForumResponseDTO>> Handle(AddForumCommand request, CancellationToken cancellationToken)
        {
            var forum = _mapper.Map<Domain.Models.Forum>(request.Model);
            if (forum == null)
                return CommandResult<AddForumResponseDTO>.GetFailed("Forum bulunamadı");
            forum.AppUserId = _currentUserService.GetUserId();
            //forum.AppUserId = "934c8b09-3a06-4a65-8542-3021ab63412e";
            var addResult = await _repository.Add(forum);

            return addResult.IsSucceed
                ? CommandResult<AddForumResponseDTO>.GetSucceed(_mapper.Map<AddForumResponseDTO>(addResult.Data))
                : CommandResult<AddForumResponseDTO>.GetFailed("Forum oluşturulamadı");
        }
    }
}
