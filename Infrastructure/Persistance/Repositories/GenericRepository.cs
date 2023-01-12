using BaseProject.Domain.Models.Context;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Infrastructure.Persistance.Repositories;
public class GenericRepository<T> : DataRepository<T, DataContext>, IGenericRepository<T> where T : class, IBaseEntity
{
    public GenericRepository(DataContext context, ICurrentUserService userService) : base(context, userService)
    {
    }
}
