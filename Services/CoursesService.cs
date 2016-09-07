using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using A02.Models;
using A02.Services.Exceptions;

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
                        join ct in _db.CourseTemplates on c.TemplateId equals ct.CourseId
                        where c.Semester == semester
                        select new CourseLiteDTO
                        {
                            Id = c.Id,
                            Name = ct.Name,
                            Semester = c.Semester,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate
                        }).ToList();
            if(!list.Any())
            {
                throw new AppObjectNotFoundException();
            }
            return list;
        }

        public CourseLiteDTO GetCourseById(int id)
        {
            var course = (from c in _db.Courses
                          join ct in _db.CourseTemplates on c.TemplateId equals ct.CourseId
                          where c.Id == id
                          select new CourseLiteDTO
                          {
                              Id = c.Id,
                              Name = ct.Name,
                              Semester = c.Semester,
                              StartDate = c.StartDate,
                              EndDate = c.EndDate
                          }).SingleOrDefault();
            if (course == null) throw new AppObjectNotFoundException();
            return course;
        }

        public void UpdateCourseDates(int id, string sDate, string eDate)
        {
            var course = (from c in _db.Courses
                          where c.Id == id
                          select c).SingleOrDefault();

            if (course == null)
            {
                throw new AppObjectNotFoundException();
            }

            course.StartDate = sDate;
            course.EndDate = eDate;
            _db.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = (from c in _db.Courses
                          where c.Id == id
                          select c).SingleOrDefault();
            if(course == null) throw new AppObjectNotFoundException();

            var connections = (from con in _db.StudentConnections
                              where con.CourseId == id
                              select con).DefaultIfEmpty();
            if (connections.Any())
            {
                foreach (var con in connections)
                {
                    _db.StudentConnections.Remove(con);
                }
            }
            _db.Courses.Remove(course);
            _db.SaveChanges();
        }

        public List<StudentLiteDTO> GetAllStudentsInCourse(int id)
        {
            var students = (from s in _db.Students
                            join sc in _db.StudentConnections on s.SSN equals sc.StudentId
                            where sc.CourseId == id
                            select new StudentLiteDTO
                            {
                                Name = s.Name,
                                SSN = s.SSN
                            }).ToList();
            if (!students.Any()) throw new AppObjectNotFoundException();
            return students;
        }

        public void AddStudentToCourse(int cId, string sId)
        {
            
        }

        public void RemoveStudentFromCourse(int cId, string sId)
        {
            var connection = (from con in _db.StudentConnections
                              where con.CourseId == cId && con.StudentId == sId
                              select con).SingleOrDefault();
            if(connection == null) throw new AppObjectNotFoundException();
            _db.StudentConnections.Remove(connection);
            _db.SaveChanges();
        }
    }
}
