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
        // GET: api/tasks/user/{userId}
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

        // POST: api/tasks/generate/{userId}
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

        // PUT: api/tasks/status/{taskId}
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

        // GET: api/tasks/details/{taskId}
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

        // POST: api/tasks/upload-evidence/{taskId}
        [HttpPost("upload-evidence/{taskId}")]
        public IActionResult UploadEvidence(int taskId, [FromForm] UploadEvidenceRequest request)
        {
            try
            {
                if (request.EvidenceFile == null || request.EvidenceFile.Length == 0)
                {
                    return BadRequest(new { error = "Evidence file is required" });
                }

                // Validate file type and size (example validation)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                var maxFileSize = 5 * 1024 * 1024; // 5MB limit

                var fileExtension = Path.GetExtension(request.EvidenceFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { error = "Invalid file type. Only JPG, PNG, and GIF files are allowed." });
                }

                if (request.EvidenceFile.Length > maxFileSize)
                {
                    return BadRequest(new { error = "File size exceeds the 5MB limit." });
                }

                // In a real implementation, you would save the file to storage
                // For now, we'll just store the file info
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = $"uploads/{fileName}"; // This would be actual file path in production

                var success = TaskService.UploadEvidence(
                    taskId, 
                    request.EvidenceFile.FileName, 
                    filePath, 
                    request.EvidenceFile.ContentType, 
                    request.EvidenceFile.Length);

                if (!success)
                {
                    return BadRequest(new { error = "Failed to upload evidence" });
                }

                return Ok(new { message = "Evidence uploaded successfully", taskId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to upload evidence", message = ex.Message });
            }
        }

        public class UpdateTaskStatusRequest
        {
            public string NewStatus { get; set; }
        }

        public class UploadEvidenceRequest
        {
            public IFormFile EvidenceFile { get; set; }
        }
    }
}