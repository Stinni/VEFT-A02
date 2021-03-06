﻿using System.ComponentModel.DataAnnotations;

namespace A02.Models.ViewModels
{
    /// <summary>
    /// A ViewModel class for adding a student to a course
    /// </summary>
    public class AddStudentToCourseViewModel
    {
        /// <summary>
        /// The student's SSN
        /// </summary>
        [RegularExpression("^\\d{10}$")]
        public string StudentSSN { get; set; }
    }
}
