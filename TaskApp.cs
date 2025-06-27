using System.Text.Json;

namespace TaskManager
{
    public static class TaskApp
    {
        private static List<TaskItem> tasks = [];
        private static int nextId = 1;
        private const int headerLines = 2;
        private const string DataFile = "tasks.json";

        public static void Run()
        {
            while (true)
            {
                Render();
                Console.SetCursorPosition(0, headerLines);
                ShowMenu();
                var key = Console.ReadKey(true);
                HandleInput(key);
            }
        }

        private static void Render()
        {
            Console.Clear();
            int total = tasks.Count;
            int done = tasks.FindAll(t => t.IsCompleted).Count;
            Console.WriteLine($"Всего задач: {total} | Выполнено: {done}");
            Console.WriteLine(new string('-', 30));
        }

        private static void ShowMenu()
        {
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Отметить выполненной");
            Console.WriteLine("3. Удалить задачу");
            Console.WriteLine("4. Показать все задачи");
            Console.WriteLine("5. Сохранить задачи в файл");
            Console.WriteLine("6. Загрузить задачи из файла");
            Console.WriteLine("Q. Выход");
            Console.Write("Выбор: ");
        }

        private static void HandleInput(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    AddTask();
                    break;
                case ConsoleKey.D2:
                    MarkCompleted();
                    break;
                case ConsoleKey.D3:
                    DeleteTask();
                    break;
                case ConsoleKey.D4:
                    ListTasks();
                    break;
                case ConsoleKey.D5:
                    SaveTasks();
                break;
                case ConsoleKey.D6:
                    LoadTasks();
                break;
                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }
        }

        private static void AddTask()
        {
            Console.Write("Описание задачи: ");
            string? desc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(desc))
            {
                tasks.Add(new TaskItem(nextId++, desc));
            }
            else
            {
                Console.WriteLine("Описание не может быть пустым. Нажмите любую клавишу для возврата...");
                Console.ReadKey(true);
            }
        }

        private static void MarkCompleted()
        {
            Console.Write("ID задачи для отметки: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var task = tasks.Find(t => t.Id == id);
                if (task != null)
                    task.IsCompleted = true;
            }
        }

        private static void DeleteTask()
        {
            Console.Write("ID задачи для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                tasks.RemoveAll(t => t.Id == id);
            }
        }

        private static void ListTasks()
        {
            Console.Clear();
            Console.WriteLine("Список задач:");
            foreach (var t in tasks)
                Console.WriteLine($"[{(t.IsCompleted ? 'x' : ' ')}] {t.Id}: {t.Description}");
            Console.WriteLine("\nНажмите любую клавишу для возврата...");
            Console.ReadKey(true);
        }

        private static void SaveTasks()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(DataFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        private static void LoadTasks()
        {
            if (!File.Exists(DataFile)) return;
            try
            {
                string json = File.ReadAllText(DataFile);
                var loaded = JsonSerializer.Deserialize<List<TaskItem>>(json);
                if (loaded != null)
                {
                    tasks = loaded;
                    nextId = tasks.Count > 0 ? tasks[^1].Id + 1 : 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
                tasks = [];
            }
        }
    }
}