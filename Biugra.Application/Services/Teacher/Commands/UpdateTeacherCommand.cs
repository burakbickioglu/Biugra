using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Service.Services.Category.Commands;

namespace Biugra.Service.Services.Teacher.Commands;
public class UpdateTeacherCommand : CommandBase<CommandResult<TeacherDTO>>
{
    public TeacherDTO Model { get; set; }

    public UpdateTeacherCommand(TeacherDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<UpdateTeacherCommand, CommandResult<TeacherDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Teacher> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Teacher> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<TeacherDTO>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = _mapper.Map<Domain.Models.Teacher>(request.Model);
            if (teacher == null)
                return CommandResult<TeacherDTO>.GetFailed("Öğretmen bulunamadı");

            var model = await _repository.GetFirst(s => s.Id == request.Model.Id);
            if (model == null)
            {
                return CommandResult<TeacherDTO>.GetFailed("Öğretmen bulunamadı");
            }
            var updateResult = await _repository.Update(teacher);

            return updateResult.IsSucceed
                ? CommandResult<TeacherDTO>.GetSucceed(_mapper.Map<TeacherDTO>(updateResult.Data))
                : CommandResult<TeacherDTO>.GetFailed("Öğretmen güncellenemedi");
        }
    }
}
