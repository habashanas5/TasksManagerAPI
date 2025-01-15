using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTask>>> GetTasks(
            string status = null,
            string priority = null,
            string category = null,
            string search = null,
            int page = 1,
            int pageSize = 10)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var tasks = _context.userTasks.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                tasks = tasks.Where(t => t.Status == status);

            if (!string.IsNullOrEmpty(priority))
                tasks = tasks.Where(t => t.Priority == priority);

            if (!string.IsNullOrEmpty(category))
                tasks = tasks.Where(t => t.Category == category);

            if (!string.IsNullOrEmpty(search))
                tasks = tasks.Where(t => t.Title.Contains(search) || t.Description.Contains(search));

            var totalCount = await tasks.CountAsync();

            var paginatedTasks = await tasks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Tasks = paginatedTasks
            });
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTask>> GetTask(int id)
        {
            var task = await _context.userTasks.FindAsync(id);

            if (task == null)
                return NotFound();

            return task;
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] UserTask task)
        {
            if (task == null)
                return BadRequest("Invalid data.");

            try
            {
                _context.userTasks.Add(task);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Task added successfully!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UserTask task)
        {
            if (id != task.Id)
                return BadRequest("Task ID mismatch.");

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Task updated successfully!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.userTasks.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.userTasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task deleted successfully!" });
        }

        private bool TasksExists(int id)
        {
            return _context.userTasks.Any(m => m.Id == id);
        }

        // GET: api/tasks/progress
        [HttpGet("progress")]
        public async Task<IActionResult> ViewProgress()
        {
            var totalTasks = await _context.userTasks.CountAsync();
            if (totalTasks == 0)
            {
                return Ok(new
                {
                    Message = "No tasks available.",
                    TotalTasks = 0,
                    CompletedTasks = 0,
                    CompletionRate = "0%"
                });
            }

            var completedTasks = await _context.userTasks
                .Where(t => t.Status.ToLower() == "completed")
                .CountAsync();

            double completionRate = (double)completedTasks / totalTasks * 100;

            return Ok(new
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                CompletionRate = $"{completionRate:F2}%"
            });
        }

        // GET: api/tasks/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchTasks([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { Message = "Search query cannot be empty." });
            }

            var tasks = await _context.userTasks
                .Where(t => t.Title.Contains(query) || t.Description.Contains(query))
                .ToListAsync();

            if (tasks.Count == 0)
            {
                return NotFound(new { Message = "No tasks found matching your search query." });
            }

            return Ok(tasks);
        }
    }
}
