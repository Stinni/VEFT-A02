using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using A02.Models;
using A02.Services;

namespace A02.API.Controllers
{
    [Route("api/courses")]
    public class CoursesController
    {
        private readonly ICoursesService _service;

        public CoursesController(ICoursesService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<CourseLiteDTO> GetCoursesOnSemester(string semester = null)
        {
            return _service.GetCoursesBySemester(semester);
            /*return new List<CourseLiteDTO>
            {
                new CourseLiteDTO
                {
                    ID = 1,
                    Name = "Web Services",
                    Semester = "20163"
                }
            };*/
        }
    }
}
