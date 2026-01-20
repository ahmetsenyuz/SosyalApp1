using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SosyalApp1.src.components
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileComponent : ControllerBase
    {
        // GET: api/profile
        [HttpGet]
        public IActionResult GetUserProfile()
        {
            // In a real implementation, this would return user profile data
            // based on the authenticated user
            return Ok(new {
                username = "testuser",
                totalPoints = 100,
                completedTasksCount = 5
            });
        }
    }
}