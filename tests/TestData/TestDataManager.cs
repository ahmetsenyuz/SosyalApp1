using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SosyalApp1.tests.TestData
{
    public static class TestDataManager
    {
        private static readonly string TestDataDirectory = "tests/TestData";

        public static T LoadTestData<T>(string fileName)
        {
            var filePath = Path.Combine(TestDataDirectory, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Test data file not found: {filePath}");

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }

        public static void SaveTestData<T>(T data, string fileName)
        {
            var filePath = Path.Combine(TestDataDirectory, fileName);
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void ClearTestData(string fileName)
        {
            var filePath = Path.Combine(TestDataDirectory, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static List<string> GetAllTestFiles()
        {
            if (!Directory.Exists(TestDataDirectory))
                return new List<string>();

            return new List<string>(Directory.GetFiles(TestDataDirectory, "*.json"));
        }
    }

    // Test data models
    public class TestUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class TestTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DifficultyLevel { get; set; }
        public string Status { get; set; }
        public int AssignedUserId { get; set; }
    }

    public class TestScoreHistory
    {
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public string DifficultyLevel { get; set; }
        public int PointsEarned { get; set; }
        public DateTime DateCompleted { get; set; }
    }
}