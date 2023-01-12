using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Teacher.Commands;
public class AddTeacherCommand : CommandBase<CommandResult<AddTeacherResponseDTO>>
{
    public AddTeacherRequestDTO Model { get; set; }

    public AddTeacherCommand(AddTeacherRequestDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<AddTeacherCommand, CommandResult<AddTeacherResponseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Teacher> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Teacher> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<AddTeacherResponseDTO>> Handle(AddTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = _mapper.Map<Domain.Models.Teacher>(request.Model);
            if (teacher == null)
                return CommandResult<AddTeacherResponseDTO>.GetFailed("Öğretmen oluşturulamadı.");

            var addResult = await _repository.Add(teacher);

            return addResult.IsSucceed
                ? CommandResult<AddTeacherResponseDTO>.GetSucceed(_mapper.Map<AddTeacherResponseDTO>(addResult.Data))
                : CommandResult<AddTeacherResponseDTO>.GetFailed("Öğretmen oluşturulamadı");
        }
    }
}
