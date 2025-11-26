using Microsoft.EntityFrameworkCore;
using TDM.Domain.Entities;
using TDM.Domain.Repositories;

namespace TDM.Server.Persistence.PostgreSQL.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _dbContext;

    public ContactRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<ContactEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Contacts
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<ContactEntity>> GetByIdsAsync(
        IReadOnlyCollection<long> ids,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Contacts
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ContactEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Contacts
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ContactEntity>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Contacts
            .Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name))
            .ToListAsync(cancellationToken);
    }

    public async Task<ContactEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Contacts
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public void Add(ContactEntity entity)
    {
        _dbContext.Contacts.Add(entity);
    }

    public void Update(ContactEntity entity)
    {
        _dbContext.Contacts.Update(entity);
    }

    public void Delete(ContactEntity entity)
    {
        _dbContext.Contacts.Remove(entity);
    }

    public async Task DeleteRangeAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Contacts
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken);

        _dbContext.Contacts.RemoveRange(entities);
    }
}
