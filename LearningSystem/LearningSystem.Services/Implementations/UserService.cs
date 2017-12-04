namespace LearningSystem.Services.Implementations
{
    using System;
    using AutoMapper.QueryableExtensions;
    using LearningSystem.Data;
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static ServiceConstants;

    class UserService : IUserService
    {
        private readonly LearningSystemDbContext db;
        private readonly IPdfGenerator pdfs;

        public UserService(
            LearningSystemDbContext db, 
            IPdfGenerator pdfs)
        {
            this.db = db;
            this.pdfs = pdfs;
        }


        public async Task<UserProfileServiceModel> ProfileAsync(string id)
        {
            return await this.db
                .Users
                .Where(u => u.Id == id)
                .ProjectTo<UserProfileServiceModel>(new { userId = id })
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<UserListingServiceModel>> FindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;
            return await this.db
                .Users
                .OrderBy(u => u.UserName)
                .Where(u => u.UserName.ToLower().Contains(searchText.ToLower())
                         || u.Name.ToLower().Contains(searchText.ToLower()))
                .ProjectTo<UserListingServiceModel>()
                .ToListAsync();
        }


        public async Task<byte[]> GetPdfCertificateAsync(int courseId, string studentId)
        {
            var studentInCourse = await this.db.FindAsync<StudentCourse>(courseId, studentId);

            if (studentInCourse == null)
            {
                return null;
            }

            var certificateInfo = await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => new
                {
                    CourseName = c.Name,
                    CourseStartDate = c.StartDate,
                    CourseEndDate = c.EndDate,
                    TrainerName = c.Trainer.Name,
                    StudentName = c.Students
                        .Where(s=>s.StudentId==studentId)
                        .Select(s=>s.Student.Name)
                        .FirstOrDefault(),
                    StudentGrade = c.Students
                        .Where(s => s.StudentId == studentId)
                        .Select(s => s.Grade)
                        .FirstOrDefault(),
                })
                .FirstOrDefaultAsync();

            return this.pdfs.GeneratePdfFromHtml(String.Format(
                PdfCertificateFormat,
                certificateInfo.CourseName,
                certificateInfo.CourseStartDate.ToShortDateString(),
                certificateInfo.CourseEndDate.Date.ToShortDateString(),
                certificateInfo.StudentName,
                certificateInfo.StudentGrade.ToString(),
                certificateInfo.TrainerName,
                DateTime.UtcNow.Date.ToShortDateString()));
        }
    }
}
