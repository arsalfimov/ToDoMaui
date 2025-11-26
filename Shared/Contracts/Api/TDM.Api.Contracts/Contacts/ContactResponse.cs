namespace TDM.Api.Contracts.Contacts;

public record ContactResponse(
    long Id,
    string FirstName,
    string LastName,
    string? Phone,
    string? Email,
    string? Address,
    string? Description,
    long CreatedAt,
    long UpdatedAt);
