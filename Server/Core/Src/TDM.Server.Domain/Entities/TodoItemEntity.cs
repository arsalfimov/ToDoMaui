using TDM.Api.Enum;
using TDM.Domain.Common;

namespace TDM.Domain.Entities;

public class TodoItemEntity : BaseEntity
{
    public required string Title { get; set; }
    
    public string? Details { get; set; }
    
    public long? DueDate { get; set; }
    
    public TodoStatus Status { get; set; } = TodoStatus.NotStarted;
    
    public Priority Priority { get; set; } = Priority.Low;
    
    public long? ContactId { get; set; }
    
    public ContactEntity? Contact { get; set; }
    
    public long? CompletedAt { get; set; }

    /// <summary>
    /// Фабрика для создания новой задачи с валидацией
    /// </summary>
    /// <param name="title">Заголовок задачи</param>
    /// <param name="details">Описание (опционально)</param>
    /// <param name="dueDate">Срок выполнения (опционально)</param>
    /// <param name="status">Статус (по умолчанию NotStarted)</param>
    /// <param name="priority">Приоритет (по умолчанию Medium)</param>
    /// <param name="contactId">ID контакта (опционально)</param>
    /// <param name="description">Описание сущности (опционально)</param>
    /// <returns>Новый экземпляр TodoItemEntity</returns>
    public static TodoItemEntity Create(
        string title,
        string? details = null,
        long? dueDate = null,
        TodoStatus status = TodoStatus.NotStarted,
        Priority priority = Priority.Low,
        long? contactId = null,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Заголовок задачи не должен превышать 200 символов", nameof(title));

        if (contactId.HasValue && contactId.Value <= 0)
            throw new ArgumentException("ID контакта должен быть положительным числом", nameof(contactId));

        if (dueDate.HasValue && dueDate.Value < 0)
            throw new ArgumentException("Срок выполнения не может быть отрицательным", nameof(dueDate));

        return new TodoItemEntity
        {
            Title = title,
            Details = details,
            DueDate = dueDate,
            Status = status,
            Priority = priority,
            ContactId = contactId,
            Description = description
        };
    }

    /// <summary>
    /// Обновить информацию о задаче
    /// </summary>
    /// <param name="title">Новый заголовок</param>
    /// <param name="details">Новое описание</param>
    /// <param name="dueDate">Новый срок выполнения</param>
    /// <param name="status">Новый статус</param>
    /// <param name="priority">Новый приоритет</param>
    /// <param name="contactId">Новый ID контакта</param>
    /// <param name="description">Новое описание сущности</param>
    public void Update(
        string title,
        string? details = null,
        long? dueDate = null,
        TodoStatus status = TodoStatus.NotStarted,
        Priority priority = Priority.Low,
        long? contactId = null,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Заголовок задачи не должен превышать 200 символов", nameof(title));

        if (contactId.HasValue && contactId.Value <= 0)
            throw new ArgumentException("ID контакта должен быть положительным числом", nameof(contactId));

        if (dueDate.HasValue && dueDate.Value < 0)
            throw new ArgumentException("Срок выполнения не может быть отрицательным", nameof(dueDate));

        Title = title;
        Details = details;
        DueDate = dueDate;
        Status = status;
        Priority = priority;
        ContactId = contactId;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    
    public void Complete()
    {
        Status = TodoStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        UpdatedAt = CompletedAt.Value;
    }

    public void Cancel()
    {
        Status = TodoStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
