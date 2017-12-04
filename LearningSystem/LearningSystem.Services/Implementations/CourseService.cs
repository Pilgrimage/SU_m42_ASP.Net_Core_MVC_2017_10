namespace LearningSystem.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using LearningSystem.Data;
    using LearningSystem.Services.Models.Courses;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LearningSystem.Data.Models;

    public class CourseService : ICourseService
    {
        private readonly LearningSystemDbContext db;

        public CourseService(LearningSystemDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<CourseListingServiceModel>> UpcomingAsync()
        {
            return await this.db
                .Courses
                .OrderByDescending(c => c.Id)
                .Where(c => c.StartDate > DateTime.UtcNow)
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();
        }


        public async Task<IEnumerable<CourseListingServiceModel>> FindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;
            return await this.db
                .Courses
                .OrderByDescending(c => c.Id)
                .Where(c => c.Name.ToLower().Contains(searchText.ToLower()))
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();
        }


        // Normal version
        //public async Task<CourseDetailsServiceModel> ByIdAsync(int id)
        //{
        //    return await this.db
        //        .Courses
        //        .Where(c => c.Id == id)
        //        .ProjectTo<CourseDetailsServiceModel>()
        //        .FirstOrDefaultAsync();
        //}


        public async Task<TModel> ByIdAsync<TModel>(int id) where TModel : class
        {
            return await this.db
                .Courses
                .Where(c => c.Id == id)
                .ProjectTo<TModel>()
                .FirstOrDefaultAsync();
        }


        public async Task<bool> IsUserSignedInCourseAsync(int courseId, string userId)
        {
            return await this.db
                .Courses
                .AnyAsync(c => c.Id == courseId && c.Students.Any(s => s.StudentId == userId));
        }

        public async Task<bool> SignUpUserAsync(int courseId, string userId)
        {
            var courseInfo = await GetCourseInfo(courseId, userId);

            if (courseInfo == null || courseInfo.StartDate < DateTime.UtcNow || courseInfo.UserIdSignedIn)
            {
                return false;
            }

            var studentInCourse = new StudentCourse
            {
                CourseId = courseId,
                StudentId = userId
            };

            //var course = await this.db.Courses.FindAsync(courseId);
            //var user = await this.db.Users.FindAsync(userId);
            //if (course == null || user == null)
            //{
            //    return false;
            //}
            //if (await this.IsUserSignedInCourse(courseId, userId))
            //{
            //    return true;
            //}

            this.db.Add(studentInCourse);
            await this.db.SaveChangesAsync();

            return true;
        }


        public async Task<bool> SignOutUserAsync(int courseId, string userId)
        {
            var courseInfo = await GetCourseInfo(courseId, userId);

            if (courseInfo == null || courseInfo.StartDate < DateTime.UtcNow || !courseInfo.UserIdSignedIn)
            {
                return false;
            }

            //// This work
            //var studentInCourse = await this.db
            //    .Courses
            //    .Where(c => c.Id == courseId)
            //    .SelectMany(c => c.Students)
            //    .FirstOrDefaultAsync(s => s.StudentId == userId);

            //// and this work!
            //var studentInCourse = await this.db.FindAsync(typeof(StudentCourse), courseId, userId);

            // and this work 
            var studentInCourse = await this.db.FindAsync<StudentCourse>(courseId, userId);


            this.db.Remove(studentInCourse);
            await this.db.SaveChangesAsync();

            return true;
        }


        public async Task<bool> SaveExamSubmissionAsync(int courseId, string userId, byte[] examSubmission, string fileName)
        {
            if (!await this.IsUserSignedInCourseAsync(courseId,userId))
            {
                return false;
            }

            Exam exam = new Exam
            {
                CourseId = courseId,
                StudentId = userId,
                ExamSubmission = examSubmission,
                ExamFileName = fileName,
                SubmissionTime = DateTime.UtcNow
            };

            this.db.Exams.Add(exam);
            await this.db.SaveChangesAsync();

            return true;
        }


        private async Task<CourseWithSudentInfo> GetCourseInfo(int courseId, string userId)
        {
            return await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => new CourseWithSudentInfo
                {
                    StartDate = c.StartDate,
                    UserIdSignedIn = c.Students.Any(s => s.StudentId == userId)
                })
                .FirstOrDefaultAsync();
        }
    }
}