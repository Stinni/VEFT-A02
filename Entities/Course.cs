﻿namespace A02.Entities
{
    public class Course
    {
        /// <summary>
        /// Primary key - The course's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the CourseTemplates table
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// String for which semester the course is being taught
        /// Example: 20163 - the year and then 3 for the autumn semester
        /// </summary>
        public string Semester { get; set; }
    }
}
