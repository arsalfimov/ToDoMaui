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
/// ViewModel for displaying and editing todo item details.
/// </summary>
[QueryProperty(nameof(TodoItemId), nameof(TodoItemId))]
public partial class TodoItemDetailsViewModel : BaseViewModel
{
    private readonly ITodoItemsApiClient _todoItemsClient;
    private readonly IContactsApiClient _contactsClient;

    [ObservableProperty]
    private long _todoItemId;

    [ObservableProperty]
    private TodoItemResponse? _todoItem;

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string? _details;

    [ObservableProperty]
    private DateTime _dueDate = DateTime.Today.AddDays(7);

    [ObservableProperty]
    private bool _hasDueDate;

    [ObservableProperty]
    private TodoStatus _status;

    [ObservableProperty]
    private Priority _priority;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private string? _titleError;

    [ObservableProperty]
    private ObservableCollection<ContactResponse> _contacts = [];

    [ObservableProperty]
    private ContactResponse? _selectedContact;

    [ObservableProperty]
    private long? _contactId;

    public TodoItemDetailsViewModel(ITodoItemsApiClient todoItemsClient, IContactsApiClient contactsClient)
    {
        _todoItemsClient = todoItemsClient ?? throw new ArgumentNullException(nameof(todoItemsClient));
        _contactsClient = contactsClient ?? throw new ArgumentNullException(nameof(contactsClient));
    }

    partial void OnTodoItemIdChanged(long value)
    {
        if (value > 0)
        {
            _ = LoadTodoItemDetailsAsync();
            _ = LoadContactsAsync();
        }
    }

    partial void OnSelectedContactChanged(ContactResponse? value)
    {
        ContactId = value?.Id;
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
            
            // Установить текущий выбранный контакт если он есть
            if (TodoItem?.ContactId != null)
            {
                SelectedContact = Contacts.FirstOrDefault(c => c.Id == TodoItem.ContactId);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка загрузки контактов: {ex.Message}");
        }
    }

    /// <summary>
    /// Loads todo item details.
    /// </summary>
    [RelayCommand]
    private async Task LoadTodoItemDetailsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Загрузка данных...");

            TodoItem = await _todoItemsClient.GetByIdAsync(TodoItemId);

            if (TodoItem != null)
            {
                Title = TodoItem.Title;
                Details = TodoItem.Details;
                
                if (TodoItem.DueDate.HasValue)
                {
                    DueDate = DateTimeOffset.FromUnixTimeSeconds(TodoItem.DueDate.Value).LocalDateTime;
                    HasDueDate = true;
                }
                else
                {
                    DueDate = DateTime.Today.AddDays(7);
                    HasDueDate = false;
                }
                
                Status = (TodoStatus)TodoItem.Status;
                Priority = (Priority)TodoItem.Priority;
                Description = TodoItem.Description;
                ContactId = TodoItem.ContactId;
                
                // Установить выбранный контакт если он есть
                if (TodoItem.ContactId != null && Contacts.Any())
                {
                    SelectedContact = Contacts.FirstOrDefault(c => c.Id == TodoItem.ContactId);
                }
            }
            
