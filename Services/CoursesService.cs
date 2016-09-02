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
            return (from x in _db.Courses
                    select new CourseLiteDTO
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Semester = x.Semester
                    }).ToList();
        }
    }
}
