using TDM.Domain.Common;

namespace TDM.Domain.Entities;

public class RoleEntity : BaseEntity
{
    public required string Name { get; set; }

    public ICollection<UserEntity> Users { get; } = [];

    public static RoleEntity Create(
        string name,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new RoleEntity
        {
            Name = name,
            Description = description
        };
    }

    public void Update(
        string name,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
