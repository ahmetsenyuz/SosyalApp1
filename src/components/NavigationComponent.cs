using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SosyalApp1.src.components
{
    [ApiController]
    [Route("api/[controller]")]
    public class NavigationComponent : ControllerBase
    {
        // GET: api/navigation
        [HttpGet]
        public IActionResult GetNavigation()
        {
            // In a real implementation, this would return navigation menu items
            var navigationItems = new List<object>
            {
                new { name = "Dashboard", url = "/dashboard", icon = "dashboard" },
                new { name = "Profile", url = "/profile", icon = "user" },
                new { name = "Tasks", url = "/tasks", icon = "list" },
                new { name = "Settings", url = "/settings", icon = "settings" }
            };
            
            return Ok(navigationItems);
        }
    }
}