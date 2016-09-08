namespace A02.Models.ViewModels
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
        /// The course's start date
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// The course's end date
        /// </summary>
        public string EndDate { get; set; }
    }
}
