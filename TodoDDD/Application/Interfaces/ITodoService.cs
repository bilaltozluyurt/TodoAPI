using TodoDDD.Domain.Entities;

namespace TodoDDD.Application.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(Guid id);
        Task<TodoItem> CreateAsync(string title);
        Task<bool> UpdateAsync(Guid id, string title, bool isCompleted);
        Task<bool> DeleteAsync(Guid id);
    }
}
