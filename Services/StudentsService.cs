using System.Collections.Generic;
using System.Linq;
using A02.Models;

namespace A02.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentsService : IStudentsService
    {
        private readonly AppDataContext _db;

        public StudentsService(AppDataContext db)
        {
            _db = db;
        }

        // TODO: FIX THIS!!! <- Has to check students in link table and such...
        public List<StudentLiteDTO> GetStudentsInCourse(int id)
        {
            var list = (from x in _db.Students
                        select new StudentLiteDTO
                        {
                            SSN = x.SSN,
                            Name = x.Name
                        }).ToList();
            return !list.Any() ? null : list;
        }

        public StudentLiteDTO GetStudentBySSN(string ssn)
        {
            return null;
        }
    }
}
