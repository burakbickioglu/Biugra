using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Comment;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Category.Commands;
public class AddCategoryCommand : CommandBase<CommandResult<AddCategoryResponseDTO>>
{
    public AddCategoryRequestDTO Model { get; set; }

    public AddCategoryCommand(AddCategoryRequestDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<AddCategoryCommand, CommandResult<AddCategoryResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Category> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Category> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<AddCategoryResponseDTO>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Domain.Models.Category>(request.Model);
            if (category == null)
                return CommandResult<AddCategoryResponseDTO>.GetFailed("Kategori oluşturulamadı.");

            var addResult = await _repository.Add(category);

            return addResult.IsSucceed
                ? CommandResult<AddCategoryResponseDTO>.GetSucceed(_mapper.Map<AddCategoryResponseDTO>(addResult.Data))
                : CommandResult<AddCategoryResponseDTO>.GetFailed("Kategori oluşturulamadı");
        }
    }
}
