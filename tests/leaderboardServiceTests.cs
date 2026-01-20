using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SosyalApp1.src.services;

namespace SosyalApp1.tests
{
    [TestClass]
    public class LeaderboardServiceTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Clear score history before each test
            var scoreHistoryField = typeof(LeaderboardService).GetField("_scoreHistory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            scoreHistoryField?.SetValue(null, new Dictionary<int, List<ScoreHistoryItem>>());

            // Reset point values
            var pointValuesField = typeof(LeaderboardService).GetField("_pointValues", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var newPointValues = new Dictionary<string, int>
            {
                { "easy", 10 },
                { "medium", 25 },
                { "hard", 50 }
            };
            pointValuesField?.SetValue(null, newPointValues);
        }

        [TestMethod]
        public void CalculatePointsForTask_ShouldReturnCorrectPointsForDifficultyLevels()
        {
            // Arrange
            var easyPoints = LeaderboardService.CalculatePointsForTask("easy");
            var mediumPoints = LeaderboardService.CalculatePointsForTask("medium");
            var hardPoints = LeaderboardService.CalculatePointsForTask("hard");
            var unknownPoints = LeaderboardService.CalculatePointsForTask("unknown");

            // Act & Assert
            Assert.AreEqual(10, easyPoints);
            Assert.AreEqual(25, mediumPoints);
            Assert.AreEqual(50, hardPoints);
            Assert.AreEqual(0, unknownPoints);
        }

        [TestMethod]
        public void AddScoreHistory_ShouldAddHistoryItemCorrectly()
        {
            // Arrange
            int userId = 1;
            int taskId = 101;
            string difficultyLevel = "easy";
            int pointsEarned = 10;

            // Act
            LeaderboardService.AddScoreHistory(userId, taskId, difficultyLevel, pointsEarned);

            // Assert
            var scoreHistory = LeaderboardService.GetScoreHistoryForUser(userId);
            Assert.IsNotNull(scoreHistory);
            Assert.AreEqual(1, scoreHistory.Count);
            Assert.AreEqual(taskId, scoreHistory[0].TaskId);
            Assert.AreEqual(difficultyLevel, scoreHistory[0].DifficultyLevel);
            Assert.AreEqual(pointsEarned, scoreHistory[0].PointsEarned);
        }

        [TestMethod]
        public void GetTotalPointsForUser_ShouldReturnCorrectTotalPoints()
        {
            // Arrange
            int userId = 1;
            LeaderboardService.AddScoreHistory(userId, 101, "easy", 10);
            LeaderboardService.AddScoreHistory(userId, 102, "medium", 25);
            LeaderboardService.AddScoreHistory(userId, 103, "hard", 50);

            // Act
            var totalPoints = LeaderboardService.GetTotalPointsForUser(userId);

            // Assert
            Assert.AreEqual(85, totalPoints);
        }

        [TestMethod]
        public void GetDailyScoreForUser_ShouldReturnCorrectDailyScore()
        {
            // Arrange
            int userId = 1;
            var today = DateTime.UtcNow.Date;
            LeaderboardService.AddScoreHistory(userId, 101, "easy", 10);
            LeaderboardService.AddScoreHistory(userId, 102, "medium", 25);

            // Act
            var dailyScore = LeaderboardService.GetDailyScoreForUser(userId, today);

            // Assert
            Assert.AreEqual(35, dailyScore);
        }

        [TestMethod]
        public void GetWeeklyScoreForUser_ShouldReturnCorrectWeeklyScore()
        {
            // Arrange
            int userId = 1;
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-7);
            LeaderboardService.AddScoreHistory(userId, 101, "easy", 10);
            LeaderboardService.AddScoreHistory(userId, 102, "medium", 25);

            // Act
            var weeklyScore = LeaderboardService.GetWeeklyScoreForUser(userId, startOfWeek);

            // Assert
            Assert.AreEqual(35, weeklyScore);
        }

