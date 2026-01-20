using System;
using System.Collections.Generic;
using System.Linq;

namespace SosyalApp1.src.services
{
    public class LeaderboardService
    {
        // Predefined point values for different difficulty levels
        private static Dictionary<string, int> _pointValues = new Dictionary<string, int>
        {
            { "easy", 10 },
            { "medium", 25 },
            { "hard", 50 }
        };

        // Store score history for each user
        private static Dictionary<int, List<ScoreHistoryItem>> _scoreHistory = new Dictionary<int, List<ScoreHistoryItem>>();

        // Calculate points for a task based on its difficulty
        public static int CalculatePointsForTask(string difficultyLevel)
        {
            if (_pointValues.ContainsKey(difficultyLevel))
            {
                return _pointValues[difficultyLevel];
            }

            // Default point value if difficulty level is not recognized
            return 0;
        }

        // Add score history for a completed task
        public static void AddScoreHistory(int userId, int taskId, string difficultyLevel, int pointsEarned)
        {
            if (!_scoreHistory.ContainsKey(userId))
            {
                _scoreHistory[userId] = new List<ScoreHistoryItem>();
            }

            var scoreHistoryItem = new ScoreHistoryItem
            {
                TaskId = taskId,
                DifficultyLevel = difficultyLevel,
                PointsEarned = pointsEarned,
                DateCompleted = DateTime.UtcNow
            };

            _scoreHistory[userId].Add(scoreHistoryItem);
        }

        // Get total points for a user
        public static int GetTotalPointsForUser(int userId)
        {
            if (!_scoreHistory.ContainsKey(userId))
            {
                return 0;
            }

            return _scoreHistory[userId].Sum(item => item.PointsEarned);
        }

        // Get daily score aggregation for a user
        public static int GetDailyScoreForUser(int userId, DateTime date)
        {
            if (!_scoreHistory.ContainsKey(userId))
            {
                return 0;
            }

            return _scoreHistory[userId]
                .Where(item => item.DateCompleted.Date == date.Date)
                .Sum(item => item.PointsEarned);
        }

        // Get weekly score aggregation for a user
        public static int GetWeeklyScoreForUser(int userId, DateTime weekStart_date)
        {
            if (!_scoreHistory.ContainsKey(userId))
            {
                return 0;
            }

            var weekEndDate = weekStart_date.AddDays(7);
            return _scoreHistory[userId]
                .Where(item => item.DateCompleted >= weekStart_date && item.DateCompleted < weekEndDate)
                .Sum(item => item.PointsEarned);
        }

        // Get monthly score aggregation for a user
        public static int GetMonthlyScoreForUser(int userId, DateTime monthStart_date)
        {
            if (!_scoreHistory.ContainsKey(userId))
            {
                return 0;
            }

            var monthEndDate = monthStart_date.AddMonths(1);
            return _scoreHistory[userId]
                .Where(item => item.DateCompleted >= monthStart_date && item.DateCompleted < monthEndDate)
                .Sum(item => item.PointsEarned);
        }

        // Get score history for a user
        public static List<ScoreHistoryItem> GetScoreHistoryForUser(int userId)
        {
            if (!_scoreHistory.ContainsKey(userId))
            {
                return new List<ScoreHistoryItem>();
            }

            return _scoreHistory[userId];
        }

        // Get global leaderboard (top users)
        public static List<LeaderboardItem> GetGlobalLeaderboard(int limit = 10)
        {
            var leaderboard = new List<LeaderboardItem>();

            foreach (var user in _scoreHistory)
            {
                leaderboard.Add(new LeaderboardItem
                {
                    UserId = user.Key,
                    TotalPoints = user.Value.Sum(item => item.PointsEarned),
                    Username = $"user{user.Key}" // In a real app, this would come from a user service
                });
            }

            return leaderboard.OrderByDescending(l => l.TotalPoints).Take(limit).ToList();
        }

        // Get friend leaderboard (only connected users)
        public static List<LeaderboardItem> GetFriendLeaderboard(int userId, int limit = 10)
        {
            // In a real implementation, this would filter by friends only
            // For now, we'll return a mock list of friends
            var friendIds = new List<int> { userId + 1, userId + 2, userId + 3 };
            
            var leaderboard = new List<LeaderboardItem>();

            foreach (var friendId in friendIds)
            {
                if (_scoreHistory.ContainsKey(friendId))
                {
                    leaderboard.Add(new LeaderboardItem
                    {
                        UserId = friendId,
                        TotalPoints = _scoreHistory[friendId].Sum(item => item.PointsEarned),
                        Username = $"friend{friendId}" // In a real app, this would come from a user service
                    });
                }
            }

            return leaderboard.OrderByDescending(l => l.TotalPoints).Take(limit).ToList();
        }

        // Update point values for difficulty levels
        public static void UpdatePointValues(Dictionary<string, int> newPointValues)
        {
            _pointValues = newPointValues;
        }
    }

    // Class to represent a score history item
    public class ScoreHistoryItem
    {
        public int TaskId { get; set; }
        public string DifficultyLevel { get; set; }
        public int PointsEarned { get; set; }
        public DateTime DateCompleted { get; set; }
    }

    // Class to represent a leaderboard item
    public class LeaderboardItem
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int TotalPoints { get; set; }
    }
}