using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.User;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Admin.Queries;
public class GetDashboardQuery : CommandBase<CommandResult<DashboardDTO>>
{
    public class Handler : IRequestHandler<GetDashboardQuery, CommandResult<DashboardDTO>>
    {
        private readonly IGenericRepository<Domain.Models.Forum> _forumRepository;
        private readonly IGenericRepository<Domain.Models.Comment> _commentRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public Handler(IGenericRepository<Domain.Models.Forum> forumRepository, IGenericRepository<Domain.Models.Comment> commentRepository, UserManager<AppUser> userManager, IMapper mapper)
        {
            _forumRepository = forumRepository;
            _commentRepository = commentRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CommandResult<DashboardDTO>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            DashboardDTO dto = new();
            var forumQuery = _forumRepository.GetAllFiltered();
            dto.TodayForumCount = forumQuery.Where(p => p.CreatedOn.Date == DateTime.Today).Count();
            dto.ThisMonthForumCount = forumQuery.Where(p => p.CreatedOn > DateTime.Now.AddDays(-30)).Count();
            dto.TotalForumCount = forumQuery.Count();

            var commentQuery = _commentRepository.GetAllFiltered();
            dto.TodayCommentCount = commentQuery.Where(p => p.CreatedOn.Date == DateTime.Today).Count();
            dto.ThisMonthCommentCount = commentQuery.Where(p => p.CreatedOn > DateTime.Now.AddDays(-30)).Count();
            dto.TotalCommentCount = commentQuery.Count();

            var users = _userManager.Users;
            dto.TotalUserCount = users.Count();
            dto.TodayUserCount = users.Where(p => p.CreatedAt.Date == DateTime.Today).Count();
            dto.ThisMonthUserCount = users.Where(p => p.CreatedAt > DateTimeOffset.Now.AddDays(-30)).Count();

            var lastFiveUser = await users.OrderByDescending(p => p.CreatedAt).Take(5).ToListAsync();

            dto.LastFiveUser = _mapper.Map<List<UserDTO>>(lastFiveUser);

            return CommandResult<DashboardDTO>.GetSucceed(dto);

        }
    }
}
