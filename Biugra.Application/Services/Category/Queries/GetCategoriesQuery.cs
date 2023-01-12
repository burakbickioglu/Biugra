using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Category.Queries;
public class GetCategoriesQuery : CommandBase<CommandResult<List<CategoryDTO>>>
{
    public class Handler : IRequestHandler<GetCategoriesQuery, CommandResult<List<CategoryDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Category> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Category> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<List<CategoryDTO>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repository.GetAllFiltered().ToListAsync();
            return categories != null
                ? CommandResult<List<CategoryDTO>>.GetSucceed(_mapper.Map<List<CategoryDTO>>(categories))
                : CommandResult<List<CategoryDTO>>.GetSucceed(new List<CategoryDTO>());

        }
    }
}
