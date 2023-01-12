using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos.Teacher;

namespace Biugra.Service.Mapper;
public class TeacherProfile : Profile
{
	public TeacherProfile()
	{
		CreateMap<Teacher, TeacherDTO>().ReverseMap();
		CreateMap<Teacher, AddTeacherResponseDTO>().ReverseMap();
		CreateMap<Teacher, AddTeacherRequestDTO>().ReverseMap();
    }
}
