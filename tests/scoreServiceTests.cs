using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SosyalApp1.src.scores;

namespace SosyalApp1.tests
{
    [TestClass]
    public class ScoreServiceTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Clear score history before each test
            var scoreHistoryField = typeof(ScoreService).GetField("_scoreHistory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            scoreHistoryField?.SetValue(null, new Dictionary<int, List<ScoreHistoryItem>>());
            
            // Reset point values
            var pointValuesField = typeof(ScoreService).GetField("_pointValues", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
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
            var easyPoints = ScoreService.CalculatePointsForTask("easy");
            var mediumPoints = ScoreService.CalculatePointsForTask("medium");
            var hardPoints = ScoreService.CalculatePointsForTask("hard");
            var unknownPoints = ScoreService.CalculatePointsForTask("unknown");

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
            ScoreService.AddScoreHistory(userId, taskId, difficultyLevel, pointsEarned);

            // Assert
            var scoreHistory = ScoreService.GetScoreHistoryForUser(userId);
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
            ScoreService.AddScoreHistory(userId, 101, "easy", 10);
            ScoreService.AddScoreHistory(userId, 102, "medium", 25);
            ScoreService.AddScoreHistory(userId, 103, "hard", 50);

            // Act
            var totalPoints = ScoreService.GetTotalPointsForUser(userId);

            // Assert
            Assert.AreEqual(85, totalPoints);
        }

        [TestMethod]
        public void GetDailyScoreForUser_ShouldReturnCorrectDailyScore()
        {
            // Arrange
            int userId = 1;
            var today = DateTime.UtcNow.Date;
            ScoreService.AddScoreHistory(userId, 101, "easy", 10);
            ScoreService.AddScoreHistory(userId, 102, "medium", 25);

            // Act
            var dailyScore = ScoreService.GetDailyScoreForUser(userId, today);

            // Assert
            Assert.AreEqual(35, dailyScore);
        }

        [TestMethod]
        public void GetWeeklyScoreForUser_ShouldReturnCorrectWeeklyScore()
        {
            // Arrange
            int userId = 1;
            var startDate = DateTime.UtcNow.Date.AddDays(-7);
            ScoreService.AddScoreHistory(userId, 101, "easy", 10);
            ScoreService.AddScoreHistory(userId, 102, "medium", 25);

            // Act
            var weeklyScore = ScoreService.GetWeeklyScoreForUser(userId, startDate);

            // Assert
            Assert.AreEqual(35, weeklyScore);
        }

        [TestMethod]
        public void GetMonthlyScoreForUser_ShouldReturnCorrectMonthlyScore()
        {
            // Arrange
            int userId = 1;
            var startDate = DateTime.UtcNow.Date.AddMonths(-1);
            ScoreService.AddScoreHistory(userId, 101, "easy", 10);
            ScoreService.AddScoreHistory(userId, 102, "medium", 25);

            // Act
            var monthlyScore = ScoreService.GetMonthlyScoreForUser(userId, startDate);

            // Assert
            Assert.AreEqual(35, monthlyScore);
        }

        [TestMethod]
        public void GetScoreHistoryForUser_ShouldReturnCorrectHistory()
        {
            // Arrange
            int userId = 1;
            ScoreService.AddScoreHistory(userId, 101, "easy", 10);
            ScoreService.AddScoreHistory(userId, 102, "medium", 25);

            // Act
            var scoreHistory = ScoreService.GetScoreHistoryForUser(userId);

            // Assert
            Assert.IsNotNull(scoreHistory);
            Assert.AreEqual(2, scoreHistory.Count);
            Assert.IsTrue(scoreHistory.Any(h => h.TaskId == 101 && h.PointsEarned == 10));
            Assert.IsTrue(scoreHistory.Any(h => h.TaskId == 102 && h.PointsEarned == 25));
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
            ScoreService.UpdatePointValues(newPointValues);

            // Assert
            Assert.AreEqual(15, ScoreService.CalculatePointsForTask("easy"));
            Assert.AreEqual(30, ScoreService.CalculatePointsForTask("medium"));
            Assert.AreEqual(60, ScoreService.CalculatePointsForTask("hard"));
        }
    }
}