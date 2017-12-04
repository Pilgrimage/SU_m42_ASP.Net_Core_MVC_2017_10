namespace LearningSystem.Services
{
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Models;
    using LearningSystem.Services.Models.Courses;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITrainerService
    {
        Task<IEnumerable<CourseListingServiceModel>> CoursesAsync(string userId);

        Task<IEnumerable<StudentInCourseServiceModel>> StudentsInCourseAsync(int courseId);

        Task<bool> IsTrainerAsync(int courseId, string trainerId);

        Task<bool> SetGradeAsync(int courseId, string studentId, Grade grade);

        Task<byte[]> GetExamLastSubmissionAsync(int courseId, string studentId);

        Task<ExamStudentCourseServiceModel> GetExamFullInfoAsync(int courseId, string studentId);

        Task<StudentInCourseNamesServiceModel> StudentInCourseNamesAsync(int courseId, string studentId);
    }
}