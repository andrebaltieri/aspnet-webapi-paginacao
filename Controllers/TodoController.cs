using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pagenator.Data;

namespace Pagenator.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet("/load")]
        public async Task<IActionResult> Load(
            [FromServices] AppDbContext context)
        {
            for (var i = 0; i < 1348; i++)
            {
                var todo = new Todo()
                {
                    Id = i + 1,
                    Done = false,
                    CreatedAt = DateTime.Now,
                    Title = $"Tarefa {i}"
                };
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet("/{skip:int}/{take:int}")]
        public async Task<IActionResult> Get(
            [FromServices] AppDbContext context,
            int skip = 0,
            int take = 25)
        {
            var count = await context.Todos.CountAsync();
            var todos = await context
                .Todos
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(new
            {
                total = count,
                skip,
                take,
                data = todos
            });
        }
    }
}
