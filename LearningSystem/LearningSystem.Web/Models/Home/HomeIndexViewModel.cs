namespace LearningSystem.Web.Models.Home
{
    using System.Collections.Generic;
    using LearningSystem.Services.Models.Courses;

    public class HomeIndexViewModel : SearchFormModel
    {
        public IEnumerable<CourseListingServiceModel> Courses { get; set; } = new List<CourseListingServiceModel>();

    }
}