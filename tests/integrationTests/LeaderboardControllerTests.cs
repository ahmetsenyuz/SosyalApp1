using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SosyalApp1.tests.integrationTests
{
    [TestClass]
    public class LeaderboardControllerTests
    {
        private static HttpClient _httpClient;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _httpClient = new HttpClient();
        }

        [TestMethod]
        public async Task GetGlobalLeaderboard_ShouldReturnLeaderboard()
        {
            // Arrange
            var url = "/api/leaderboard/global";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task GetFriendLeaderboard_ShouldReturnFriendLeaderboard()
        {
            // Arrange
            int userId = 1;
            var url = $"/api/leaderboard/user/{userId}/friends";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}