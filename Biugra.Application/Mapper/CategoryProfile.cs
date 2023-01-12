using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos.Category;

namespace Biugra.Service.Mapper;
public class CategoryProfile : Profile
{
	public CategoryProfile()
	{
		CreateMap<Category, AddCategoryRequestDTO>().ReverseMap();
		CreateMap<Category, AddCategoryResponseDTO>().ReverseMap();
		CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}
