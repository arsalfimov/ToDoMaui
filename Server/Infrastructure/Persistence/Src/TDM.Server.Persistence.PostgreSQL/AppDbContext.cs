using Microsoft.EntityFrameworkCore;
using TDM.Domain.Common;
using TDM.Domain.Entities;

namespace TDM.Server.Persistence.PostgreSQL;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<ContactEntity> Contacts { get; set; } = null!;
    public DbSet<TodoItemEntity> TodoItems { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<RoleEntity> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
