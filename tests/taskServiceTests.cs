using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SosyalApp1.src.tasks;

namespace SosyalApp1.tests
{
    [TestClass]
    public class TaskServiceTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Clear tasks before each test
            TaskService.GetType().GetField("_tasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<TaskModel>());
            
            // Reset ID counter
            TaskService.GetType().GetField("_nextId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, 1);
        }

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
            
            // Check that all tasks have the correct user ID
            foreach (var task in tasks)
            {
                Assert.AreEqual(userId, task.AssignedUserId);
                Assert.AreEqual("assigned", task.Status);
            }
        }

        [TestMethod]
        public void GenerateTasksForUser_ShouldAssignDifferentDifficultyLevels()
        {
            // Arrange
            int userId = 1;
            
            // Act
            var tasks = TaskService.GenerateTasksForUser(userId);
            
            // Assert
            var difficultyLevels = tasks.Select(t => t.DifficultyLevel).ToList();
            Assert.IsTrue(difficultyLevels.Contains("easy"));
            Assert.IsTrue(difficultyLevels.Contains("medium"));
            Assert.IsTrue(difficultyLevels.Contains("hard"));
        }

        [TestMethod]
        public void GetTasksForUser_ShouldReturnOnlyUsersTasks()
        {
            // Arrange
            int userId1 = 1;
            int userId2 = 2;
            
            TaskService.GenerateTasksForUser(userId1);
            TaskService.GenerateTasksForUser(userId2);
            
            // Act
            var user1Tasks = TaskService.GetTasksForUser(userId1);
            var user2Tasks = TaskService.GetTasksForUser(userId2);
            
            // Assert
            Assert.AreEqual(3, user1Tasks.Count);
            Assert.AreEqual(3, user2Tasks.Count);
            
            // Check that tasks belong to correct users
            foreach (var task in user1Tasks)
            {
                Assert.AreEqual(userId1, task.AssignedUserId);
            }
            
            foreach (var task in user2Tasks)
            {
                Assert.AreEqual(userId2, task.AssignedUserId);
            }
        }

        [TestMethod]
        public void UpdateTaskStatus_ShouldUpdateStatusSuccessfully()
        {
            // Arrange
            int userId = 1;
            var tasks = TaskService.GenerateTasksForUser(userId);
            int taskId = tasks.First().Id;
            
            // Act
            var result = TaskService.UpdateTaskStatus(taskId, "pending_evidence");
            
            // Assert
            Assert.IsTrue(result);
            
            var updatedTask = TaskService.GetTaskDetails(taskId);
            Assert.AreEqual("pending_evidence", updatedTask.Status);
        }

        [TestMethod]
        public void UpdateTaskStatus_ShouldValidateStatusTransitions()
        {
            // Arrange
            int userId = 1;
            var tasks = TaskService.GenerateTasksForUser(userId);
            int taskId = tasks.First().Id;
            
            // Act & Assert - Valid transition
            Assert.IsTrue(TaskService.UpdateTaskStatus(taskId, "pending_evidence"));
            
            // Try invalid transition
            Assert.IsFalse(TaskService.UpdateTaskStatus(taskId, "assigned"));
        }

        [TestMethod]
        public void UpdateTaskStatus_ShouldSetCompletedAtWhenTaskCompleted()
        {
            // Arrange
            int userId = 1;
            var tasks = TaskService.GenerateTasksForUser(userId);
            int taskId = tasks.First().Id;
            
            // Act
            TaskService.UpdateTaskStatus(taskId, "completed");
            
            // Assert
            var completedTask = TaskService.GetTaskDetails(taskId);
            Assert.IsNotNull(completedTask.CompletedAt);
        }

        [TestMethod]
        public void GetTaskDetails_ShouldReturnCorrectTask()
        {
            // Arrange
            int userId = 1;
            var tasks = TaskService.GenerateTasksForUser(userId);
            int taskId = tasks.First().Id;
            
            // Act
            var task = TaskService.GetTaskDetails(taskId);
            
            // Assert
            Assert.IsNotNull(task);
            Assert.AreEqual(taskId, task.Id);
            Assert.AreEqual(userId, task.AssignedUserId);
        }

        [TestMethod]
        public void GetTaskDetails_ShouldReturnNullForNonExistentTask()
        {
            // Act
            var task = TaskService.GetTaskDetails(999);
            
            // Assert
            Assert.IsNull(task);
        }
    }
}