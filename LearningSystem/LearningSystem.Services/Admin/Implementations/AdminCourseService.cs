namespace LearningSystem.Services.Admin.Implementations
{
    using LearningSystem.Data;
    using LearningSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using LearningSystem.Services.Admin.Models;
    using Microsoft.EntityFrameworkCore;

    public class AdminCourseService : IAdminCourseService
    {
        private readonly LearningSystemDbContext db;

        public AdminCourseService(LearningSystemDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<AdminCourseBasicListingServiceModel>> AllAsync()
        {
            return await this.db
                .Courses
                .ProjectTo<AdminCourseBasicListingServiceModel>()
                .ToListAsync();
        }


        public async Task CreateAsync(string name, string description, string trainerId, DateTime startDate, DateTime endDate)
        {
            Course course = new Course
            {
                Name = name,
                Description = description,
                TrainerId = trainerId,
                StartDate = startDate,
                EndDate = endDate
            };

            this.db.Courses.Add(course);
            await this.db.SaveChangesAsync();
        }
    }
}