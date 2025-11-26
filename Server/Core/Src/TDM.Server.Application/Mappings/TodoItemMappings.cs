using Riok.Mapperly.Abstractions;
using TDM.Api.Contracts.TodoItems;
using TDM.Domain.Entities;
using TDM.Server.Application.Features.TodoItems.Commands;

namespace TDM.Server.Application.Mappings;

[Mapper]
public static partial class TodoItemMappings
{
    public static partial CreateTodoItemCommand ToCommand(this CreateTodoItemRequest request);
    
    public static UpdateTodoItemCommand ToCommand(this UpdateTodoItemRequest request, long id)
        => new UpdateTodoItemCommand(id, request.Title, request.Details, request.DueDate, 
            request.Status, request.Priority, request.ContactId, request.Description);
    
    public static TodoItemResponse ToResponse(this TodoItemEntity entity)
    {
        var contactName = entity.Contact != null 
            ? $"{entity.Contact.FirstName} {entity.Contact.LastName}"
            : null;
            
        return new TodoItemResponse(
            Id: entity.Id,
            Title: entity.Title,
            Details: entity.Details,
            DueDate: entity.DueDate,
            Status: (int)entity.Status,
            Priority: (int)entity.Priority,
            ContactId: entity.ContactId,
            ContactName: contactName,
            CompletedAt: entity.CompletedAt,
            Description: entity.Description,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt
        );
    }
}
