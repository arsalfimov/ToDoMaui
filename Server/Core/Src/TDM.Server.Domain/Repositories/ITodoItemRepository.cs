using TDM.Api.Enum;
using TDM.Domain.Common;
using TDM.Domain.Entities;

namespace TDM.Domain.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с задачами (TodoItems)
/// </summary>
public interface ITodoItemRepository : IRepositoryMarker
{
    /// <summary>
    /// Получить задачу по ID
    /// </summary>
    /// <param name="id">ID задачи</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Задача или null если не найдена</returns>
    Task<TodoItemEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить список задач по их ID
    /// </summary>
    /// <param name="ids">Коллекция ID задач</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция найденных задач (неизменяемая)</returns>
    Task<IReadOnlyCollection<TodoItemEntity>> GetByIdsAsync(
        IReadOnlyCollection<long> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все задачи
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция всех задач</returns>
    Task<IReadOnlyCollection<TodoItemEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все задачи контакта
    /// </summary>
    /// <param name="contactId">ID контакта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция задач контакта</returns>
    Task<IReadOnlyCollection<TodoItemEntity>> GetByContactIdAsync(
        long contactId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить задачи по статусу
    /// </summary>
    /// <param name="status">Статус задачи</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция задач с указанным статусом</returns>
    Task<IReadOnlyCollection<TodoItemEntity>> GetByStatusAsync(
        TodoStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить задачи по приоритету
    /// </summary>
    /// <param name="priority">Приоритет задачи</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция задач с указанным приоритетом</returns>
    Task<IReadOnlyCollection<TodoItemEntity>> GetByPriorityAsync(
        Priority priority,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить просроченные задачи (срок выполнения в прошлом и статус не Completed/Cancelled)
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция просроченных задач</returns>
    Task<IReadOnlyCollection<TodoItemEntity>> GetOverdueAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить задачи на сегодня (срок выполнения сегодня)
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция задач на сегодня</returns>
    Task<IReadOnlyCollection<TodoItemEntity>> GetTodayAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить задачи по заголовку (частичное совпадение)
    /// </summary>
    /// <param name="title">Текст для поиска в заголовке</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция найденных задач</returns>
    Task<IReadOnlyCollection<TodoItemEntity>> GetByTitleAsync(
        string title,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить новую задачу (не сохраняет в БД)
    /// </summary>
    /// <param name="entity">Сущность задачи</param>
    void Add(TodoItemEntity entity);

    /// <summary>
    /// Обновить задачу (не сохраняет в БД)
    /// </summary>
    /// <param name="entity">Сущность задачи</param>
    void Update(TodoItemEntity entity);

    /// <summary>
    /// Удалить задачу (не сохраняет в БД)
    /// </summary>
    /// <param name="entity">Сущность задачи</param>
    void Delete(TodoItemEntity entity);

    /// <summary>
    /// Удалить несколько задач по ID
    /// </summary>
    /// <param name="ids">Коллекция ID задач для удаления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteRangeAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
}
