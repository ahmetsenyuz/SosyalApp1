using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SosyalApp1.src.scores;

namespace SosyalApp1.tests.unitTests
{
    [TestClass]
    public class ScoreServiceTests
    {
        [TestMethod]
        public void CalculatePointsForTask_ShouldReturnCorrectPoints()
        {
            // Arrange
            string difficultyLevel = "easy";

            // Act
            int points = ScoreService.CalculatePointsForTask(difficultyLevel);

            // Assert
            Assert.AreEqual(10, points);
        }

        [TestMethod]
        public void CalculatePointsForTask_WithUnknownDifficulty_ShouldReturnZero()
        {
            // Arrange
            string difficultyLevel = "unknown";

            // Act
            int points = ScoreService.CalculatePointsForTask(difficultyLevel);

            // Assert
            Assert.AreEqual(0, points);
        }

        [TestMethod]
        public void AddScoreHistory_ShouldAddScoreRecord()
        {
            // Arrange
            int userId = 1;
            int taskId = 1;
            string difficultyLevel = "medium";
            int pointsEarned = 25;

            // Act
            ScoreService.AddScoreHistory(userId, taskId, difficultyLevel, pointsEarned);

            // Assert
            var history = ScoreService.GetScoreHistoryForUser(userId);
            Assert.IsNotNull(history);
            Assert.IsTrue(history.Count > 0);
        }

        [TestMethod]
        public void GetTotalPointsForUser_ShouldReturnTotalPoints()
        {
            // Arrange
            int userId = 1;

            // Act
            int totalPoints = ScoreService.GetTotalPointsForUser(userId);

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
            int dailyScore = ScoreService.GetDailyScoreForUser(userId, date);

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
            int weeklyScore = ScoreService.GetWeeklyScoreForUser(userId, weekStart);

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
            int monthlyScore = ScoreService.GetMonthlyScoreForUser(userId, monthStart);

            // Assert
            Assert.IsTrue(monthlyScore >= 0);
        }

        [TestMethod]
        public void GetScoreHistoryForUser_ShouldReturnScoreHistory()
        {
            // Arrange
            int userId = 1;

            // Act
            var history = ScoreService.GetScoreHistoryForUser(userId);

            // Assert
            Assert.IsNotNull(history);
        }
    }
}