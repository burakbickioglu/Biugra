using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Infrastructure.Interfaces.Repositories;
using Biugra.Service.Services.Category.Queries;

namespace Biugra.Service.Services.Teacher.Queries;
public class GetTeachersQuery : CommandBase<CommandResult<List<TeacherDTO>>>
{
    public class Handler : IRequestHandler<GetTeachersQuery, CommandResult<List<TeacherDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Teacher> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Teacher> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<List<TeacherDTO>>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
        {
            var teachers = await _repository.GetAllFiltered().ToListAsync();
            return teachers != null
                ? CommandResult<List<TeacherDTO>>.GetSucceed(_mapper.Map<List<TeacherDTO>>(teachers))
                : CommandResult<List<TeacherDTO>>.GetSucceed(new List<TeacherDTO>());

        }
    }
}
