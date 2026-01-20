using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SosyalApp1.src.components
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardComponent : ControllerBase
    {
        // GET: api/dashboard
        [HttpGet]
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
    }
}