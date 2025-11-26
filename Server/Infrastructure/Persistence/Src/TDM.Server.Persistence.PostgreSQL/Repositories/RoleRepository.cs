using Microsoft.EntityFrameworkCore;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;

namespace TDM.Server.Persistence.PostgreSQL.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _dbContext;

    public RoleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<RoleEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<RoleEntity>> GetByIdsAsync(
        IReadOnlyCollection<long> ids,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .Where(r => ids.Contains(r.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<RoleEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .ToListAsync(cancellationToken);
    }

    public async Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public void Add(RoleEntity entity)
    {
        _dbContext.Roles.Add(entity);
    }

    public void Update(RoleEntity entity)
    {
        _dbContext.Roles.Update(entity);
    }

    public void Delete(RoleEntity entity)
    {
        _dbContext.Roles.Remove(entity);
    }

    public async Task DeleteRangeAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Roles
            .Where(r => ids.Contains(r.Id))
            .ToListAsync(cancellationToken);

        _dbContext.Roles.RemoveRange(entities);
    }
}
