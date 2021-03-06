﻿using System;

namespace A02.Models
{
    /// <summary>
    /// A DTO class for sending info about a student to the api
    /// </summary>
    public class StudentLiteDTO
    {
        /// <summary>
        /// The student's SSN
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// The student's name
        /// </summary>
        public string Name { get; set; }
    }
}
