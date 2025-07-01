using Microsoft.AspNetCore.Mvc;
using TodoDDD.API.DTOs;
using TodoDDD.Application.Interfaces;
using TodoDDD.Domain.Entities;

namespace TodoDDD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoResponse>>> GetAll()
        {
            var items = await _todoService.GetAllAsync();
            var response = items.Select(x => new TodoResponse
            {
                Id = x.Id,
                Title = x.Title,                
                IsCompleted = x.IsCompleted,
                
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoResponse>> GetById(Guid id)
        {
            var item = await _todoService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var response = new TodoResponse
            {
                Id = item.Id,
                Title = item.Title,               
                IsCompleted = item.IsCompleted,
                
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<TodoResponse>> Create(CreateTodoRequest request)
        {
            var item = await _todoService.CreateAsync(request.Title);

            var response = new TodoResponse
            {
                Id = item.Id,
                Title = item.Title,
                IsCompleted = item.IsCompleted,
                
            };

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTodoRequest request)
        {
            var result = await _todoService.UpdateAsync(id, request.Title, request.IsCompleted);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _todoService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
