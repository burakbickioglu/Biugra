using Biugra.Domain.Models.Dtos.User;
using Biugra.Domain.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biugra.Domain.Models.Dtos.Category;
using Biugra.Service.Services.Category.Queries;

namespace Biugra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetCategory/{id}")]
        public async Task<CommandResult<CategoryDTO>> GetCategory(Guid id) => await _mediator.Send(new GetCategoryQuery(id));

        [HttpGet]
        [Route("GetCategories")]
        public async Task<CommandResult<List<CategoryDTO>>> GetCategories() => await _mediator.Send(new GetCategoriesQuery());
    }
}
