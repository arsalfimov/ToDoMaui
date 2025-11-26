using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDM.Api.Client.Clients;
using TDM.Api.Contracts.Contacts;
using TDM.UI.Maui.Common;

namespace TDM.UI.Maui.ViewModels;

/// <summary>
/// ViewModel for viewing and editing contact details.
/// </summary>
[QueryProperty(nameof(Contact), "Contact")]
public partial class ContactDetailsViewModel : BaseViewModel
{
    private readonly IContactsApiClient _contactsClient;

    [ObservableProperty]
    private ContactResponse? _contact;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string? _phone;

    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private string? _address;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private bool _isEditMode;

    [ObservableProperty]
    private string? _phoneError;

    [ObservableProperty]
    private string? _emailError;

    public ContactDetailsViewModel(IContactsApiClient contactsClient)
    {
        _contactsClient = contactsClient ?? throw new ArgumentNullException(nameof(contactsClient));
    }

    partial void OnContactChanged(ContactResponse? value)
    {
        if (value == null) return;

        FirstName = value.FirstName;
        LastName = value.LastName;
        Phone = value.Phone;
        Email = value.Email;
        Address = value.Address;
        Description = value.Description;
    }

    /// <summary>
    /// Toggles edit mode.
    /// </summary>
    [RelayCommand]
    private void ToggleEditMode()
    {
        IsEditMode = !IsEditMode;
    }

    /// <summary>
    /// Saves the contact changes.
    /// </summary>
    [RelayCommand]
    private async Task SaveContactAsync()
    {
        if (Contact == null || IsBusy) return;

        // Очищаем предыдущие ошибки
        PhoneError = null;
        EmailError = null;
        ClearError();

        bool hasErrors = false;

        if (string.IsNullOrWhiteSpace(FirstName))
        {
            SetError("Пожалуйста, заполните имя контакта.");
            return;
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            SetError("Пожалуйста, заполните фамилию контакта.");
            return;
        }

        // Валидация телефона
        if (!string.IsNullOrWhiteSpace(Phone))
        {
            if (Phone.Length > 20)
            {
                PhoneError = "Телефон не должен превышать 20 символов.";
                hasErrors = true;
            }
            else if (!ValidationHelper.IsValidPhone(Phone))
            {
                PhoneError = "Некорректный формат номера телефона. Пример: +7 (999) 999-99-99";
                hasErrors = true;
            }
        }

        // Валидация email
        if (!string.IsNullOrWhiteSpace(Email))
        {
            if (Email.Length > 200)
            {
                EmailError = "Email не должен превышать 200 символов.";
                hasErrors = true;
            }
            else if (!ValidationHelper.IsValidEmail(Email))
            {
                EmailError = "Некорректный формат email адреса.";
                hasErrors = true;
            }
        }

        if (hasErrors)
        {
            SetError("Пожалуйста, исправьте ошибки в форме.");
            return;
        }

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Сохранение изменений...");

            var request = new UpdateContactRequest(
                FirstName: FirstName,
                LastName: LastName,
                Phone: Phone,
                Email: Email,
                Address: Address,
                Description: Description
            );

            var updatedContact = await _contactsClient.UpdateAsync(Contact.Id, request);
            Contact = updatedContact;
            IsEditMode = false;

            await Shell.Current.DisplayAlert("Успешно", "Контакт обновлен", "OK");
        }
        catch (Exception ex)
        {
            SetError($"Ошибка сохранения: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
        }
    }

    /// <summary>
    /// Cancels editing and restores original values.
    /// </summary>
    [RelayCommand]
    private void CancelEdit()
    {
        if (Contact != null)
        {
            FirstName = Contact.FirstName;
            LastName = Contact.LastName;
            Phone = Contact.Phone;
            Email = Contact.Email;
            Address = Contact.Address;
            Description = Contact.Description;
        }
        IsEditMode = false;
    }

    /// <summary>
    /// Deletes the contact.
    /// </summary>
    [RelayCommand]
    private async Task DeleteContactAsync()
    {
        if (Contact == null || IsBusy) return;

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

            await _contactsClient.DeleteAsync(Contact.Id);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetError($"Ошибка удаления: {ex.Message}");
            IsBusy = false;
        }
    }
}
