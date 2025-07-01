using Microsoft.EntityFrameworkCore;
using TodoDDD.Domain.Entities;

namespace TodoDDD.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasKey(t => t.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
