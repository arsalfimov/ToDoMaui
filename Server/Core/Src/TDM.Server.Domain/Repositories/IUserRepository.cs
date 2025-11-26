using TDM.Domain.Common;
using TDM.Domain.Entities;

namespace TDM.Domain.Repositories;

public interface IUserRepository : IRepositoryMarker
{
    Task<UserEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UserEntity>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UserEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UserEntity>> GetByRoleIdAsync(long roleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UserEntity>> GetBlockedUsersAsync(CancellationToken cancellationToken = default);
    
    void Add(UserEntity entity);
    void Update(UserEntity entity);
    void Delete(UserEntity entity);
    Task DeleteRangeAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
}
