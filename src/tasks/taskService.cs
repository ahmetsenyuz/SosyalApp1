using System;
using System.Collections.Generic;
using System.Linq;

namespace SosyalApp1.src.tasks
{
    public class TaskService
    {
        private static List<TaskModel> _tasks = new List<TaskModel>();
        private static int _nextId = 1;

        // Task generation algorithm based on difficulty levels
        public static List<TaskModel> GenerateTasksForUser(int userId)
        {
            var tasks = new List<TaskModel>();
            
            // Generate 3 tasks per user
            var difficultyLevels = new[] { "easy", "medium", "hard" };
            
            foreach (var difficulty in difficultyLevels)
            {
                var task = new TaskModel
                {
                    Id = _nextId++,
                    Title = $"Task {difficulty} {userId}",
                    Description = $"Complete this {difficulty} task",
                    DifficultyLevel = difficulty,
                    Status = "assigned",
                    AssignedUserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                tasks.Add(task);
                _tasks.Add(task);
            }
            
            return tasks;
        }

        // Get tasks for a specific user
        public static List<TaskModel> GetTasksForUser(int userId)
        {
            return _tasks.Where(t => t.AssignedUserId == userId).ToList();
        }

        // Update task status
        public static bool UpdateTaskStatus(int taskId, string newStatus)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null) return false;

            // Validate status transitions
            if (!IsValidStatusTransition(task.Status, newStatus))
                return false;

            task.Status = newStatus;
            if (newStatus == "completed")
                task.CompletedAt = DateTime.UtcNow;
            
            return true;
        }

        // Validate status transitions
        private static bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            // Define valid transitions
            var validTransitions = new Dictionary<string, HashSet<string>>
            {
                { "assigned", new HashSet<string> { "pending_evidence", "completed" } },
                { "pending_evidence", new HashSet<string> { "completed" } },
                { "completed", new HashSet<string>() } // Can't change from completed
            };

            return validTransitions.ContainsKey(currentStatus) && 
                   validTransitions[currentStatus].Contains(newStatus);
        }

        // Get task details
        public static TaskModel GetTaskDetails(int taskId)
        {
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }

        // Upload evidence for a task
        public static bool UploadEvidence(int taskId, string fileName, string filePath, string fileType, long? fileSize)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null) return false;

            // Validate that task is in pending_evidence status
            if (task.Status != "pending_evidence") return false;

            // Store evidence information
            task.EvidenceFileName = fileName;
            task.EvidenceFilePath = filePath;
            task.EvidenceFileType = fileType;
            task.EvidenceFileSize = fileSize;

            // Update status to completed
            return UpdateTaskStatus(taskId, "completed");
        }
    }
}