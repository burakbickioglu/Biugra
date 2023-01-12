using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos.ProvidedService;
using Biugra.Domain.Models.Dtos.Teacher;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biugra.BackOffice.Models;

public class UpdateProvidedServiceViewModel
{
    public ProvidedServiceDTO? ProvidedService { get; set; }
    public List<TeacherDTO>? Teachers { get; set; }
}
