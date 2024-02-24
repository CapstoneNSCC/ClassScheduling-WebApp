using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{

    [Table("TblTechClass")]
    public class TechClassModel
    {
        [ForeignKey("Course")]
        public int IdCourse { get; set; }

        [ForeignKey("Technology")]
        public int IdTechnology { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        public virtual CourseModel Course { get; set; }

        [Required(ErrorMessage = "Technology is required.")]
        public virtual TechnologyModel Technology { get; set; }
    }

}
