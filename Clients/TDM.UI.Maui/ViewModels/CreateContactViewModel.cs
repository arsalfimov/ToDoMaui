using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDM.Api.Client.Clients;
using TDM.Api.Contracts.Contacts;
using TDM.UI.Maui.Common;

namespace TDM.UI.Maui.ViewModels;

/// <summary>
/// ViewModel for creating a new contact.
/// </summary>
public partial class CreateContactViewModel : BaseViewModel
{
    private readonly IContactsApiClient _contactsClient;

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
    private string? _firstNameError;

    [ObservableProperty]
    private string? _lastNameError;

    [ObservableProperty]
    private string? _phoneError;

    [ObservableProperty]
    private string? _emailError;

    [ObservableProperty]
    private string? _addressError;

    public CreateContactViewModel(IContactsApiClient contactsClient)
    {
        _contactsClient = contactsClient ?? throw new ArgumentNullException(nameof(contactsClient));
    }

    /// <summary>
    /// Validates all form fields.
    /// </summary>
    private bool ValidateForm()
    {
        bool isValid = true;
        ClearValidationErrors();

        // Validate FirstName
        if (!ValidationHelper.IsRequired(FirstName))
        {
            FirstNameError = "Имя контакта обязательно.";
            isValid = false;
        }
        else if (!ValidationHelper.IsValidLength(FirstName, 100))
        {
            FirstNameError = "Имя не должно превышать 100 символов.";
            isValid = false;
        }

        // Validate LastName
        if (!ValidationHelper.IsRequired(LastName))
        {
            LastNameError = "Фамилия контакта обязательна.";
            isValid = false;
        }
        else if (!ValidationHelper.IsValidLength(LastName, 100))
        {
            LastNameError = "Фамилия не должна превышать 100 символов.";
            isValid = false;
        }

        // Validate Phone
        if (!string.IsNullOrWhiteSpace(Phone))
        {
            if (!ValidationHelper.IsValidLength(Phone, 20))
            {
                PhoneError = "Телефон не должен превышать 20 символов.";
                isValid = false;
            }
            else if (!ValidationHelper.IsValidPhone(Phone))
            {
                PhoneError = "Некорректный формат номера телефона. Пример: +7 (999) 999-99-99";
                isValid = false;
            }
        }

        // Validate Email
        if (!string.IsNullOrWhiteSpace(Email))
        {
            if (!ValidationHelper.IsValidLength(Email, 200))
            {
                EmailError = "Email не должен превышать 200 символов.";
                isValid = false;
            }
            else if (!ValidationHelper.IsValidEmail(Email))
            {
                EmailError = "Некорректный формат email адреса.";
                isValid = false;
            }
        }

        // Validate Address
        if (!ValidationHelper.IsValidLength(Address, 500))
        {
            AddressError = "Адрес не должен превышать 500 символов.";
            isValid = false;
        }

        return isValid;
    }

    /// <summary>
    /// Clears all validation errors.
    /// </summary>
    private void ClearValidationErrors()
    {
        FirstNameError = null;
        LastNameError = null;
        PhoneError = null;
        EmailError = null;
        AddressError = null;
    }

    /// <summary>
    /// Creates a new contact.
    /// </summary>
    [RelayCommand]
    private async Task CreateContactAsync()
    {
        if (IsBusy) return;

        ClearError();

        if (!ValidateForm())
        {
            SetError("Пожалуйста, исправьте ошибки в форме.");
            return;
        }

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Создание контакта...");

            var request = new CreateContactRequest(
                FirstName: FirstName,
                LastName: LastName,
                Phone: Phone,
                Email: Email,
                Address: Address,
                Description: Description
            );

            await _contactsClient.CreateAsync(request);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetError($"Ошибка создания контакта: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
        }
    }

    /// <summary>
    /// Cancels contact creation and navigates back.
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
