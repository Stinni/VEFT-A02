using System.Collections.Generic;
using A02.Models;

namespace A02.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICoursesService
    {
        List<CourseLiteDTO> GetCoursesBySemester(string semester);

        CourseLiteDTO GetCourseById(int id);

        void UpdateCourseDates(int id, string sDate, string eDate);

        void DeleteCourse(int id);

        List<StudentLiteDTO> GetAllStudentsInCourse(int id);

        void AddStudentToCourse(int cId, string sId);

        void RemoveStudentFromCourse(int cId, string sId);
    }
}
