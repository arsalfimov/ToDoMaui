using Microsoft.EntityFrameworkCore;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;

namespace TDM.Server.Persistence.PostgreSQL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<UserEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserEntity>> GetByIdsAsync(
        IReadOnlyCollection<long> ids,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserEntity?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserEntity>> GetByRoleIdAsync(
        long roleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Where(u => u.RoleId == roleId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserEntity>> GetBlockedUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Where(u => u.IsBlocked)
            .ToListAsync(cancellationToken);
    }

    public void Add(UserEntity entity)
    {
        _dbContext.Users.Add(entity);
    }

    public void Update(UserEntity entity)
    {
        _dbContext.Users.Update(entity);
    }

    public void Delete(UserEntity entity)
    {
        _dbContext.Users.Remove(entity);
    }

    public async Task DeleteRangeAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Users
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(cancellationToken);

        _dbContext.Users.RemoveRange(entities);
    }
}
