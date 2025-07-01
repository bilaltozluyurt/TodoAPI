using TodoDDD.Application.Interfaces;
using TodoDDD.Domain.Entities;

namespace TodoDDD.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TodoItem> CreateAsync(string title)
        {
            var item = new TodoItem
            {
                Title = title,
                
            };

            await _repository.AddAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(Guid id, string title, bool isCompleted)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return false;

            item.Title = title;            
            item.IsCompleted = isCompleted;

            await _repository.UpdateAsync(item);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
