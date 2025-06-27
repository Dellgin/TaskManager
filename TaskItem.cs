namespace TaskManager
{
    public class TaskItem(int id, string desc)
    {
        public int Id { get; set; } = id;
        public string Description { get; set; } = desc;
        public bool IsCompleted { get; set; } = false;
    }
}