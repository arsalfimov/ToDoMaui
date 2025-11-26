using TDM.Domain.Common;
using TDM.Domain.Entities;

namespace TDM.Domain.Repositories;

public interface IRoleRepository : IRepositoryMarker
{
    Task<RoleEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RoleEntity>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RoleEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    void Add(RoleEntity entity);
    void Update(RoleEntity entity);
    void Delete(RoleEntity entity);
    Task DeleteRangeAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
}
