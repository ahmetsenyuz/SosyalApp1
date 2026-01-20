using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SosyalApp1.src.tasks;

namespace SosyalApp1.tests.unitTests
{
    [TestClass]
    public class TaskServiceTests
    {
        [TestMethod]
        public void GenerateTasksForUser_ShouldGenerateThreeTasks()
        {
            // Arrange
            int userId = 1;

            // Act
            var tasks = TaskService.GenerateTasksForUser(userId);

            // Assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(3, tasks.Count);
        }

        [TestMethod]
        public void GetTasksForUser_ShouldReturnTasksForSpecificUser()
        {
            // Arrange
            int userId = 1;

            // Act
            var tasks = TaskService.GetTasksForUser(userId);

            // Assert
            Assert.IsNotNull(tasks);
            Assert.IsTrue(tasks.All(t => t.AssignedUserId == userId));
        }

        [TestMethod]
        public void UpdateTaskStatus_WithValidTransition_ShouldUpdateStatus()
        {
            // Arrange
            int taskId = 1;
            string newStatus = "completed";

            // Act
            bool result = TaskService.UpdateTaskStatus(taskId, newStatus);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdateTaskStatus_WithInvalidTransition_ShouldReturnFalse()
        {
            // Arrange
            int taskId = 1;
            string newStatus = "assigned";

            // Act
            bool result = TaskService.UpdateTaskStatus(taskId, newStatus);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetTaskDetails_ShouldReturnTaskDetails()
        {
            // Arrange
            int taskId = 1;

            // Act
            var task = TaskService.GetTaskDetails(taskId);

            // Assert
            Assert.IsNotNull(task);
            Assert.AreEqual(taskId, task.Id);
        }

        [TestMethod]
        public void UploadEvidence_WithValidTask_ShouldUpdateTask()
        {
            // Arrange
            int taskId = 1;
            string fileName = "test.png";
            string filePath = "/uploads/test.png";
            string fileType = "image/png";
            long fileSize = 1024;

            // Act
            bool result = TaskService.UploadEvidence(taskId, fileName, filePath, fileType, fileSize);

            // Assert
            Assert.IsTrue(result);
        }
    }
}