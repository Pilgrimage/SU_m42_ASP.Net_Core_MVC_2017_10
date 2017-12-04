﻿namespace LearningSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CourseNameMinLength)]
        [MaxLength(CourseNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CourseDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string TrainerId { get; set; }
        public User Trainer { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<StudentCourse> Students { get; set; } = new List<StudentCourse>();

        public List<Exam> Exams { get; set; } = new List<Exam>();
    }
}