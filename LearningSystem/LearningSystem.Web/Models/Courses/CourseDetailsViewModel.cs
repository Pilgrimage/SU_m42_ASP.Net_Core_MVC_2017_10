namespace LearningSystem.Web.Models.Courses
{
    using LearningSystem.Services.Models.Courses;

    public class CourseDetailsViewModel
    {
        public CourseDetailsServiceModel Course { get; set; }

        public bool IsUserSignedInCourse { get; set; }
    }
}