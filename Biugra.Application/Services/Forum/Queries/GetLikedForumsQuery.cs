using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Forum.Queries;
public class GetLikedForumsQuery : CommandBase<CommandResult<List<ForumResponseDTO>>>
{
    public class Handler : IRequestHandler<GetLikedForumsQuery, CommandResult<List<ForumResponseDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Forum> _repository;
        private readonly UserManager<AppUser> _userManager;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Forum> repository, UserManager<AppUser> userRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userRepository;
        }

        public async Task<CommandResult<List<ForumResponseDTO>>> Handle(GetLikedForumsQuery request, CancellationToken cancellationToken)
        {
            var LikedForums = await _repository.GetAllFiltered(p=>p.IsHelpfull).Include(p=>p.Category).Include(p => p.Comments).ThenInclude(p => p.AppUser).ToListAsync();
            
            var forums = _mapper.Map<List<ForumResponseDTO>>(LikedForums);

            foreach (var forum in forums)
            {
                var user = await _userManager.FindByIdAsync(forum.AppUserId);
                forum.UserName = user.FirstName != null ? user.FirstName : "FirstName";
                forum.UserLastName = user.LastName != null ? user.LastName : "LastName";
            }
            return LikedForums != null
                ? CommandResult<List<ForumResponseDTO>>.GetSucceed(forums)
                : CommandResult<List<ForumResponseDTO>>.GetSucceed(new List<ForumResponseDTO>());
        }
    }
}
