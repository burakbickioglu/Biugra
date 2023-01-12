using Biugra.Domain.Models.Dtos.Category;
using Biugra.Domain.Models.Dtos;
using Biugra.Service.Services.Category.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Models.Dtos.Teacher;
using Biugra.Service.Services.Teacher.Queries;

namespace Biugra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetTeachers")]
        public async Task<CommandResult<List<TeacherDTO>>> GetTeachers() => await _mediator.Send(new GetTeachersQuery());

        [HttpGet]
        [Route("GetTeacher/{id}")]
        public async Task<CommandResult<TeacherDTO>> GetTeacher(Guid id) => await _mediator.Send(new GetTeacherQuery(id));
    }
}
