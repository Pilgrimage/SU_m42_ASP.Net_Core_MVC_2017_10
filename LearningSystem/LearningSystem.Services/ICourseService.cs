namespace LearningSystem.Services
{
    using LearningSystem.Services.Models.Courses;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICourseService
    {
        Task<IEnumerable<CourseListingServiceModel>> UpcomingAsync();

        Task<IEnumerable<CourseListingServiceModel>> FindAsync(string searchText);

        Task<TModel> ByIdAsync<TModel>(int id) where TModel : class;

        Task<bool> IsUserSignedInCourseAsync(int courseId, string userId);

        Task<bool> SignUpUserAsync(int courseId, string userId);

        Task<bool> SignOutUserAsync(int courseId, string userId);

        Task<bool> SaveExamSubmissionAsync(int courseId, string userId, byte[] examSubmission, string fileName);
    }
}