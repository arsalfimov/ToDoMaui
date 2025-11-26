using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDM.Api.Client.Clients;
using TDM.Api.Contracts.Contacts;
using TDM.UI.Maui.Common;

namespace TDM.UI.Maui.ViewModels;

/// <summary>
/// ViewModel for managing the contacts list.
/// </summary>
public partial class ContactsListViewModel : BaseViewModel
{
    private readonly IContactsApiClient _contactsClient;

    [ObservableProperty]
    private ObservableCollection<ContactResponse> _contacts = [];

    [ObservableProperty]
    private ContactResponse? _selectedContact;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _isRefreshing;

    public ContactsListViewModel(IContactsApiClient contactsClient)
    {
        _contactsClient = contactsClient ?? throw new ArgumentNullException(nameof(contactsClient));
    }

    /// <summary>
    /// Loads all contacts from the API.
    /// </summary>
    [RelayCommand]
    private async Task LoadContactsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Загрузка контактов...");

            var contacts = await _contactsClient.GetAllAsync();
            Contacts = new ObservableCollection<ContactResponse>(contacts);
        }
        catch (Exception ex)
        {
            SetError($"Ошибка загрузки контактов: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
            IsRefreshing = false;
        }
    }

    /// <summary>
    /// Searches contacts by name.
    /// </summary>
    [RelayCommand]
    private async Task SearchContactsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadContactsAsync();
                return;
            }

            var contacts = await _contactsClient.SearchByNameAsync(SearchText);
            Contacts = new ObservableCollection<ContactResponse>(contacts);
        }
        catch (Exception ex)
        {
            SetError($"Ошибка поиска: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Navigates to contact details page.
    /// </summary>
    [RelayCommand]
    private async Task NavigateToContactDetailsAsync(ContactResponse? contact)
    {
        if (contact == null) return;

        await Shell.Current.GoToAsync(nameof(Views.ContactDetailsPage), new Dictionary<string, object>
        {
            ["Contact"] = contact
        });
    }

    /// <summary>
    /// Navigates to create contact page.
    /// </summary>
    [RelayCommand]
    private async Task NavigateToCreateContactAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CreateContactPage));
    }

    /// <summary>
    /// Deletes a contact.
    /// </summary>
    [RelayCommand]
    private async Task DeleteContactAsync(long contactId)
    {
        if (IsBusy) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "Подтверждение",
            "Вы уверены, что хотите удалить этот контакт?",
            "Да",
            "Нет");

        if (!confirmed) return;

        try
        {
            IsBusy = true;
            ClearError();

            await _contactsClient.DeleteAsync(contactId);
            await LoadContactsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Ошибка удаления контакта: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Refreshes the contacts list (pull-to-refresh).
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadContactsAsync();
    }
}
