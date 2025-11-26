using Microsoft.EntityFrameworkCore;
using TDM.Api.Enum;
using TDM.Domain.Entities;

namespace TDM.Server.Persistence.PostgreSQL;

/// <summary>
/// Класс для инициализации базы данных начальными данными
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Инициализирует базу данных seed данными, если она пуста (синхронная версия)
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public static void Initialize(AppDbContext context)
    {
        // Применяем миграции, если они есть
        context.Database.Migrate();
        
        // Проверяем, есть ли уже данные в базе
        if (context.Contacts.Any())
        {
            // База уже содержит данные, инициализация не требуется
            return;
        }
        
        // Заполняем контакты
        SeedContacts(context);
        
        // Сохраняем изменения
        context.SaveChanges();
    }
    
    /// <summary>
    /// Инициализирует базу данных seed данными, если она пуста (асинхронная версия)
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public static async Task InitializeAsync(AppDbContext context)
    {
        // Проверяем, есть ли уже данные в базе
        if (await context.Contacts.AnyAsync())
        {
            // База уже содержит данные, инициализация не требуется
            return;
        }
        
        // Заполняем контакты
        await SeedContactsAsync(context);
        
        // Сохраняем изменения
        await context.SaveChangesAsync();
    }
    
    private static void SeedContacts(AppDbContext context)
    {
        var baseTimestamp = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();
        
        // Мужские имена
        var maleFirstNames = new[] { "Александр", "Дмитрий", "Максим", "Сергей", "Андрей", "Алексей", "Артем", "Илья", "Кирилл", "Михаил",
                                     "Владимир", "Николай", "Евгений", "Виктор", "Павел", "Роман", "Денис", "Олег", "Игорь", "Егор",
                                     "Антон", "Станислав", "Константин", "Ярослав", "Тимур" };
        
        // Женские имена
        var femaleFirstNames = new[] { "Анна", "Мария", "Елена", "Ольга", "Татьяна", "Наталья", "Ирина", "Екатерина", "Светлана", "Юлия",
                                       "Дарья", "Полина", "Анастасия", "Виктория", "Вероника", "Алина", "Ксения", "Марина", "Валерия", "София",
                                       "Людмила", "Галина", "Нина", "Вера", "Любовь" };
        
        // Мужские фамилии
        var maleLastNames = new[] { "Иванов", "Смирнов", "Кузнецов", "Попов", "Васильев", "Петров", "Соколов", "Михайлов", "Новиков", "Федоров",
                                    "Морозов", "Волков", "Алексеев", "Лебедев", "Семенов", "Егоров", "Павлов", "Козлов", "Степанов", "Николаев",
                                    "Орлов", "Андреев", "Макаров", "Никитин", "Захаров" };
        
        // Женские фамилии
        var femaleLastNames = new[] { "Иванова", "Смирнова", "Кузнецова", "Попова", "Васильева", "Петрова", "Соколова", "Михайлова", "Новикова", "Федорова",
                                      "Морозова", "Волкова", "Алексеева", "Лебедева", "Семенова", "Егорова", "Павлова", "Козлова", "Степанова", "Николаева",
                                      "Орлова", "Андреева", "Макарова", "Никитина", "Захарова" };
        
        // Транслитерация для email
        string Transliterate(string text)
        {
            var translitMap = new Dictionary<string, string>
            {
                {"а", "a"}, {"б", "b"}, {"в", "v"}, {"г", "g"}, {"д", "d"}, {"е", "e"}, {"ё", "yo"}, {"ж", "zh"},
                {"з", "z"}, {"и", "i"}, {"й", "y"}, {"к", "k"}, {"л", "l"}, {"м", "m"}, {"н", "n"}, {"о", "o"},
                {"п", "p"}, {"р", "r"}, {"с", "s"}, {"т", "t"}, {"у", "u"}, {"ф", "f"}, {"х", "kh"}, {"ц", "ts"},
                {"ч", "ch"}, {"ш", "sh"}, {"щ", "sch"}, {"ъ", ""}, {"ы", "y"}, {"ь", ""}, {"э", "e"}, {"ю", "yu"}, {"я", "ya"}
            };
            
            var result = text.ToLower();
            foreach (var pair in translitMap)
            {
                result = result.Replace(pair.Key, pair.Value);
            }
            return result;
        }
        
        var cities = new[] { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань", "Нижний Новгород", "Челябинск", "Самара", "Омск", "Ростов-на-Дону" };
        var streets = new[] { "ул. Ленина", "пр. Победы", "ул. Гагарина", "ул. Пушкина", "пр. Мира", "ул. Советская", "ул. Кирова", "Невский проспект", "ул. Красная", "ул. Баумана" };
        var positions = new[] { "Руководитель отдела", "Менеджер по продажам", "Системный администратор", "HR специалист", "Бухгалтер", 
                                "Разработчик", "Дизайнер", "Аналитик", "Маркетолог", "Юрист", "Программист", "Тестировщик" };
        
        var contacts = new List<ContactEntity>();
        var random = new Random(42); // Фиксированный seed для воспроизводимости
        
        for (int i = 0; i < 50; i++)
        {
            // Определяем пол (50/50)
            bool isMale = i % 2 == 0;
            
            var firstName = isMale 
                ? maleFirstNames[i % maleFirstNames.Length] 
                : femaleFirstNames[i % femaleFirstNames.Length];
            
            var lastName = isMale 
                ? maleLastNames[i % maleLastNames.Length] 
                : femaleLastNames[i % femaleLastNames.Length];
            
            var city = cities[random.Next(cities.Length)];
            var street = streets[random.Next(streets.Length)];
            var position = positions[random.Next(positions.Length)];
            var houseNum = random.Next(1, 200);
            var aptNum = random.Next(1, 150);
            
            // Генерируем номер телефона: 50% российских, 50% белорусских
            string phone;
            if (i % 2 == 0)
            {
                phone = $"+7 ({random.Next(900, 1000)}) {random.Next(100, 1000)}-{random.Next(10, 100)}-{random.Next(10, 100)}";
            }
            else
            {
                var belarusOperators = new[] { "29", "33", "44", "25" };
                var operatorCode = belarusOperators[random.Next(belarusOperators.Length)];
                phone = $"+375 ({operatorCode}) {random.Next(100, 1000)}-{random.Next(10, 100)}-{random.Next(10, 100)}";
            }
            
            // Транслитерация имени и фамилии для email
            var emailFirstName = Transliterate(firstName);
            var emailLastName = Transliterate(lastName);
            var email = $"{emailFirstName}.{emailLastName}{i}@example.com";
            
            var contact = ContactEntity.Create(
                firstName: firstName,
                lastName: lastName,
                phone: phone,
                email: email,
                address: $"г. {city}, {street}, д. {houseNum}, кв. {aptNum}",
                description: position
            );
            
            contact.CreatedAt = baseTimestamp;
            contact.UpdatedAt = baseTimestamp;
            
            contacts.Add(contact);
        }
        
        context.Contacts.AddRange(contacts);
        context.SaveChanges();
        
        SeedTodoItems(context, contacts, baseTimestamp);
    }
    
    private static void SeedTodoItems(AppDbContext context, List<ContactEntity> contacts, long baseTimestamp)
    {
        var random = new Random(42);
        
        var taskTitles = new[] {
            "Подготовить отчет", "Провести встречу", "Отправить письмо", "Обновить документацию",
            "Проверить код", "Написать тесты", "Исправить баги", "Провести презентацию",
            "Согласовать бюджет", "Обучить новых сотрудников", "Закупить оборудование",
            "Организовать мероприятие", "Подготовить презентацию", "Провести анализ",
            "Оптимизировать процесс", "Разработать план", "Провести аудит", "Обновить систему",
            "Настроить интеграцию", "Подготовить договор", "Создать отчет по продажам",
            "Провести собеседование", "Запланировать отпуск", "Подписать контракт"
        };
        
        var taskDetails = new[] {
            "Необходимо подготовить подробный отчет с аналитикой",
            "Встреча запланирована на конференц-зале",
            "Важное письмо для клиента",
            "Обновить техническую документацию проекта",
            "Code review перед релизом",
            "Написать unit-тесты для новых функций",
            "Исправить критические баги из backlog",
            "Презентация для инвесторов",
            "Согласовать бюджет на следующий квартал",
            null, // Некоторые задачи без деталей
            "Закупить новое оборудование для офиса",
            "Организовать корпоративное мероприятие",
            null,
            "Провести детальный анализ рынка",
            "Оптимизировать бизнес-процессы компании"
        };
        
        var todoItems = new List<TodoItemEntity>();
        
        foreach (var contact in contacts)
        {
            // Создаем 10 задач для каждого контакта
            for (int i = 0; i < 10; i++)
            {
                var title = taskTitles[random.Next(taskTitles.Length)];
                var details = random.Next(100) < 60 // 60% задач имеют детали
                    ? taskDetails[random.Next(taskDetails.Length)]
                    : null;
                
                var priority = (Priority)random.Next(1, 5); // Low=1, Medium=2, High=3, Urgent=4
                
                // Распределение статусов: 20% NotStarted, 50% InProgress, 30% Completed
                TodoStatus status;
                var statusRoll = random.Next(100);
                if (statusRoll < 20)
                    status = TodoStatus.NotStarted;
                else if (statusRoll < 70)
                    status = TodoStatus.InProgress;
                else
                    status = TodoStatus.Completed;
                
                // Дата: от -30 дней (просроченные) до +60 дней (будущие)
                var dueDate = DateTimeOffset.UtcNow.AddDays(random.Next(-30, 60)).ToUnixTimeSeconds();
                
                var todo = TodoItemEntity.Create(
                    title: title,
                    details: details,
                    dueDate: dueDate,
                    status: status,
                    priority: priority,
                    contactId: contact.Id,
                    description: $"Задача связана с контактом: {contact.FirstName} {contact.LastName}"
                );
                
                todo.CreatedAt = baseTimestamp + (i * 3600); // Разные timestamp для каждой задачи
                todo.UpdatedAt = baseTimestamp + (i * 3600);
                
                // Для завершенных задач устанавливаем CompletedAt
                if (status == TodoStatus.Completed)
                {
                    todo.CompletedAt = baseTimestamp + (i * 3600) + random.Next(1000, 100000);
                }
                
                todoItems.Add(todo);
            }
        }
        
        context.TodoItems.AddRange(todoItems);
        context.SaveChanges();
    }
    
    private static async Task SeedContactsAsync(AppDbContext context)
    {
        var baseTimestamp = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();
        
        var firstNames = new[] { "Александр", "Дмитрий", "Максим", "Сергей", "Андрей", "Алексей", "Артем", "Илья", "Кирилл", "Михаил",
                                 "Анна", "Мария", "Елена", "Ольга", "Татьяна", "Наталья", "Ирина", "Екатерина", "Светлана", "Юлия",
                                 "Владимир", "Николай", "Евгений", "Виктор", "Павел", "Роман", "Денис", "Олег", "Игорь", "Егор",
                                 "Дарья", "Полина", "Анастасия", "Виктория", "Вероника", "Алина", "Ксения", "Марина", "Валерия", "София",
                                 "Антон", "Станислав", "Константин", "Ярослав", "Тимур", "Вячеслав", "Глеб", "Матвей", "Федор", "Леонид" };
        
        var lastNames = new[] { "Иванов", "Смирнов", "Кузнецов", "Попов", "Васильев", "Петров", "Соколов", "Михайлов", "Новиков", "Федоров",
                                "Морозов", "Волков", "Алексеев", "Лебедев", "Семенов", "Егоров", "Павлов", "Козлов", "Степанов", "Николаев",
                                "Орлов", "Андреев", "Макаров", "Никитин", "Захаров", "Зайцев", "Соловьев", "Борисов", "Яковлев", "Григорьев",
                                "Романов", "Воробьев", "Сергеев", "Терентьев", "Фролов", "Александров", "Дмитриев", "Королев", "Гусев", "Киселев",
                                "Ильин", "Максимов", "Поляков", "Сорокин", "Виноградов", "Ковалев", "Белов", "Медведев", "Антонов", "Тарасов" };
        
        var cities = new[] { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань", "Нижний Новгород", "Челябинск", "Самара", "Омск", "Ростов-на-Дону" };
        var streets = new[] { "ул. Ленина", "пр. Победы", "ул. Гагарина", "ул. Пушкина", "пр. Мира", "ул. Советская", "ул. Кирова", "Невский проспект", "ул. Красная", "ул. Баумана" };
        var positions = new[] { "Руководитель отдела", "Менеджер по продажам", "Системный администратор", "HR специалист", "Бухгалтер", 
                                "Разработчик", "Дизайнер", "Аналитик", "Маркетолог", "Юрист", "Программист", "Тестировщик" };
        
        var contacts = new List<ContactEntity>();
        var random = new Random(42); // Фиксированный seed для воспроизводимости
        
        for (int i = 0; i < 50; i++)
        {
            var firstName = firstNames[i % firstNames.Length];
            var lastName = lastNames[i % lastNames.Length];
            var city = cities[random.Next(cities.Length)];
            var street = streets[random.Next(streets.Length)];
            var position = positions[random.Next(positions.Length)];
            var houseNum = random.Next(1, 200);
            var aptNum = random.Next(1, 150);
            
            // Генерируем номер телефона: 50% российских, 50% белорусских
            string phone;
            if (i % 2 == 0)
            {
                // Российский номер: +7 (XXX) XXX-XX-XX
                phone = $"+7 ({random.Next(900, 1000)}) {random.Next(100, 1000)}-{random.Next(10, 100)}-{random.Next(10, 100)}";
            }
            else
            {
                // Белорусский номер: +375 (XX) XXX-XX-XX
                var belarusOperators = new[] { "29", "33", "44", "25" };
                var operatorCode = belarusOperators[random.Next(belarusOperators.Length)];
                phone = $"+375 ({operatorCode}) {random.Next(100, 1000)}-{random.Next(10, 100)}-{random.Next(10, 100)}";
            }
            
            var contact = ContactEntity.Create(
                firstName: firstName,
                lastName: lastName,
                phone: phone,
                email: $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com",
                address: $"г. {city}, {street}, д. {houseNum}, кв. {aptNum}",
                description: position
            );
            
            contact.CreatedAt = baseTimestamp;
            contact.UpdatedAt = baseTimestamp;
            
            contacts.Add(contact);
        }
        
        await context.Contacts.AddRangeAsync(contacts);
        await context.SaveChangesAsync(); // Сохраняем контакты, чтобы получить их ID
        
        // Создаем задачи для каждого контакта
        await SeedTodoItemsAsync(context, contacts, baseTimestamp);
    }
    
    private static async Task SeedTodoItemsAsync(AppDbContext context, List<ContactEntity> contacts, long baseTimestamp)
    {
        var random = new Random(42);
        
        var taskTitles = new[] {
            "Подготовить отчет", "Провести встречу", "Отправить письмо", "Обновить документацию",
            "Проверить код", "Написать тесты", "Исправить баги", "Провести презентацию",
            "Согласовать бюджет", "Обучить новых сотрудников", "Закупить оборудование",
            "Организовать мероприятие", "Подготовить презентацию", "Провести анализ",
            "Оптимизировать процесс", "Разработать план", "Провести аудит", "Обновить систему",
            "Настроить интеграцию", "Подготовить договор", "Создать отчет по продажам",
            "Провести собеседование", "Запланировать отпуск", "Подписать контракт"
        };
        
        var taskDetails = new[] {
            "Необходимо подготовить подробный отчет с аналитикой",
            "Встреча запланирована на конференц-зале",
            "Важное письмо для клиента",
            "Обновить техническую документацию проекта",
            "Code review перед релизом",
            "Написать unit-тесты для новых функций",
            "Исправить критические баги из backlog",
            "Презентация для инвесторов",
            "Согласовать бюджет на следующий квартал",
            null, // Некоторые задачи без деталей
            "Закупить новое оборудование для офиса",
            "Организовать корпоративное мероприятие",
            null,
            "Провести детальный анализ рынка",
            "Оптимизировать бизнес-процессы компании"
        };
        
        var todoItems = new List<TodoItemEntity>();
        
        foreach (var contact in contacts)
        {
            // Создаем 10 задач для каждого контакта
            for (int i = 0; i < 10; i++)
            {
                var title = taskTitles[random.Next(taskTitles.Length)];
                var details = random.Next(100) < 60 // 60% задач имеют детали
                    ? taskDetails[random.Next(taskDetails.Length)]
                    : null;
                
                var priority = (Priority)random.Next(1, 5); // Low=1, Medium=2, High=3, Urgent=4
                
                // Распределение статусов: 20% NotStarted, 50% InProgress, 30% Completed
                TodoStatus status;
                var statusRoll = random.Next(100);
                if (statusRoll < 20)
                    status = TodoStatus.NotStarted;
                else if (statusRoll < 70)
                    status = TodoStatus.InProgress;
                else
                    status = TodoStatus.Completed;
                
                // Дата: от -30 дней (просроченные) до +60 дней (будущие)
                var dueDate = DateTimeOffset.UtcNow.AddDays(random.Next(-30, 60)).ToUnixTimeSeconds();
                
                var todo = TodoItemEntity.Create(
                    title: title,
                    details: details,
                    dueDate: dueDate,
                    status: status,
                    priority: priority,
                    contactId: contact.Id,
                    description: $"Задача связана с контактом: {contact.FirstName} {contact.LastName}"
                );
                
                todo.CreatedAt = baseTimestamp + (i * 3600); // Разные timestamp для каждой задачи
                todo.UpdatedAt = baseTimestamp + (i * 3600);
                
                // Для завершенных задач устанавливаем CompletedAt
                if (status == TodoStatus.Completed)
                {
                    todo.CompletedAt = baseTimestamp + (i * 3600) + random.Next(1000, 100000);
                }
                
                todoItems.Add(todo);
            }
        }
        
        await context.TodoItems.AddRangeAsync(todoItems);
        await context.SaveChangesAsync();
    }
}
