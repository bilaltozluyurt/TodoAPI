namespace TodoDDD.API.DTOs
{
    public class TodoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;        
        public bool IsCompleted { get; set; }
        
    }
}
