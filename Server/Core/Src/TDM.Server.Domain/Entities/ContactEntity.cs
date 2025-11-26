using TDM.Domain.Common;
namespace TDM.Domain.Entities;

public class ContactEntity : BaseEntity
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Email { get; set; }
    
    public string? Address { get; set; }
    
    public ICollection<TodoItemEntity> TodoItems { get; set; } = new List<TodoItemEntity>();

    /// <summary>
    /// Фабрика для создания нового контакта с валидацией
    /// </summary>
    /// <param name="firstName">Имя контакта</param>
    /// <param name="lastName">Фамилия контакта</param>
    /// <param name="phone">Телефон (опционально)</param>
    /// <param name="email">Email (опционально)</param>
    /// <param name="address">Адрес (опционально)</param>
    /// <param name="description">Описание (опционально)</param>
    /// <returns>Новый экземпляр ContactEntity</returns>
    public static ContactEntity Create(
        string firstName,
        string lastName,
        string? phone = null,
        string? email = null,
        string? address = null,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));

        if (firstName.Length > 100)
            throw new ArgumentException("Имя контакта не должно превышать 100 символов", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("Фамилия контакта не должна превышать 100 символов", nameof(lastName));

        if (!string.IsNullOrWhiteSpace(phone) && phone.Length > 20)
            throw new ArgumentException("Телефон не должен превышать 20 символов", nameof(phone));

        if (!string.IsNullOrWhiteSpace(email) && email.Length > 200)
            throw new ArgumentException("Email не до��жен превышать 200 символов", nameof(email));

        if (!string.IsNullOrWhiteSpace(address) && address.Length > 500)
            throw new ArgumentException("Адрес не должен превышать 500 символов", nameof(address));

        return new ContactEntity
        {
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Email = email,
            Address = address,
            Description = description
        };
    }

    /// <summary>
    /// Обновить информацию о контакте
    /// </summary>
    /// <param name="firstName">Новое имя</param>
    /// <param name="lastName">Новая фамилия</param>
    /// <param name="phone">Новый телефон</param>
    /// <param name="email">Новый email</param>
    /// <param name="address">Новый адрес</param>
    /// <param name="description">Новое описание</param>
    public void Update(
        string firstName,
        string lastName,
        string? phone = null,
        string? email = null,
        string? address = null,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));

        if (firstName.Length > 100)
            throw new ArgumentException("Имя контакта не должно превышать 100 символов", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("Фамилия контакта не должна превышать 100 символов", nameof(lastName));

        if (!string.IsNullOrWhiteSpace(phone) && phone.Length > 20)
            throw new ArgumentException("Телефон не должен превышать 20 символов", nameof(phone));

        if (!string.IsNullOrWhiteSpace(email) && email.Length > 200)
            throw new ArgumentException("Email не должен превышать 200 символов", nameof(email));

        if (!string.IsNullOrWhiteSpace(address) && address.Length > 500)
            throw new ArgumentException("Адрес не должен превышать 500 символов", nameof(address));

        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        Address = address;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
