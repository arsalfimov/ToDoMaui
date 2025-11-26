using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDM.Api.Client.Clients;
using TDM.Api.Contracts.TodoItems;
using TDM.Api.Enum;
using TDM.UI.Maui.Common;

namespace TDM.UI.Maui.ViewModels;

/// <summary>
/// ViewModel for managing the todo items list.
/// </summary>
public partial class TodoListViewModel : BaseViewModel
{
    private readonly ITodoItemsApiClient _todoItemsClient;

    [ObservableProperty]
    private ObservableCollection<TodoItemResponse> _todoItems = [];

    [ObservableProperty]
    private TodoItemResponse? _selectedTodoItem;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private TodoStatus? _filterStatus;

    [ObservableProperty]
    private Priority? _filterPriority;

    public TodoListViewModel(ITodoItemsApiClient todoItemsClient)
    {
        _todoItemsClient = todoItemsClient ?? throw new ArgumentNullException(nameof(todoItemsClient));
    }

    /// <summary>
    /// Called when SearchText changes - automatically applies filters.
    /// </summary>
    partial void OnSearchTextChanged(string value)
    {
        // Применяем фильтры автоматически при изменении текста поиска
        _ = ApplyFiltersAsync();
    }

    // Called when FilterStatus changes (Picker selection)
    partial void OnFilterStatusChanged(TodoStatus? value)
    {
        _ = ApplyFiltersAsync();
    }

    // Called when FilterPriority changes (Picker selection)
    partial void OnFilterPriorityChanged(Priority? value)
    {
        _ = ApplyFiltersAsync();
    }

    /// <summary>
    /// Initializes the ViewModel and loads data.
    /// </summary>
    public async Task InitializeAsync()
    {
        await LoadTodoItemsAsync();
    }

    /// <summary>
    /// Loads all todo items from the API.
    /// </summary>
    [RelayCommand]
    private async Task LoadTodoItemsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Загрузка задач...");

            // Если есть активные фильтры, применяем их
            if (!string.IsNullOrWhiteSpace(SearchText) || FilterStatus.HasValue || FilterPriority.HasValue)
            {
                await ApplyFiltersAsync();
                return;
            }

            var items = await _todoItemsClient.GetAllAsync();
            TodoItems = new ObservableCollection<TodoItemResponse>(items.OrderByDescending(x => x.CreatedAt));
        }
        catch (Exception ex)
        {
            SetError($"Ошибка загрузки задач: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
            IsRefreshing = false;
        }
    }

    /// <summary>
    /// Searches todo items by title.
    /// </summary>
    [RelayCommand]
    private async Task SearchTodoItemsAsync()
    {
        await ApplyFiltersAsync();
    }

    /// <summary>
    /// Filters todo items by status.
    /// </summary>
    [RelayCommand]
    private async Task FilterByStatusAsync(TodoStatus? status)
    {
        FilterStatus = status;
        await ApplyFiltersAsync();
    }

    /// <summary>
    /// Filters todo items by priority.
    /// </summary>
    [RelayCommand]
    private async Task FilterByPriorityAsync(Priority? priority)
    {
        FilterPriority = priority;
        await ApplyFiltersAsync();
    }

    /// <summary>
    /// Clears all filters and reloads all items.
    /// </summary>
    [RelayCommand]
    private async Task ClearFiltersAsync()
    {
        SearchText = string.Empty;
        FilterStatus = null;
        FilterPriority = null;
        await ApplyFiltersAsync();
    }

    /// <summary>
    /// Applies all active filters (search, status, priority) to the todo items list.
    /// </summary>
    private async Task ApplyFiltersAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            SetLoading(true, "Применение фильтров...");

            // Загружаем все задачи
            var items = await _todoItemsClient.GetAllAsync();
            IEnumerable<TodoItemResponse> filteredItems = items;

            // Применяем фильтр по поиску
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredItems = filteredItems.Where(x => 
                    x.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            // Применяем фильтр по статусу
            if (FilterStatus.HasValue)
            {
                filteredItems = filteredItems.Where(x => x.Status == (int)FilterStatus.Value);
            }

            // Применяем фильтр по приоритету
            if (FilterPriority.HasValue)
            {
                filteredItems = filteredItems.Where(x => x.Priority == (int)FilterPriority.Value);
            }

            TodoItems = new ObservableCollection<TodoItemResponse>(
                filteredItems.OrderByDescending(x => x.CreatedAt));
        }
        catch (Exception ex)
        {
            SetError($"Ошибка применения фильтров: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
            SetLoading(false);
        }
    }

    /// <summary>
    /// Deletes a todo item.
    /// </summary>
    [RelayCommand]
    private async Task DeleteTodoItemAsync(long todoItemId)
    {
        if (IsBusy) return;

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

            await _todoItemsClient.DeleteAsync(todoItemId);
            await LoadTodoItemsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Ошибка удаления задачи: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Navigates to create todo item page.
    /// </summary>
    [RelayCommand]
    private async Task NavigateToCreateTodoItemAsync()
    {
        await Shell.Current.GoToAsync("create-todo-item");
    }

    /// <summary>
    /// Navigates to todo item details page.
    /// </summary>
    [RelayCommand]
    private async Task NavigateToTodoItemDetailsAsync(TodoItemResponse? todoItem)
    {
        if (todoItem == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "TodoItemId", todoItem.Id }
        };

        await Shell.Current.GoToAsync("todo-item-details", parameters);
    }

    /// <summary>
    /// Refreshes the todo items list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadTodoItemsAsync();
    }
}
