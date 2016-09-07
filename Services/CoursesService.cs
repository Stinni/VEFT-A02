using System.Collections.Generic;
using System.Linq;
using A02.Models;

namespace A02.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class CoursesService : ICoursesService
    {
        private readonly AppDataContext _db;

        public CoursesService(AppDataContext db)
        {
            _db = db;
        }

        public List<CourseLiteDTO> GetCoursesBySemester(string semester)
        {
            if (string.IsNullOrEmpty(semester))
            {
                semester = "20163"; // Current semester hardcoded
            }

            var list = (from c in _db.Courses
                        join ct in _db.CourseTemplates
                        on c.TemplateId equals ct.CourseId
                        where c.Semester == semester
                        select new CourseLiteDTO
                        {
                            Id = c.Id,
                            Name = ct.Name,
                            Semester = c.Semester
                        }).ToList();
            return !list.Any() ? null : list;
        }

        public CourseLiteDTO GetCourseById(int id)
        {
            var course = (from c in _db.Courses
                          join ct in _db.CourseTemplates
                          on c.TemplateId equals ct.CourseId
                          where c.Id == id
                          select new CourseLiteDTO
                          {
                              Id = c.Id,
                              Name = ct.Name,
                              Semester = c.Semester
                          }).SingleOrDefault();
            return course;
        }
    }
}
