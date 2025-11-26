using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDM.Api.Client.Clients;
using TDM.Api.Contracts.Contacts;
using TDM.Api.Contracts.TodoItems;
using TDM.Api.Enum;
using TDM.UI.Maui.Common;

namespace TDM.UI.Maui.ViewModels;

/// <summary>
/// ViewModel for creating a new todo item.
/// </summary>
public partial class CreateTodoItemViewModel : BaseViewModel
{
    private readonly ITodoItemsApiClient _todoItemsClient;
    private readonly IContactsApiClient _contactsClient;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string? _details;

    [ObservableProperty]
    private DateTime _dueDate = DateTime.Today.AddDays(7); // По умолчанию через неделю

    [ObservableProperty]
    private bool _hasDueDate; // Флаг, установлена ли дата

    [ObservableProperty]
    private TodoStatus _status = TodoStatus.NotStarted;

    [ObservableProperty]
    private Priority _priority = Priority.Medium;

    [ObservableProperty]
    private long? _contactId;

    [ObservableProperty]
    private ContactResponse? _selectedContact;

    [ObservableProperty]
    private ObservableCollection<ContactResponse> _contacts = [];

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private string? _titleError;

    public CreateTodoItemViewModel(ITodoItemsApiClient todoItemsClient, IContactsApiClient contactsClient)
    {
        _todoItemsClient = todoItemsClient ?? throw new ArgumentNullException(nameof(todoItemsClient));
        _contactsClient = contactsClient ?? throw new ArgumentNullException(nameof(contactsClient));
    }

    /// <summary>
    /// Initializes the ViewModel and loads contacts.
    /// </summary>
    public async Task InitializeAsync()
    {
        await LoadContactsAsync();
    }

    /// <summary>
    /// Loads all contacts for selection.
    /// </summary>
    private async Task LoadContactsAsync()
    {
        try
        {
            var contacts = await _contactsClient.GetAllAsync();
            Contacts = new ObservableCollection<ContactResponse>(contacts.OrderBy(c => c.FirstName).ThenBy(c => c.LastName));
        }
        catch (Exception ex)
        {
            SetError($"Ошибка загрузки контактов: {ex.Message}");
        }
    }

    partial void OnSelectedContactChanged(ContactResponse? value)
    {
        ContactId = value?.Id;
    }

    /// <summary>
    /// Validates the form.
    /// </summary>
    private bool ValidateForm()
    {
        bool isValid = true;
        ClearValidationErrors();

        // Validate Title
        if (!ValidationHelper.IsRequired(Title))
        {
            TitleError = "Заголовок задачи обязателен.";
            isValid = false;
        }
        else if (!ValidationHelper.IsValidLength(Title, 200))
        {
            TitleError = "Заголовок не должен превышать 200 символов.";
            isValid = false;
        }

        return isValid;
    }

    /// <summary>
    /// Clears validation errors.
    /// </summary>
    private void ClearValidationErrors()
    {
        TitleError = null;
    }

    /// <summary>
    /// Creates a new todo item.
    /// </summary>
    [RelayCommand]
    private async Task CreateTodoItemAsync()
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
            SetLoading(true, "Создание задачи...");

            long? dueDateUnix = HasDueDate 
                ? new DateTimeOffset(DueDate).ToUnixTimeSeconds() 
                : null;

            var request = new CreateTodoItemRequest(
                Title: Title,
                Details: Details,
                DueDate: dueDateUnix,
                Status: Status,
                Priority: Priority,
                ContactId: ContactId,
                Description: null
            );

            await _todoItemsClient.CreateAsync(request);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetError($"Ошибка создания задачи: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
        }
    }

    /// <summary>
    /// Cancels todo item creation and navigates back.
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}