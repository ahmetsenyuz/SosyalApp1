using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SosyalApp1.tests.integrationTests
{
    [TestClass]
    public class ScoreControllerTests
    {
        private static HttpClient _httpClient;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _httpClient = new HttpClient();
        }

        [TestMethod]
        public async Task GetTotalPointsForUser_ShouldReturnPoints()
        {
            // Arrange
            int userId = 1;
            var url = $"/api/scores/user/{userId}/total";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task GetDailyScoreForUser_ShouldReturnDailyScore()
        {
            // Arrange
            int userId = 1;
            var url = $"/api/scores/user/{userId}/daily";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task GetWeeklyScoreForUser_ShouldReturnWeeklyScore()
        {
            // Arrange
            int userId = 1;
            var url = $"/api/scores/user/{userId}/weekly";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task GetMonthlyScoreForUser_ShouldReturnMonthlyScore()
        {
            // Arrange
            int userId = 1;
            var url = $"/api/scores/user/{userId}/monthly";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task GetScoreHistoryForUser_ShouldReturnHistory()
        {
            // Arrange
            int userId = 1;
            var url = $"/api/scores/user/{userId}/history";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}