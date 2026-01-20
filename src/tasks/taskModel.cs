using System;
using System.ComponentModel.DataAnnotations;

namespace SosyalApp1.src.tasks
{
    public class TaskModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string DifficultyLevel { get; set; } // easy, medium, hard
        
        public string Status { get; set; } // assigned, pending_evidence, completed
        
        public int AssignedUserId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        // Evidence-related properties
        public string EvidenceFileName { get; set; }
        public string EvidenceFilePath { get; set; }
        public string EvidenceFileType { get; set; }
        public long? EvidenceFileSize { get; set; }
    }
}