using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Data;
using To_Do_List.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Mime;

namespace To_Do_List.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private TodoContext context;

        public TodoController(TodoContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetToDoTasks()
        {
            return Ok(await context.ToDoTasks.ToListAsync());
        }

        [HttpPost("{text}")]
        public async Task<ActionResult> PostToDoTask(string text)
        {
            ToDoTask newTask = new ToDoTask { Description = text };

            context.ToDoTasks.Add(newTask);

            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoTask), new { id = newTask.Id }, newTask);
        }

        [HttpPost]
        public async Task<ActionResult> PostToDoTasks(List<ToDoTask> toDoTasks)
        {
            context.ToDoTasks.RemoveRange(context.ToDoTasks);

            context.ToDoTasks.AddRange(toDoTasks);

            await context.SaveChangesAsync();

            return Ok(toDoTasks);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(int id)
        {
            var task = await context.ToDoTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            context.ToDoTasks.Remove(task);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetToDoTask(int id)
        {
            var task = await context.ToDoTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}/{description}")]
        public async Task<IActionResult> UpdateToDoTask(int id, string description)
        {
            var task = await context.ToDoTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Description = description;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/{status}")]
        public async Task<IActionResult> SetStatus(int id, bool status)
        {
            var ToDoTask = await context.ToDoTasks.FindAsync(id);
            if (ToDoTask == null)
            {
                return NotFound();
            }

            ToDoTask.IsCompleted = status;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}