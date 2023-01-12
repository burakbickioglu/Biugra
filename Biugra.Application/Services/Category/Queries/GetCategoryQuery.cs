using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Category.Queries;
public class GetCategoryQuery : CommandBase<CommandResult<CategoryDTO>>
{
    public Guid Id { get; set; }

    public GetCategoryQuery(Guid ıd)
    {
        Id = ıd;
    }

    public class Handler : IRequestHandler<GetCategoryQuery, CommandResult<CategoryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Category> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Category> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<CategoryDTO>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetFirst(p => p.Id == request.Id);
            return category != null
                ? CommandResult<CategoryDTO>.GetSucceed(_mapper.Map<CategoryDTO>(category))
                : CommandResult<CategoryDTO>.GetFailed("Kategori bulunamadı.");
        }
    }
}
