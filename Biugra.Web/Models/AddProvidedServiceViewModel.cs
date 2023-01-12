using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;

namespace Biugra.BackOffice.Models;
public class AddProvidedServiceViewModel
{
    public AddProvidedServiceRequestDTO? ProvidedService { get; set; }
    public List<TeacherDTO>? Teachers { get; set; }
}
