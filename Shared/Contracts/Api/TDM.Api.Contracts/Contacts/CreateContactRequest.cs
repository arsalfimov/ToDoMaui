namespace TDM.Api.Contracts.Contacts;

public record CreateContactRequest(
    string FirstName,
    string LastName,
    string? Phone = null,
    string? Email = null,
    string? Address = null,
    string? Description = null);
