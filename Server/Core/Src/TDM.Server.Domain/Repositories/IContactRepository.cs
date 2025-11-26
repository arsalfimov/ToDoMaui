using TDM.Domain.Common;
using TDM.Domain.Entities;

namespace TDM.Domain.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с контактами
/// </summary>
public interface IContactRepository : IRepositoryMarker
{
    /// <summary>
    /// Получить контакт по ID
    /// </summary>
    /// <param name="id">ID контакта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Контакт или null если не найден</returns>
    Task<ContactEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить список контактов по их ID
    /// </summary>
    /// <param name="ids">Коллекция ID контактов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция найденных контактов (неизменяемая)</returns>
    Task<IReadOnlyCollection<ContactEntity>> GetByIdsAsync(
        IReadOnlyCollection<long> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все контакты
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция всех контактов</returns>
    Task<IReadOnlyCollection<ContactEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить контакты по имени (частичное совпадение)
    /// </summary>
    /// <param name="name">Текст для поиска в имени</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция найденных контактов</returns>
    Task<IReadOnlyCollection<ContactEntity>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить контакт по email
    /// </summary>
    /// <param name="email">Email контакта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Контакт или null если не найден</returns>
    Task<ContactEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить новый контакт (не сохраняет в БД)
    /// </summary>
    /// <param name="entity">Сущность контакта</param>
    void Add(ContactEntity entity);

    /// <summary>
    /// Обновить контакт (не сохраняет в БД)
    /// </summary>
    /// <param name="entity">Сущность контакта</param>
    void Update(ContactEntity entity);

    /// <summary>
    /// Удалить контакт (не сохраняет в БД)
    /// </summary>
    /// <param name="entity">Сущность контакта</param>
    void Delete(ContactEntity entity);

    /// <summary>
    /// Удалить несколько контактов по ID
    /// </summary>
    /// <param name="ids">Коллекция ID контактов для удаления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteRangeAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
}
