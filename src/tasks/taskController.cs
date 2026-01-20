using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SosyalApp1.src.tasks
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        // GET: api/task/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetTasksForUser(int userId)
        {
            try
            {
                var tasks = TaskService.GetTasksForUser(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to retrieve tasks", message = ex.Message });
            }
        }

        // POST: api/task/generate/{userId}
        [HttpPost("generate/{userId}")]
        public IActionResult GenerateTasksForUser(int userId)
        {
            try
            {
                var tasks = TaskService.GenerateTasksForUser(userId);
                return Ok(new { message = "Tasks generated successfully", tasks });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to generate tasks", message = ex.Message });
            }
        }

        // PUT: api/task/status/{taskId}
        [HttpPut("status/{taskId}")]
        public IActionResult UpdateTaskStatus(int taskId, [FromBody] UpdateTaskStatusRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NewStatus))
                {
                    return BadRequest(new { error = "New status is required" });
                }

                var success = TaskService.UpdateTaskStatus(taskId, request.NewStatus);
                if (!success)
                {
                    return BadRequest(new { error = "Failed to update task status" });
                }

                return Ok(new { message = "Task status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to update task status", message = ex.Message });
            }
        }

        // GET: api/task/details/{taskId}
        [HttpGet("details/{taskId}")]
        public IActionResult GetTaskDetails(int taskId)
        {
            try
            {
                var task = TaskService.GetTaskDetails(taskId);
                if (task == null)
                {
                    return NotFound(new { error = "Task not found" });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to retrieve task details", message = ex.Message });
            }
        }
    }

    public class UpdateTaskStatusRequest
    {
        public string NewStatus { get; set; }
    }
}