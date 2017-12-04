namespace LearningSystem.Services.Models
{
    using AutoMapper;
    using LearningSystem.Common.Mapping;
    using LearningSystem.Data.Models;

    public class UserListingServiceModel : IMapFrom<User>, IHaveCustomMapping
    {
        public string Username { get; set; }

        public string Name { get; set; }

        public int CoursesCount { get; set; }


        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<User, UserListingServiceModel>()
                .ForMember(u => u.CoursesCount, cfg => cfg.MapFrom(u => u.Courses.Count));
        }

    }
}