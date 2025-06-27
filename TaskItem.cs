namespace TaskManager
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public TaskItem() { }

        public TaskItem(int id, string desc)
        {
            Id = id;
            Description = desc;
            IsCompleted = false;
            CreatedAt = DateTime.Now;
        }
    }
}