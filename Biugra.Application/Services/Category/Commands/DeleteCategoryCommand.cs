using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Category.Commands;
public class DeleteCategoryCommand : CommandBase<CommandResult<CategoryDTO>>
{
    public Guid Id { get; set; }

    public DeleteCategoryCommand(Guid ıd)
    {
        Id = ıd;
    }

    public class Handler : IRequestHandler<DeleteCategoryCommand, CommandResult<CategoryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Category> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Category> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<CategoryDTO>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryResponse = await _repository.GetFirstTracked(p => p.Id == request.Id);
            if (categoryResponse == null)
                return CommandResult<CategoryDTO>.GetFailed("Kategori bulunamadı.");

            var deleteResult = await _repository.Delete(categoryResponse.Id);
            return deleteResult.IsSucceed
                ? CommandResult<CategoryDTO>.GetSucceed(_mapper.Map<CategoryDTO>(deleteResult.Data))
                : CommandResult<CategoryDTO>.GetFailed("Kategori silinemedi");
        }
    }
}
