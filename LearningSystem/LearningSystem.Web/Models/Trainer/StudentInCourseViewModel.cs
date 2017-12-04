namespace LearningSystem.Web.Models.Trainer
{
    using System.Collections.Generic;
    using LearningSystem.Services.Models;
    using LearningSystem.Services.Models.Courses;

    public class StudentInCourseViewModel
    {
        public IEnumerable<StudentInCourseServiceModel> Students { get; set; }

        public CourseListingServiceModel Course { get; set; }
    }
}