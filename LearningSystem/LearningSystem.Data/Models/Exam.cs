namespace LearningSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Exam
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string StudentId { get; set; }
        public User Student { get; set; }

        [Required]
        [MaxLength(CourseExamSubmissionFileLength)]
        public byte[] ExamSubmission { get; set; }

        [Required]
        public string ExamFileName { get; set; }    

        [Required]
        public DateTime SubmissionTime { get; set; }
    }
}
