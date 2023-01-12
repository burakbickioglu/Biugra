using BaseProject.Domain.Models;
using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Forum.Queries;
public class GetForumQuery : CommandBase<CommandResult<ForumResponseDTO>>
{
    public Guid ForumId { get; set; }

    public GetForumQuery(Guid forumId)
    {
        ForumId = forumId;
    }

    public class Handler : IRequestHandler<GetForumQuery, CommandResult<ForumResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Forum> _repository;
        private readonly UserManager<AppUser> _userManager;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Forum> repository, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<CommandResult<ForumResponseDTO>> Handle(GetForumQuery request, CancellationToken cancellationToken)
        {
            var forum = await _repository.GetAllFiltered(p => p.Id == request.ForumId).Include(p=>p.Comments).ThenInclude(p=>p.AppUser).Include(p=>p.Category).FirstAsync();

            if(forum == null)
                return CommandResult<ForumResponseDTO>.GetFailed("Forum bulunamadı.");
            var response = _mapper.Map<ForumResponseDTO>(forum);


            var user = await _userManager.FindByIdAsync(forum.AppUserId);
            response.UserName = user.FirstName != null ? user.FirstName : "FirstName";
            response.UserLastName = user.LastName != null ? user.LastName : "LastName";

            return CommandResult<ForumResponseDTO>.GetSucceed(response);
        }
    }
}
