using TDM.Api.Client.Common;
using TDM.Api.Enum;
using Refit;
using TDM.Api.Contracts.TodoItems;

namespace TDM.Api.Client.Clients;

/// <summary>
/// Refit client for managing todo items.
/// </summary>
public interface ITodoItemsApiClient : IApiClientMarker
{
    /// <summary>
    /// Get all todo items.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of all todo items.</returns>
    [Get("/api/to-do-items")]
    Task<IReadOnlyCollection<TodoItemResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a todo item by its identifier.
    /// </summary>
    /// <param name="id">The todo item identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The todo item with the specified identifier.</returns>
    [Get("/api/to-do-items/{id}")]
    Task<TodoItemResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get todo items by contact identifier.
    /// </summary>
    /// <param name="contactId">The contact identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of todo items for the specified contact.</returns>
    [Get("/api/to-do-items/contact/{contactId}")]
    Task<IReadOnlyCollection<TodoItemResponse>> GetByContactIdAsync(long contactId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get todo items by status.
    /// </summary>
    /// <param name="status">The todo status.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of todo items with the specified status.</returns>
    [Get("/api/to-do-items/status/{status}")]
    Task<IReadOnlyCollection<TodoItemResponse>> GetByStatusAsync(TodoStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get todo items by priority.
    /// </summary>
    /// <param name="priority">The priority level.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of todo items with the specified priority.</returns>
    [Get("/api/to-do-items/priority/{priority}")]
    Task<IReadOnlyCollection<TodoItemResponse>> GetByPriorityAsync(Priority priority, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get overdue todo items.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of overdue todo items.</returns>
    [Get("/api/to-do-items/overdue")]
    Task<IReadOnlyCollection<TodoItemResponse>> GetOverdueAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get today's todo items.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of today's todo items.</returns>
    [Get("/api/to-do-items/today")]
    Task<IReadOnlyCollection<TodoItemResponse>> GetTodayAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Search todo items by title.
    /// </summary>
    /// <param name="title">The title to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of todo items matching the title.</returns>
    [Get("/api/to-do-items/search")]
    Task<IReadOnlyCollection<TodoItemResponse>> SearchByTitleAsync([Query] string title, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new todo item.
    /// </summary>
    /// <param name="request">The todo item payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created todo item.</returns>
    [Post("/api/to-do-items")]
    Task<TodoItemResponse> CreateAsync([Body] CreateTodoItemRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing todo item.
    /// </summary>
    /// <param name="id">The identifier of the todo item to update.</param>
    /// <param name="request">The updated todo item payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated todo item.</returns>
    [Put("/api/to-do-items/{id}")]
    Task<TodoItemResponse> UpdateAsync(long id, [Body] UpdateTodoItemRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mark a todo item as complete.
    /// </summary>
    /// <param name="id">The identifier of the todo item to complete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated todo item.</returns>
    [Put("/api/to-do-items/{id}/complete")]
    Task<TodoItemResponse> CompleteAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancel a todo item.
    /// </summary>
    /// <param name="id">The identifier of the todo item to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated todo item.</returns>
    [Put("/api/to-do-items/{id}/cancel")]
    Task<TodoItemResponse> CancelAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a todo item by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the todo item to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [Delete("/api/to-do-items/{id}")]
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete multiple todo items by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the todo items to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of deleted todo items.</returns>
    [Post("/api/to-do-items/delete-range")]
    Task<int> DeleteRangeAsync([Body] IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
}