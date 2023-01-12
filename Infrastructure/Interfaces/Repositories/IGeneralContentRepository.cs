namespace Biugra.Infrastructure.Interfaces.Repositories
{
    public interface IGeneralContentRepository<T> : IDataRepository<T> where T : class, IBaseEntity
    {
    }
}
