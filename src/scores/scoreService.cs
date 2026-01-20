using System;
using System.Collections.Generic;
using System.Linq;

namespace SosyalApp1.src.scores
{
    public class ScoreService
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
        public static int GetWeeklyScoreForUser(int userId, DateTime weekStartDate)
        {
            if (!_scoreHistory.ContainsKey(userId))
            {
                return 0;
            }

            var weekEndDate = weekStartDate.AddDays(7);
            return _scoreHistory[userId]
                .Where(item => item.DateCompleted >= weekStartDate && item.DateCompleted < weekEndDate)
                .Sum(item => item.PointsEarned);
        }

        // Get monthly score aggregation for a user
        public static int GetMonthlyScoreForUser(int userId, DateTime monthStartDate)
        {
            if (!_scoreHistory.ContainsKey(userId))
            {
                return 0;
            }

            var monthEndDate = monthStartDate.AddMonths(1);
            return _scoreHistory[userId]
                .Where(item => item.DateCompleted >= monthStartDate && item.DateCompleted < monthEndDate)
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

        // Update point values for difficulty levels
        public static void UpdatePointValues(Dictionary<string, int> newPointValues)
        {
            _pointValues = newPointValues;
        }
    }

    // Class to represent score history items
    public class ScoreHistoryItem
    {
        public int TaskId { get; set; }
        public string DifficultyLevel { get; set; }
        public int PointsEarned { get; set; }
        public DateTime DateCompleted { get; set; }
    }
}