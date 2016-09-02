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
    }
}
