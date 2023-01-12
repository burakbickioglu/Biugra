using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Service.Services.Category.Commands;

namespace Biugra.Service.Services.Teacher.Commands;
public class DeleteTeacherCommand : CommandBase<CommandResult<TeacherDTO>>
{
    public Guid Id { get; set; }

    public DeleteTeacherCommand(Guid ıd)
    {
        Id = ıd;
    }

    public class Handler : IRequestHandler<DeleteTeacherCommand, CommandResult<TeacherDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Teacher> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Teacher> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<TeacherDTO>> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacherResponse = await _repository.GetFirstTracked(p => p.Id == request.Id);
            if (teacherResponse == null)
                return CommandResult<TeacherDTO>.GetFailed("Öğretmen bulunamadı.");

            var deleteResult = await _repository.Delete(teacherResponse.Id);
            return deleteResult.IsSucceed
                ? CommandResult<TeacherDTO>.GetSucceed(_mapper.Map<TeacherDTO>(deleteResult.Data))
                : CommandResult<TeacherDTO>.GetFailed("Öğretmen silinemedi");
        }
    }
}
