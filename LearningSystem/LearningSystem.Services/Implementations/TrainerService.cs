namespace LearningSystem.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using LearningSystem.Data;
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Models;
    using LearningSystem.Services.Models.Courses;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TrainerService : ITrainerService
    {
        public readonly LearningSystemDbContext db;

        public TrainerService(LearningSystemDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<CourseListingServiceModel>> CoursesAsync(string userId)
        {
            return await this.db
                .Courses
                .Where(c => c.TrainerId == userId)
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();
        }


        public async Task<IEnumerable<StudentInCourseServiceModel>> StudentsInCourseAsync(int courseId)
        {
            return await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Students.Select(s => s.Student))
                .ProjectTo<StudentInCourseServiceModel>(new { courseId = courseId })
                .ToListAsync();

        }


        public async Task<bool> IsTrainerAsync(int courseId, string trainerId)
        {
            return await this.db
                .Courses
                .AnyAsync(c => c.Id == courseId && c.TrainerId == trainerId);
        }


        public async Task<bool> SetGradeAsync(int courseId, string studentId, Grade grade)
        {
            var studentInCourse = await this.db
                .FindAsync<StudentCourse>(courseId, studentId);

            if (studentInCourse == null)
            {
                return false;
            }

            studentInCourse.Grade = grade;

            await this.db.SaveChangesAsync();

            return true;
        }


        public async Task<byte[]> GetExamLastSubmissionAsync(int courseId, string studentId)
        {
            return (await this.db
                .Exams
                .Where(e => e.CourseId == courseId && e.StudentId == studentId)
                .OrderByDescending(e => e.SubmissionTime)
                .FirstOrDefaultAsync())
                ?.ExamSubmission;
        }


        public async Task<ExamStudentCourseServiceModel> GetExamFullInfoAsync(int courseId, string studentId)
        {
            return await this.db
                .Exams
                .Where(e => e.CourseId == courseId && e.StudentId == studentId)
                .OrderByDescending(e => e.SubmissionTime)
                .ProjectTo<ExamStudentCourseServiceModel>()
                .FirstOrDefaultAsync();
        }


        public async Task<StudentInCourseNamesServiceModel> StudentInCourseNamesAsync(int courseId, string studentId)
        {
            var username = await this.db
                .Users
                .Where(u => u.Id == studentId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            if (username==null)
            {
                return null;
            }

            var coursename = await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(u => u.Name)
                .FirstOrDefaultAsync();

            if (coursename == null)
            {
                return null;
            }

            return new StudentInCourseNamesServiceModel
            {
                CourseName = coursename,
                Username = username
            };
        }
    }
}