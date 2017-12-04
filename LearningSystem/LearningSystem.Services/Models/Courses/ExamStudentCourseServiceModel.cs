namespace LearningSystem.Services.Models.Courses
{
    using AutoMapper;
    using LearningSystem.Common.Mapping;
    using LearningSystem.Data.Models;
    using System;

    public class ExamStudentCourseServiceModel : IMapFrom<Exam> , IHaveCustomMapping
    {
        public string Username { get; set; }

        public string CourseName { get; set; }

        public Byte[] ExamSubmission { get; set; }

        public string ExamFileName { get; set; }

        public DateTime SubmissionTime { get; set; }


        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Exam, ExamStudentCourseServiceModel>()
                .ForMember(e => e.Username, cfg => cfg.MapFrom(e => e.Student.UserName))
                .ForMember(e => e.CourseName, cfg => cfg.MapFrom(e => e.Course.Name));
        }
    }
}