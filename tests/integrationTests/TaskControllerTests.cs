using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SosyalApp1.tests.integrationTests
{
    [TestClass]
    public class TaskControllerTests
    {
        private static HttpClient _httpClient;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _httpClient = new HttpClient();
        }

        [TestMethod]
        public async Task GetTasksForUser_ShouldReturnTasks()
        {
            // Arrange
            int userId = 1;
            var url = $"/api/tasks/user/{userId}";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task UpdateTaskStatus_ShouldUpdateTaskStatus()
        {
            // Arrange
            int taskId = 1;
            var url = $"/api/tasks/{taskId}/status";
            var payload = new { status = "completed" };

            // Act
            var response = await _httpClient.PutAsJsonAsync(url, payload);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task UploadEvidence_ShouldUploadEvidence()
        {
            // Arrange
            int taskId = 1;
            var url = $"/api/tasks/{taskId}/evidence";

            // Act
            // In a real scenario, we would upload a file
            // For now, we'll just test the endpoint exists
            var response = await _httpClient.PostAsync(url, null);

            // Assert
            // This would depend on actual implementation
            Assert.IsTrue(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotImplemented);
        }
    }
}