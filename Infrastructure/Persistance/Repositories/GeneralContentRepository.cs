using BaseProject.Domain.Models.Context;

namespace Biugra.Infrastructure.Persistance.Repositories;

public class GeneralContentRepository<T> : DataRepository<T, DataContext>, IGeneralContentRepository<T> where T : class, IBaseEntity
{
    public GeneralContentRepository(DataContext context, ICurrentUserService userService) : base(context, userService)
    {
    }
}
