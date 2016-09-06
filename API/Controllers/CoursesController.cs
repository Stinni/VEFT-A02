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

        // GET api/courses/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/courses
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/courses/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/courses/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
