using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biugra.Service.Services.Forum.Queries;

public class GetUserForumsQuery : CommandBase<CommandResult<List<ForumResponseDTO>>>
{
    public GetUserForumsQuery(Guid id)
    {
        this.id = id;
    }

    public Guid id { get; set; }

    public class Handler : IRequestHandler<GetUserForumsQuery, CommandResult<List<ForumResponseDTO>>>
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

        public async Task<CommandResult<List<ForumResponseDTO>>> Handle(GetUserForumsQuery request, CancellationToken cancellationToken)
        {
            var Forums = await _repository.GetAllFiltered(p=>p.AppUserId == request.id.ToString()).Include(p => p.Category).Include(p => p.Comments).ThenInclude(p => p.AppUser).ToListAsync();
            if (Forums == null)
                return CommandResult<List<ForumResponseDTO>>.GetSucceed(new List<ForumResponseDTO>());

            var response = _mapper.Map<List<ForumResponseDTO>>(Forums);
            foreach (var forum in response)
            {
                var user = await _userManager.FindByIdAsync(forum.AppUserId);
                forum.UserName = user.FirstName != null ? user.FirstName : "FirstName";
                forum.UserLastName = user.LastName != null ? user.LastName : "LastName";
            }

            return CommandResult<List<ForumResponseDTO>>.GetSucceed(response);
        }
    }
}
