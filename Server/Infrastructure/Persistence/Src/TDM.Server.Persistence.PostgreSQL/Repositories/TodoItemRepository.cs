using Microsoft.EntityFrameworkCore;
using TDM.Api.Enum;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;

namespace TDM.Server.Persistence.PostgreSQL.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly AppDbContext _dbContext;

    public TodoItemRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<TodoItemEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItemEntity>> GetByIdsAsync(
        IReadOnlyCollection<long> ids,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItemEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItemEntity>> GetByContactIdAsync(
        long contactId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .Where(t => t.ContactId == contactId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItemEntity>> GetByStatusAsync(
        TodoStatus status,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .Where(t => t.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItemEntity>> GetByPriorityAsync(
        Priority priority,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .Where(t => t.Priority == priority)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItemEntity>> GetOverdueAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .Where(t => t.DueDate != null && t.DueDate < now &&
                        t.Status != TodoStatus.Completed && t.Status != TodoStatus.Cancelled)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItemEntity>> GetTodayAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var todayStart = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset).ToUnixTimeSeconds();
        var todayEnd = new DateTimeOffset(now.Year, now.Month, now.Day, 23, 59, 59, now.Offset).ToUnixTimeSeconds();

        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .Where(t => t.DueDate != null && t.DueDate >= todayStart && t.DueDate <= todayEnd)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TodoItemEntity>> GetByTitleAsync(
        string title,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodoItems
            .Include(t => t.Contact)
            .Where(t => t.Title.Contains(title))
            .ToListAsync(cancellationToken);
    }

    public void Add(TodoItemEntity entity)
    {
        _dbContext.TodoItems.Add(entity);
    }

    public void Update(TodoItemEntity entity)
    {
        _dbContext.TodoItems.Update(entity);
    }

    public void Delete(TodoItemEntity entity)
    {
        _dbContext.TodoItems.Remove(entity);
    }

    public async Task DeleteRangeAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.TodoItems
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(cancellationToken);

        _dbContext.TodoItems.RemoveRange(entities);
    }
}
