namespace LearningSystem.Services.Admin
{
    using LearningSystem.Services.Admin.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAdminCourseService
    {
        Task<IEnumerable<AdminCourseBasicListingServiceModel>> AllAsync();

        Task CreateAsync(string name, string description, string trainerId, DateTime startDate, DateTime endDate);
    }
}