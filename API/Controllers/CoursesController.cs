using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

using A02.Models.ViewModels;
using A02.Services;
using A02.Services.Exceptions;

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

        // GET api/courses/
        // GET api/courses?semester=20163
        [HttpGet]
        public IActionResult GetCoursesOnSemester(string semester)
        {
            try
            {
                var list = _service.GetCoursesBySemester(semester);
                return new ObjectResult(list);
            }
            catch(AppObjectNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        // GET api/courses/1
        // Should return a more detailed object describing T-514-VEFT, taught in 20153 (see above in test data definition)
        [HttpGet]
        [Route("{id}", Name = "GetCourseById")]
        public IActionResult GetCourseById(int id)
        {
            try
            {
                var course = _service.GetCourseById(id);
                return new OkObjectResult(course);
            }
            catch (AppObjectNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        // PUT api/courses/5
        [HttpPut("{id}")]
        public IActionResult UpdateCourseDates(int id, [FromBody]UpdateCourseViewModel model)
        {
            var sd = model.StartDate;
            var ed = model.EndDate;
            // Since I couldn't get ModelBinding to work for the ModelState.IsValid method I
            // simply used regex here to check if the input string is in the right form :)
            var rgx = new Regex("(^(((0[1-9]|1[0-9]|2[0-8])[\\.](0[1-9]|1[012]))|((29|30|31)[\\.](0[13578]|1[02]))|" +
                                  "((29|30)[\\.](0[4,6,9]|11)))[\\.](19|[2-9][0-9])\\d\\d$)|(^29[\\.]02[\\.](19|[2-9][0-9])" +
                                  "(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)");
            if (sd == null || ed == null || !rgx.IsMatch(sd) || !rgx.IsMatch(ed)) return new BadRequestResult();
            try
            {
                _service.UpdateCourseDates(id, sd, ed);
                return new NoContentResult();
            }
            catch(AppObjectNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        // DELETE api/courses/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            try
            {
                _service.DeleteCourse(id);
                return new NoContentResult();
            }
            catch (AppObjectNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        // GET /api/courses/1/students
        [HttpGet]
        [Route("{id}/students", Name = "GetAllStudentsInCourse")]
        public IActionResult GetAllStudentsInCourse(int id)
        {
            try
            {
                var students = _service.GetAllStudentsInCourse(id);
                return new OkObjectResult(students);
            }
            catch (AppObjectNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        // POST /api/courses/2/students
        [HttpPost]
        [Route("{id}/students", Name = "GetAllStudentsInCourse")]
        public IActionResult AddStudentToACourse(int id, [FromBody]AddStudentToCourseViewModel model)
        {
            var ssn = model.StudentSSN;
            var rgx = new Regex("^\\d{10}$");
            if (ssn == null || !rgx.IsMatch(ssn)) return new BadRequestResult();
            try
            {
                _service.AddStudentToCourse(id, ssn);
                return new NoContentResult();
            }
            catch (AppObjectNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (AppObjectExistsException)
            {
                return new BadRequestResult();
            }
        }
    }
}
