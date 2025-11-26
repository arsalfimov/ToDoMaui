using TDM.Domain.Common;

namespace TDM.Domain.Entities;

public class UserEntity : BaseEntity
{
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    public required bool IsBlocked { get; set; }

    public long RoleId { get; set; }
    public RoleEntity Role { get; set; } = null!;

    public static UserEntity Create(
        string login,
        string passwordHash,
        bool isBlocked,
        long roleId,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(login);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(roleId);

        return new UserEntity
        {
            Login = login,
            PasswordHash = passwordHash,
            IsBlocked = isBlocked,
            RoleId = roleId,
            Description = description
        };
    }

    public void Update(
        string login,
        bool isBlocked,
        long roleId,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(login);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(roleId);

        Login = login;
        IsBlocked = isBlocked;
        RoleId = roleId;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public void UpdatePassword(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        
        PasswordHash = passwordHash;
        UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