            // Принудительно уведомляем UI об изменении ВСЕХ свойств
            OnPropertyChanged(nameof(TodoItem));
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Details));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(Priority));
            OnPropertyChanged(nameof(DueDate));
            OnPropertyChanged(nameof(HasDueDate));
            OnPropertyChanged(nameof(ContactId));
            OnPropertyChanged(nameof(SelectedContact));
        }
        catch (Exception ex)
        {
            SetError($"Ошибка загрузки задачи: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
        }
    }

    /// <summary>
    /// Enables editing mode.
    /// </summary>
    [RelayCommand]
    private void StartEditing()
    {
        IsEditing = true;
    }

    /// <summary>
    /// Saves changes to the todo item.
    /// </summary>
    [RelayCommand]
    private async Task SaveChangesAsync()
    {
        if (IsBusy || TodoItem == null) return;

        // Валидация
        TitleError = null;
        ClearError();

        // Убираем пробелы по краям
        Title = Title?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(Title))
        {
            TitleError = "Заголовок задачи обязателен.";
            SetError("Пожалуйста, заполните заголовок задачи.");
            return;
        }

        if (Title.Length > 200)
        {
            TitleError = "Заголовок не должен превышать 200 символов.";
            SetError("Пожалуйста, исправьте ошибки в форме.");
            return;
        }

        // Убираем пробелы из Details
        if (!string.IsNullOrEmpty(Details))
        {
            Details = Details.Trim();
            if (string.IsNullOrWhiteSpace(Details))
            {
                Details = null;
            }
        }

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Сохранение изменений...");

            long? dueDateUnix = HasDueDate 
                ? new DateTimeOffset(DueDate).ToUnixTimeSeconds() 
                : null;

            var request = new UpdateTodoItemRequest(
                Title: Title,
                Details: string.IsNullOrWhiteSpace(Details) ? null : Details,
                DueDate: dueDateUnix,
                Status: Status,
                Priority: Priority,
                ContactId: ContactId,
                Description: null // Description не редактируется пользователем
            );

            var updatedTodoItem = await _todoItemsClient.UpdateAsync(TodoItemId, request);
            TodoItem = updatedTodoItem; // Обновляем TodoItem напрямую - UI обновится автоматически!
            IsEditing = false;
        }
        catch (Refit.ApiException apiEx)
        {
            var errorMessage = $"Ошибка сохранения ({apiEx.StatusCode})";
            SetError(errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Ошибка: {ex.Message}";
            SetError(errorMessage);
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
        }
    }

    /// <summary>
    /// Cancels editing.
    /// </summary>
    [RelayCommand]
    private void CancelEditing()
    {
        if (TodoItem == null) return;
        
        // Восстанавливаем оригинальные значения из TodoItem
        Title = TodoItem.Title;
        Details = TodoItem.Details;
        
        if (TodoItem.DueDate.HasValue)
        {
            DueDate = DateTimeOffset.FromUnixTimeSeconds(TodoItem.DueDate.Value).LocalDateTime;
            HasDueDate = true;
        }
        else
        {
            DueDate = DateTime.Today.AddDays(7);
            HasDueDate = false;
        }
        
        Status = (TodoStatus)TodoItem.Status;
        Priority = (Priority)TodoItem.Priority;
        ContactId = TodoItem.ContactId;
        
        if (TodoItem.ContactId != null && Contacts.Any())
        {
            SelectedContact = Contacts.FirstOrDefault(c => c.Id == TodoItem.ContactId);
        }
        else
        {
            SelectedContact = null;
        }
        
        TitleError = null;
        ClearError();
        IsEditing = false;
    }

    /// <summary>
    /// Marks todo item as completed.
    /// </summary>
    [RelayCommand]
    private async Task CompleteAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Завершение задачи...");

            var completedTodoItem = await _todoItemsClient.CompleteAsync(TodoItemId);
            TodoItem = completedTodoItem; // Обновляем TodoItem - UI обновится автоматически!
        }
        catch (Exception ex)
        {
            SetError($"Ошибка завершения задачи: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false); // ← ИСПРАВЛЕНО: добавлен вызов SetLoading(false)
        }
    }

    /// <summary>
    /// Cancels the todo item.
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Отмена задачи...");

            var cancelledTodoItem = await _todoItemsClient.CancelAsync(TodoItemId);
            TodoItem = cancelledTodoItem; // Обновляем TodoItem - UI обновится автоматически!
        }
        catch (Exception ex)
        {
            SetError($"Ошибка отмены задачи: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
        }
    }

    /// <summary>
    /// Deletes the todo item.
    /// </summary>
    [RelayCommand]
    private async Task DeleteAsync()
    {
        bool confirmed = await Application.Current!.MainPage!.DisplayAlert(
            "Подтверждение",
            "Вы уверены, что хотите удалить эту задачу?",
            "Да",
            "Нет");

        if (!confirmed) return;

        try
        {
            IsBusy = true;
            ClearError();

            await _todoItemsClient.DeleteAsync(TodoItemId);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            SetError($"Ошибка удаления: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Navigates back.
    /// </summary>
    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
