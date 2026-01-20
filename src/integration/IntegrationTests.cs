using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SosyalApp1.src.integration
{
    [TestClass]
    public class IntegrationTests
    {
        private static WebApplicationFactory<Program> _factory;
        private static HttpClient _client;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        // Test complete registration and login flow
        [TestMethod]
        public async Task TestRegistrationAndLoginFlow()
        {
            // Arrange
            var registrationData = new
            {
                username = "testuser",
                email = "test@example.com",
                password = "Password123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/userjourney/register-login", registrationData);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // Test dashboard display
        [TestMethod]
        public async Task TestDashboardDisplay()
        {
            // Act
            var response = await _client.GetAsync("/api/userjourney/dashboard");

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // Test task completion workflow
        [TestMethod]
        public async Task TestTaskCompletionWorkflow()
        {
            // Arrange
            var taskData = new
            {
                evidenceFile = new { } // Mock file data
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/userjourney/complete-task/1", taskData);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // Test friend management and task recommendations
        [TestMethod]
        public async Task TestFriendManagementAndRecommendations()
        {
            // Act
            var response = await _client.GetAsync("/api/userjourney/friends");

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // Test leaderboard access
        [TestMethod]
        public async Task TestLeaderboardAccess()
        {
            // Act
            var response = await _client.GetAsync("/api/userjourney/leaderboard");

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // Test friend leaderboard access
        [TestMethod]
        public async Task TestFriendLeaderboardAccess()
        {
            // Act
            var response = await _client.GetAsync("/api/userjourney/leaderboard/friends?userId=1");

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // Test complete end-to-end user journey
        [TestMethod]
        public async Task TestCompleteUserJourney()
        {
            // Test registration/login
            var registrationData = new
            {
                username = "endtoenduser",
                email = "endtoend@example.com",
                password = "Password123!"
            };
            var registerResponse = await _client.PostAsJsonAsync("/api/userjourney/register-login", registrationData);
            Assert.IsTrue(registerResponse.IsSuccessStatusCode);

            // Test dashboard
            var dashboardResponse = await _client.GetAsync("/api/userjourney/dashboard");
            Assert.IsTrue(dashboardResponse.IsSuccessStatusCode);

            // Test friend management
            var friendsResponse = await _client.GetAsync("/api/userjourney/friends");
            Assert.IsTrue(friendsResponse.IsSuccessStatusCode);

            // Test leaderboard
            var leaderboardResponse = await _client.GetAsync("/api/userjourney/leaderboard");
            Assert.IsTrue(leaderboardResponse.IsSuccessStatusCode);

            // Test task completion (mock)
            var taskData = new
            {
                evidenceFile = new { }
            };
            var taskResponse = await _client.PostAsJsonAsync("/api/userjourney/complete-task/1", taskData);
            Assert.IsTrue(taskResponse.IsSuccessStatusCode);
        }
    }
}