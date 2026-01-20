using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SosyalApp1.src.services;

namespace SosyalApp1.tests.unitTests
{
    [TestClass]
    public class LeaderboardServiceTests
    {
        [TestMethod]
        public void CalculatePointsForTask_ShouldReturnCorrectPoints()
        {
            // Arrange
            string difficultyLevel = "hard";

            // Act
            int points = LeaderboardService.CalculatePointsForTask(difficultyLevel);

            // Assert
            Assert.AreEqual(50, points);
        }

        [TestMethod]
        public void CalculatePointsForTask_WithUnknownDifficulty_ShouldReturnZero()
        {
            // Arrange
            string difficultyLevel = "unknown";

            // Act
            int points = LeaderboardService.CalculatePointsForTask(difficultyLevel);

            // Assert
            Assert.AreEqual(0, points);
        }

        [TestMethod]
        public void AddScoreHistory_ShouldAddScoreRecord()
        {
            // Arrange
            int userId = 1;
            int taskId = 1;
            string difficultyLevel = "easy";
            int pointsEarned = 10;

            // Act
            LeaderboardService.AddScoreHistory(userId, taskId, difficultyLevel, pointsEarned);

            // Assert
            var history = LeaderboardService.GetScoreHistoryForUser(userId);
            Assert.IsNotNull(history);
            Assert.IsTrue(history.Count > 0);
        }

        [TestMethod]
        public void GetTotalPointsForUser_ShouldReturnTotalPoints()
        {
            // Arrange
            int userId = 1;

            // Act
            int totalPoints = LeaderboardService.GetTotalPointsForUser(userId);

            // Assert
            Assert.IsTrue(totalPoints >= 0);
        }

        [TestMethod]
        public void GetDailyScoreForUser_ShouldReturnDailyScore()
        {
            // Arrange
            int userId = 1;
            DateTime date = DateTime.UtcNow.Date;

            // Act
            int dailyScore = LeaderboardService.GetDailyScoreForUser(userId, date);

            // Assert
            Assert.IsTrue(dailyScore >= 0);
        }

        [TestMethod]
        public void GetWeeklyScoreForUser_ShouldReturnWeeklyScore()
        {
            // Arrange
            int userId = 1;
            DateTime weekStart = DateTime.UtcNow.Date.AddDays(-7);

            // Act
            int weeklyScore = LeaderboardService.GetWeeklyScoreForUser(userId, weekStart);

            // Assert
            Assert.IsTrue(weeklyScore >= 0);
        }

        [TestMethod]
        public void GetMonthlyScoreForUser_ShouldReturnMonthlyScore()
        {
            // Arrange
            int userId = 1;
            DateTime monthStart = DateTime.UtcNow.Date.AddMonths(-1);

            // Act
            int monthlyScore = LeaderboardService.GetMonthlyScoreForUser(userId, monthStart);

            // Assert
            Assert.IsTrue(monthlyScore >= 0);
        }

        [TestMethod]
        public void GetScoreHistoryForUser_ShouldReturnScoreHistory()
        {
            // Arrange
            int userId = 1;

            // Act
            var history = LeaderboardService.GetScoreHistoryForUser(userId);

            // Assert
            Assert.IsNotNull(history);
        }

        [TestMethod]
        public void GetGlobalLeaderboard_ShouldReturnLeaderboard()
        {
            // Arrange
            int limit = 10;

            // Act
            var leaderboard = LeaderboardService.GetGlobalLeaderboard(limit);

            // Assert
            Assert.IsNotNull(leaderboard);
        }

        [TestMethod]
        public void GetFriendLeaderboard_ShouldReturnFriendLeaderboard()
        {
            // Arrange
            int userId = 1;
            int limit = 10;

            // Act
            var leaderboard = LeaderboardService.GetFriendLeaderboard(userId, limit);

            // Assert
            Assert.IsNotNull(leaderboard);
        }
    }
}