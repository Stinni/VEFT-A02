namespace A02.Models
{
    /// <summary>
    /// Used to store info for updating a course's start and end dates
    /// 
    /// Note on validation:
    /// Required and other types of ModelBinding didn't work, some problems with dependencies
    /// and/or frameworks so I ended up scrapping that and validating input myself :)
    /// </summary>
    public class UpdateCourseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndDate { get; set; }
    }
}
