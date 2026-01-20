using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SosyalApp1.src.components
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardComponent : ControllerBase
    {
        // GET: api/leaderboard/global
        [HttpGet("global")]
        public IActionResult GetGlobalLeaderboard()
        {
            // In a real implementation, this would return global leaderboard data
            // including top users based on scores
            var leaderboard = new List<object>
            {
                new { rank = 1, username = "user1", score = 1500 },
                new { rank = 2, username = "user2", score = 1200 },
                new { rank = 3, username = "user3", score = 1000 },
                new { rank = 4, username = "user4", score = 900 },
                new { rank = 5, username = "user5", score = 800 }
            };

            return Ok(new
            {
                leaderboard = leaderboard,
                totalUsers = 100,
                timePeriod = "all-time"
            });
        }

        // GET: api/leaderboard/friends
        [HttpGet("friends")]
        public IActionResult GetFriendLeaderboard([FromQuery] int userId)
        {
            // In a real implementation, this would return friend leaderboard data
            // including only connected users
            var leaderboard = new List<object>
            {
                new { rank = 1, username = "friend1", score = 1400 },
                new { rank = 2, username = "friend2", score = 1300 },
                new { rank = 3, username = "friend3", score = 1100 }
            };

            return Ok(new
            {
                leaderboard = leaderboard,
                totalFriends = 3,
                timePeriod = "all-time"
            });
        }

        // GET: api/leaderboard/global/daily
        [HttpGet("global/daily")]
        public IActionResult GetDailyGlobalLeaderboard()
        {
            // In a real implementation, this would return daily global leaderboard data
            var leaderboard = new List<object>
            {
                new { rank = 1, username = "user1", score = 150 },
                new { rank = 2, username = "user2", score = 120 },
                new { rank = 3, username = "user3", score = 100 }
            };

            return Ok(new
            {
                leaderboard = leaderboard,
                totalUsers = 100,
                timePeriod = "daily"
            });
        }

        // GET: api/leaderboard/global/weekly
        [HttpGet("global/weekly")]
        public IActionResult GetWeeklyGlobalLeaderboard()
        {
            // In a real implementation, this would return weekly global leaderboard data
            var leaderboard = new List<object>
            {
                new { rank = 1, username = "user1", score = 850 },
                new { rank = 2, username = "user2", score = 750 },
                new { rank = 3, username = "user3", score = 650 }
            };

            return Ok(new
            {
                leaderboard = leaderboard,
                totalUsers = 100,
                timePeriod = "weekly"
            });
        }

        // GET: api/leaderboard/global/monthly
        [HttpGet("global/monthly")]
        public IActionResult GetMonthlyGlobalLeaderboard()
        {
            // In a real implementation, this would return monthly global leaderboard data
            var leaderboard = new List<object>
            {
                new { rank = 1, username = "user1", score = 2500 },
                new { rank = 2, username = "user2", score = 2200 },
                new { rank = 3, username = "user3", score = 2000 }
            };

            return Ok(new
            {
                leaderboard = leaderboard,
                totalUsers = 100,
                timePeriod = "monthly"
            });
        }

        // GET: api/leaderboard/friends/daily
        [HttpGet("friends/daily")]
        public IActionResult GetDailyFriendLeaderboard([FromQuery] int userId)
        {
            // In a real implementation, this would return daily friend leaderboard data
            var leaderboard = new List<object>
            {
                new { rank = 1, username = "friend1", score = 140 },
                new { rank = 2, username = "friend2", score = 120 },
                new { rank = 3, username = "friend3", score = 100 }
            };

            return Ok(new
            {
                leaderboard = leaderboard,
                totalFriends = 3,
                timePeriod = "daily"
            });
        }

        // GET: api/leaderboard/friends/weekly
        [HttpGet("friends/weekly")]
        public IActionResult GetWeeklyFriendLeaderboard([FromQuery] int userId)
        {
            // In a real implementation, this would return weekly friend leaderboard data
            var leaderboard = new List<object>
            {
                new { rank = 1, username = "friend1", score = 750 },
                new { rank = 2, username = "friend2", score = 650 },
                new { rank = 3, username = "friend3", score = 550 }
            };

            return Ok(new
            {
                leaderboard = leaderboard,
                totalFriends = 3,
                timePeriod = "weekly"
            });
        }

        // GET: api/leaderboard/friends/monthly
        [HttpGet("friends/monthly")]
        public IActionResult GetMonthlyFriendLeaderboard([FromQuery] int userId)
        {
            // In a real implementation, this would return monthly friend leaderboard data
            var leaderboard = new List<object>
            {
                new { rank = 1, username = "friend1", score = 2200 },
                new { rank = 2, username = "friend2", score = 2000 },
                new { rank = 3, username = "friend3", score = 1800 }
            };

            return Ok(new
            {
                leaderboard = leaderboard,
                totalFriends = 3,
                timePeriod = "monthly"
            });
        }
    }
}