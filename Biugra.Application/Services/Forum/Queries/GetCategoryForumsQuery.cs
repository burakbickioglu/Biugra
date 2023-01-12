using BaseProject.Domain.Models;
using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Biugra.Service.Services.Forum.Queries;
public class GetCategoryForumsQuery : CommandBase<CommandResult<List<ForumResponseDTO>>>
{
    public string CategoryName { get; set; }

    public GetCategoryForumsQuery(string categoryName)
    {
        CategoryName = categoryName;
    }

    public class Handler : IRequestHandler<GetCategoryForumsQuery, CommandResult<List<ForumResponseDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Category> _repository;
        private readonly UserManager<AppUser> _userManager;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Category> repository, UserManager<AppUser> userRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userRepository;
        }

        public async Task<CommandResult<List<ForumResponseDTO>>> Handle(GetCategoryForumsQuery request, CancellationToken cancellationToken)
        {
            var CategoryForums = await _repository.GetAllFiltered(p=>p.Name == request.CategoryName).Include(p => p.Forums).ThenInclude(p=>p.Comments).ThenInclude(p=>p.AppUser).FirstAsync();

            var forums = _mapper.Map<List<ForumResponseDTO>>(CategoryForums.Forums);

            foreach (var forum in forums)
            {
                var user = await _userManager.FindByIdAsync(forum.AppUserId);
                forum.UserName = user.FirstName != null ? user.FirstName : "FirstName";
                forum.UserLastName = user.LastName != null ? user.LastName : "LastName";
            }
            return CategoryForums != null
                ? CommandResult<List<ForumResponseDTO>>.GetSucceed(forums)
                : CommandResult<List<ForumResponseDTO>>.GetSucceed(new List<ForumResponseDTO>());
        }
    }
}
