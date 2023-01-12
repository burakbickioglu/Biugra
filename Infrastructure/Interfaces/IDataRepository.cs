

using Biugra.Domain.Models.Dtos;

namespace Infrastructure.Interfaces;

public interface IDataRepository<T> where T:class, IBaseEntity
{
    IQueryable<T> GetAll();
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> expression);
    IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? expression = null);
    Task<T> GetFirst(Expression<Func<T, bool>> expression);
    Task<T> GetFirstTracked(Expression<Func<T, bool>> expression);
    Task<bool> ValidateDuplicate(Expression<Func<T, bool>> expression);
    Task<bool> CheckAny(Expression<Func<T, bool>> expression);
    Task<T> Get(Guid id);
    Task<CommandResult<T>> AddorUpdate(T entity);
    Task<CommandResult<T>> Add(T entity);
    Task<CommandResult<List<T>>> AddMany(List<T> entities);
    Task<CommandResult<T>> Update(T entity);
    Task<CommandResult<List<T>>> UpdateMany(List<T> entities);
    Task<CommandResult<T>> Delete(Guid id);
    Task<CommandResult<T>> SoftDelete(Guid id);
    Task<CommandResult<List<T>>> DeleteMany(IEnumerable<Guid> ids);
    Task<CommandResult<T>> SaveAsync(T data);
    string GetUserId();
    void DisposeEntity(T data);
}
