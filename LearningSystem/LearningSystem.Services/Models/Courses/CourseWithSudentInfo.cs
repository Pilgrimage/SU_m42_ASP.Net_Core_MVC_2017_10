namespace LearningSystem.Services.Models.Courses
{
    using System;

    public class CourseWithSudentInfo
    {
        public DateTime StartDate { get; set; }

        public bool UserIdSignedIn { get; set; }
    }
}