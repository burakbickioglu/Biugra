using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Category.Commands;
public class UpdateCategoryCommand : CommandBase<CommandResult<CategoryDTO>>
{
    public CategoryDTO Model { get; set; }

    public UpdateCategoryCommand(CategoryDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<UpdateCategoryCommand, CommandResult<CategoryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Category> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Category> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<CategoryDTO>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Domain.Models.Category>(request.Model);
            if (category == null)
                return CommandResult<CategoryDTO>.GetFailed("Kategori bulunamadı");

            var model = await _repository.GetFirst(s => s.Id == request.Model.Id);
            if (model == null)
            {
                return CommandResult<CategoryDTO>.GetFailed("Kategori bulunamadı");
            }
            var updateResult = await _repository.Update(category);

            return updateResult.IsSucceed
                ? CommandResult<CategoryDTO>.GetSucceed(_mapper.Map<CategoryDTO>(updateResult.Data))
                : CommandResult<CategoryDTO>.GetFailed("Kategori güncellenemedi");
        }
    }
}
