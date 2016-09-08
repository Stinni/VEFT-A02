using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

using A02.Models.ViewModels;
using A02.Services;
using A02.Services.Exceptions;

namespace A02.API.Controllers
{
    /// <summary>
    /// The controller for "api/courses" route
    /// Used for anything related to the Courses.db database
    /// Getting data about courses, students in courses, adding students to courses
    /// and even deleting courses
    /// </summary>
    [Route("api/courses")]
    public class CoursesController
    {
        private readonly ICoursesService _service;

        /// <summary>
        /// The CoursesController Constructor
        /// </summary>
        /// <param name="service">The service being used for database access</param>
        public CoursesController(ICoursesService service)
        {
            _service = service;
        }

        /// <summary>
        /// GET method for the "api/courses/" and "api/courses?semester={semester}" routes
        /// Sends a list of all courses being taught at a certain semester
        /// </summary>
        /// <param name="semester">Optional search paramater, if empty, 20163 is used</param>
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

        /// <summary>
        /// GET method for the "api/courses/{id}" route
        /// Sends detailed info about a course with 'id' as it's Id
        /// </summary>
        /// <param name="id">The Id of the course being enquired about</param>
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

        /// <summary>
        /// PUT method for the "api/courses/{id}" route
        /// Updates the StartDate and EndDate for the course with 'id' as it's Id
        /// </summary>
        /// <param name="id">The Id of the course being updated</param>
        /// <param name="model">Model with two attributes, StartDate and EndDate as strings</param>
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

        /// <summary>
        /// DELETE method for the "api/courses/{id}" route
        /// Deletes all connections/enrollments of students to a course with
        /// 'id' as it's Id and then deletes the course itself
        /// </summary>
        /// <param name="id">The Id of the course being deleted</param>
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

        /// <summary>
        /// GET method for the "/api/courses/{id}/students" route
        /// Sends a list of all students in a course with 'id' as it's Id
        /// </summary>
        /// <param name="id">The Id of the course being enquired about</param>
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

        /// <summary>
        /// POST method for the "/api/courses/{id}/students" route
        /// Checks if the Student's SSN is valid and adds that student to
        /// a course with 'id' as it's Id
        /// </summary>
        /// <param name="id">The Id of the course that the student's enrolled in</param>
        /// <param name="model">Model with one attribute, the student's SSN named StudentSSN</param>
        [HttpPost]
        [Route("{id}/students", Name = "GetAllStudentsInCourse")]
        public IActionResult AddStudentToACourse(int id, [FromBody]AddStudentToCourseViewModel model)
        {
            var ssn = model.StudentSSN;
            // Same idea here with the regex as in the 'UpdateCourseDates' function
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
