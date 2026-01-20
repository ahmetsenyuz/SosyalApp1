using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SosyalApp1.src.integration
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserJourneyIntegration : ControllerBase
    {
        // Complete user registration and login flow
        [HttpPost("register-login")]
        public IActionResult RegisterAndLogin([FromBody] RegistrationLoginModel model)
        {
            // In a real implementation, this would involve:
            // 1. Validating the input data
            // 2. Hashing the password
            // 3. Saving the user to database
            // 4. Generating a JWT token
            
            // For now, we'll simulate a successful registration
            return Ok(new { message = "User registered successfully" });
        }

        // Dashboard with daily tasks and profile information
        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            // In a real implementation, this would return dashboard data
            // including 3 daily tasks with status
            var tasks = new List<object>
            {
                new { id = 1, title = "Complete project proposal", status = "completed", priority = "high" },
                new { id = 2, title = "Review team feedback", status = "pending", priority = "medium" },
                new { id = 3, title = "Update documentation", status = "assigned", priority = "low" }
            };
            
            return Ok(new {
                tasks = tasks,
                totalPoints = 100,
                completedTasks = 5,
                recentActivity = new List<object>
                {
                    new { action = "Completed task", description = "Project proposal review", time = "2 hours ago" },
                    new { action = "Started task", description = "Team feedback review", time = "5 hours ago" },
                    new { action = "Updated", description = "Documentation", time = "1 day ago" }
                }
            });
        }

        // Task completion workflow with evidence upload
        [HttpPost("complete-task/{taskId}")]
        public IActionResult CompleteTask(int taskId, [FromBody] CompleteTaskRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.EvidenceFile?.FileName))
                {
                    return BadRequest(new { error = "Evidence file is required" });
                }

                // Validate file type and size (example validation)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                var maxSize = 5 * 1024 * 1024; // 5MB limit

                var fileExtension = System.IO.Path.GetExtension(request.EvidenceFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { error = "Invalid file type. Only JPG, PNG, and GIF files are allowed." });
                }

                if (request.EvidenceFile.Length > maxSize)
                {
                    return BadRequest(new { error = "File size exceeds the 5MB limit." });
                }

                // In a real implementation, you would save the file to storage
                // For now, we'll just store the file info
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = $"uploads/{fileName}"; // This would be actual file path in production

                var success = TaskService.CompleteTask(
                    taskId,
                    request.EvidenceFile.FileName,
                    filePath,
                    request.EvidenceFile.ContentType,
                    request.EvidenceFile.Length);

                if (!success)
                {
                    return BadRequest(new { error = "Failed to complete task" });
                }

                return Ok(new { message = "Task completed successfully", taskId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Failed to complete task", message = ex.Message });
            }
        }

        // Friend management and task recommendation flow
        [HttpGet("friends")]
        public IActionResult GetFriends()
        {
            // In a real implementation, this would return friend list
            // For now, we'll return mock data
            return Ok(new {
                friends = new List<object>
                {
                    new { id = 1, username = "friend1", points = 150 },
                    new { id = 2, username = "friend2", points = 200 },
                    new { id = 3, username = "friend3", points = 180 }
                },
                recommendations = new List<object>
                {
                    new { id = 4, title = "Quick design review", difficulty = "easy", points = 10 },
                    new { id = 5, title = "Code refactoring", difficulty = "hard", points = 50 }
                }
            });
        }

        // Leaderboard access and viewing capabilities
        [HttpGet("leaderboard")]
        public IActionResult GetLeaderboard(int limit = 10)
        {
            var leaderboard = LeaderboardService.GetGlobalLeaderboard(limit);
            return Ok(leaderboard);
        }

        // Friend leaderboard
        [HttpGet("leaderboard/friends")]
        public IActionResult GetFriendLeaderboard(int userId, int limit = 10)
        {
            var leaderboard = LeaderboardService.GetFriendLeaderboard(userId, limit);
            return Ok(leaderboard);
        }

        public class RegistrationLoginModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CompleteTaskRequest
        {
            public IFormFile EvidenceFile { get; set; }
        }
    }
}