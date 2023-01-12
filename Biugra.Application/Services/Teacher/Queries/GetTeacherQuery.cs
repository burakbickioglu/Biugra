using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Service.Services.Category.Queries;

namespace Biugra.Service.Services.Teacher.Queries;
public class GetTeacherQuery : CommandBase<CommandResult<TeacherDTO>>
{
    public Guid Id { get; set; }

    public GetTeacherQuery(Guid ıd)
    {
        Id = ıd;
    }

    public class Handler : IRequestHandler<GetTeacherQuery, CommandResult<TeacherDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Teacher> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Teacher> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<TeacherDTO>> Handle(GetTeacherQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _repository.GetFirst(p => p.Id == request.Id);
            return teacher != null
                ? CommandResult<TeacherDTO>.GetSucceed(_mapper.Map<TeacherDTO>(teacher))
                : CommandResult<TeacherDTO>.GetFailed("Öğretmen bulunamadı.");
        }
    }
}
