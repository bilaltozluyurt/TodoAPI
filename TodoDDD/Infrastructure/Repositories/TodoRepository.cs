using Microsoft.EntityFrameworkCore;
using TodoDDD.Application.Interfaces;
using TodoDDD.Domain.Entities;
using TodoDDD.Infrastructure.Data;

namespace TodoDDD.Infrastructure.Repositories
{
    public class TodoRepository(AppDbContext context) : ITodoRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task AddAsync(TodoItem item)
        {
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            _context.TodoItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item is not null)
            {
                _context.TodoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
