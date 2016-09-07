using System.Collections.Generic;
using A02.Models;

namespace A02.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStudentsService
    {
        List<StudentLiteDTO> GetStudentsInCourse(int id);

        StudentLiteDTO GetStudentBySSN(string SSN);
    }
}
