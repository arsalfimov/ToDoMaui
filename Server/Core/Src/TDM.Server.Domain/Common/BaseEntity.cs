namespace TDM.Domain.Common;

public abstract class BaseEntity
{
    public long Id { get; set; }
    
    public string? Description { get; set; }
    
    public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    
    public long UpdatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
}
