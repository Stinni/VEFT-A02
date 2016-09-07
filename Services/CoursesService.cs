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
            var list = (from x in _db.Courses
                        where x.Semester == semester
                        select new CourseLiteDTO
                        {
                            ID = x.ID,
                            Name = x.Name,
                            Semester = x.Semester
                        }).ToList();
            return !list.Any() ? null : list;
        }

        public CourseLiteDTO GetCourseById(int id)
        {
            var course = (from x in _db.Courses
                          where x.ID == id
                          select new CourseLiteDTO
                          {
                              ID = x.ID,
                              Name = x.Name,
                              Semester = x.Semester
                          }).SingleOrDefault();
            return course;
        }
    }
}
