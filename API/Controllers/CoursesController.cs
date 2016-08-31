using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using A02.Models;

namespace A02.API.Controllers
{
    [Route("api/courses")]
    public class CoursesController
    {
        [HttpGet]
        public List<CourseLiteDTO> GetCoursesOnSemester(string semester = null)
        {
            return new List<CourseLiteDTO>
            {
                new CourseLiteDTO
                {
                    ID = 1,
                    Name = "Web Services",
                    Semester = "20163"
                }
            };
        }
    }
}
