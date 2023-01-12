using BaseProject.Domain.Models.Context;
using Biugra.Domain.Models.Dtos;

namespace Infrastructure.Persistance;
public abstract class DataRepository<T, TContext> : IDataRepository<T>
    where T : class, IBaseEntity
    where TContext : DataContext
{
    public readonly TContext context;
    public readonly ICurrentUserService _userService;

    protected DataRepository(TContext context, ICurrentUserService userService)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        _userService = userService;
    }

    public async Task<T> Get(Guid id)
    {
        var data = await context.Set<T>().FindAsync(id);
        if (data == null) return null;

        context.Entry(data).State = EntityState.Detached;
        return data;
    }
    public IQueryable<T> GetAll()
    {
        return context.Set<T>().AsSplitQuery();
    }

    public async Task<int> CountAsync()
    {
        return await context.Set<T>().CountAsync();
    }
    public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().Where(expression).CountAsync();
    }

    public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? expression = null)
    {
        var result = context.Set<T>().Where(s => !s.IsDeleted).OrderByDescending(s => s.ModifiedOn ?? s.CreatedOn).AsSplitQuery();
        if (expression != null)
            result = result.Where(expression);

        return result;
    }
    public async Task<T> GetFirst(Expression<Func<T, bool>> expression)
    {
        var data = await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        if (data == null) return null;

        return data;
    }
    public async Task<T> GetFirstTracked(Expression<Func<T, bool>> expression)
    {
        var data = await context.Set<T>().FirstOrDefaultAsync(expression);
        if (data == null) return null;

        return data;
    }
    public async Task<CommandResult<T>> AddorUpdate(T entity)
    {
        return entity.Id == Guid.Empty ? await Add(entity) : await Update(entity);
    }
    public async Task<CommandResult<T>> Add(T entity)
    {
        entity.Created(GetUserId());
        context.Set<T>().Add(entity);
        return await SaveAsync(entity);
    }
    public async Task<CommandResult<List<T>>> AddMany(List<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.Created(GetUserId());
        }
        context.Set<T>().AddRange(entities);
        return await SaveManyAsync(entities);
    }
    public async Task<CommandResult<T>> Update(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        context.Entry(entity).Property(x => x.CreatedOn).IsModified = false;
        context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        entity.Modified(GetUserId());

        return await SaveAsync(entity);
    }
    public async Task<CommandResult<List<T>>> UpdateMany(List<T> entities)
    {
        foreach (var entity in entities)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.Entry(entity).Property(x => x.CreatedOn).IsModified = false;
            context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            entity.Modified(GetUserId());
        }
        return await SaveManyAsync(entities);
    }
    public async Task<CommandResult<T>> Delete(Guid id)
    {
        var entity = await context.Set<T>().FindAsync(id);
        if (entity == null)
            return CommandResult<T>.GetFailed("");

        context.Set<T>().Remove(entity);
        return await SaveAsync(entity);
    }
    public async Task<CommandResult<T>> SoftDelete(Guid id)
    {
        var entity = await context.Set<T>().FindAsync(id);
        if (entity == null)
            return CommandResult<T>.GetFailed("");

        context.Entry(entity).State = EntityState.Modified;
        context.Entry(entity).Property(x => x.CreatedOn).IsModified = false;
        context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        entity.Deleted(GetUserId());

        return await SaveAsync(entity);
    }
    public async Task<CommandResult<List<T>>> DeleteMany(IEnumerable<Guid> ids)
    {
        var entities = await context.Set<T>().Where(s => ids.Contains(s.Id)).ToListAsync();
        if (!entities.Any())
        {
            return CommandResult<List<T>>.GetFailed("Not found", entities);
        }

        context.Set<T>().RemoveRange(entities);
        return await SaveManyAsync(entities);
    }

    public async Task<CommandResult<T>> SaveAsync(T data)
    {
        try
        {
            await context.SaveChangesAsync();
            return CommandResult<T>.GetSucceed(data);
        }
        catch (Exception ex)
        {
            context.Entry(data).State = EntityState.Detached;
            return CommandResult<T>.GetFailed(ex?.Message ?? ex?.InnerException?.Message, data);
        }
    }
    public async Task<CommandResult<List<T>>> SaveManyAsync(List<T> data)
    {
        try
        {
            await context.SaveChangesAsync();
            return CommandResult<List<T>>.GetSucceed(data);
        }
        catch (Exception ex)
        {
            context.Entry(data).State = EntityState.Detached;
            return CommandResult<List<T>>.GetFailed(ex?.InnerException?.Message, data);
        }
    }

    public async Task<bool> ValidateDuplicate(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().AnyAsync(expression);
    }
    public async Task<bool> CheckAny(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().AnyAsync(expression);
    }

    public string GetUserId()
    {
        return _userService.GetUserId();
    }

    public void DisposeEntity(T data)
    {
        context.Entry(data).State = EntityState.Detached;
    }
}
