using TDM.Api.Client.Common;
using TDM.Api.Contracts.Contacts;
using Refit;

namespace TDM.Api.Client.Clients;

/// <summary>
/// Refit client for managing contacts.
/// </summary>
public interface IContactsApiClient : IApiClientMarker
{
    /// <summary>
    /// Get all contacts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of all contacts.</returns>
    [Get("/api/contacts")]
    Task<IReadOnlyCollection<ContactResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a contact by its identifier.
    /// </summary>
    /// <param name="id">The contact identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The contact with the specified identifier.</returns>
    [Get("/api/contacts/{id}")]
    Task<ContactResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search contacts by name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of contacts matching the name.</returns>
    [Get("/api/contacts/search/name")]
    Task<IReadOnlyCollection<ContactResponse>> SearchByNameAsync([Query] string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a contact by email address.
    /// </summary>
    /// <param name="email">The email address.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The contact with the specified email.</returns>
    [Get("/api/contacts/search/email")]
    Task<ContactResponse> GetByEmailAsync([Query] string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new contact.
    /// </summary>
    /// <param name="request">The contact payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created contact.</returns>
    [Post("/api/contacts")]
    Task<ContactResponse> CreateAsync([Body] CreateContactRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing contact.
    /// </summary>
    /// <param name="id">The identifier of the contact to update.</param>
    /// <param name="request">The updated contact payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated contact.</returns>
    [Put("/api/contacts/{id}")]
    Task<ContactResponse> UpdateAsync(long id, [Body] UpdateContactRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a contact by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the contact to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [Delete("/api/contacts/{id}")]
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete multiple contacts by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the contacts to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of deleted contacts.</returns>
    [Post("/api/contacts/delete-range")]
    Task<int> DeleteRangeAsync([Body] IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
}

