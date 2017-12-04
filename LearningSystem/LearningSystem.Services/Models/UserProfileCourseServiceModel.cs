namespace LearningSystem.Services.Models
{
    using AutoMapper;
    using LearningSystem.Common.Mapping;
    using LearningSystem.Data.Models;
    using System.Linq;

    public class UserProfileCourseServiceModel : IMapFrom<Course>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Grade? Grade { get; set; }


        public void ConfigureMapping(Profile mapper)
        {
            string userId = null;

            mapper
                .CreateMap<Course, UserProfileCourseServiceModel>()
                .ForMember(p => p.Grade, cfg => cfg
                    .MapFrom(c => c.Students
                        .Where(s => s.StudentId == userId)
                        .Select(s => s.Grade)
                        .FirstOrDefault()));
        }
    }
}