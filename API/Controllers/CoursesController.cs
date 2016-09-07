using Microsoft.AspNetCore.Mvc;
using A02.Services;

namespace A02.API.Controllers
{
    [Route("api/courses")]
    public class CoursesController
    {
        private readonly ICoursesService _service;
        private readonly IStudentsService _sservice;

        public CoursesController(ICoursesService service, IStudentsService sservice)
        {
            _service = service;
            _sservice = sservice;
        }

        // GET api/courses/
        // GET api/courses?semester=20163
        [HttpGet]
        public IActionResult GetCoursesOnSemester(string semester)
        {
            var list = _service.GetCoursesBySemester(semester);
            if (list == null)
            {
                return new NotFoundResult();
            }
            return new ObjectResult(list);
        }

        // GET api/courses/1
        // Should return a more detailed object describing T-514-VEFT, taught in 20153 (see above in test data definition)
        [HttpGet]
        [Route("{id}", Name = "GetCourseById")]
        public IActionResult GetCourseById(int id)
        {
            var course = _service.GetCourseById(id);
            if (course == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(course);
        }

        // TODO: DELETE THIS!!! <- Just messing and trying it out :)
        [HttpGet]
        [Route("students/", Name = "GetStudents")]
        public IActionResult GetStudents()
        {
            var list = _sservice.GetStudentsInCourse(1);
            if (list == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(list);
        }
        // PUT api/courses/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // POST api/courses/5
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // DELETE api/courses/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/

        /*
         *  /api/courses/1 - PUT
         *  Should allow the client of the API to modify the given course instance. The properties which should be mutable are StartDate and EndDate, others (CourseID and Semester) should be immutable.
         *  
         *  /api/courses/999 - PUT
         *  Should return 404
         *  
         *  /api/courses/1 - DELETE
         *  Should remove the given course
         *  
         *  /api/courses/999 - DELETE
         *  Should return 404
         *  
         *  /api/courses/1/students - GET
         *  Should return a list of all students in T-514-VEFT in fall 2015
         *  
         *  /api/courses/2/students - POST
         *  Should add a new student to T-514-VEFT in 20163. It is assumed that the request body contains the 
         */
    }
}