        [TestMethod]
        public void GetMonthlyScoreForUser_ShouldReturnCorrectMonthlyScore()
        {
            // Arrange
            int userId = 1;
            var startOfMonth = DateTime.UtcNow.Date.AddMonths(-1);
            LeaderboardService.AddScoreHistory(userId, 101, "easy", 10);
            LeaderboardService.AddScoreHistory(userId, 102, "medium", 25);

            // Act
            var monthlyScore = LeaderboardService.GetMonthlyScoreForUser(userId, startOfMonth);

            // Assert
            Assert.AreEqual(35, monthlyScore);
        }

        [TestMethod]
        public void GetScoreHistoryForUser_ShouldReturnCorrectHistory()
        {
            // Arrange
            int userId = 1;
            LeaderboardService.AddScoreHistory(userId, 101, "easy", 10);
            LeaderboardService.AddScoreHistory(userId, 102, "medium", 25);

            // Act
            var scoreHistory = LeaderboardService.GetScoreHistoryForUser(userId);

            // Assert
            Assert.IsNotNull(scoreHistory);
            Assert.AreEqual(2, scoreHistory.Count);
            Assert.IsTrue(scoreHistory.Any(h => h.TaskId == 101 && h.PointsEarned == 10));
            Assert.IsTrue(scoreHistory.Any(h => h.TaskId == 102 && h.PointsEarned == 25));
        }

        [TestMethod]
        public void GetGlobalLeaderboard_ShouldReturnCorrectLeaderboard()
        {
            // Arrange
            LeaderboardService.AddScoreHistory(1, 101, "easy", 10);
            LeaderboardService.AddScoreHistory(2, 102, "medium", 25);
            LeaderboardService.AddScoreHistory(3, 103, "hard", 50);

            // Act
            var leaderboard = LeaderboardService.GetGlobalLeaderboard(3);

            // Assert
            Assert.IsNotNull(leaderboard);
            Assert.AreEqual(3, leaderboard.Count);
            Assert.AreEqual(3, leaderboard[0].UserId); // User with highest score
            Assert.AreEqual(50, leaderboard[0].TotalPoints);
            Assert.AreEqual(2, leaderboard[1].UserId); // User with second highest score
            Assert.AreEqual(25, leaderboard[1].TotalPoints);
            Assert.AreEqual(1, leaderboard[2].UserId); // User with third highest score
            Assert.AreEqual(10, leaderboard[2].TotalPoints);
        }

        [TestMethod]
        public void GetFriendLeaderboard_ShouldReturnCorrectFriendLeaderboard()
        {
            // Arrange
            int userId = 1;
            LeaderboardService.AddScoreHistory(2, 101, "easy", 10);
            LeaderboardService.AddScoreHistory(3, 102, "medium", 25);
            LeaderboardService.AddScoreHistory(4, 103, "hard", 50);

            // Act
            var leaderboard = LeaderboardService.GetFriendLeaderboard(userId, 3);

            // Assert
            Assert.IsNotNull(leaderboard);
            Assert.AreEqual(3, leaderboard.Count);
            Assert.IsTrue(leaderboard.Any(l => l.UserId == 2 && l.TotalPoints == 10));
            Assert.IsTrue(leaderboard.Any(l => l.UserId == 3 && l.TotalPoints == 25));
            Assert.IsTrue(leaderboard.Any(l => l.UserId == 4 && l.TotalPoints == 50));
        }

        [TestMethod]
        public void UpdatePointValues_ShouldUpdatePointValuesCorrectly()
        {
            // Arrange
            var newPointValues = new Dictionary<string, int>
            {
                { "easy", 15 },
                { "medium", 30 },
                { "hard", 60 }
            };

            // Act
            LeaderboardService.UpdatePointValues(newPointValues);

            // Assert
            Assert.AreEqual(15, LeaderboardService.CalculatePointsForTask("easy"));
            Assert.AreEqual(30, LeaderboardService.CalculatePointsForTask("medium"));
            Assert.AreEqual(60, LeaderboardService.CalculatePointsForTask("hard"));
        }
    }
}