using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{
    [Table("TblProgram")]
    public class ProgramModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Program name is required.")]
        [StringLength(30, ErrorMessage = "Program name cannot exceed 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(1, 2, ErrorMessage = "Year must be either 1 (First Year) or 2 (Second Year).")]
        public int Year { get; set; }

        // Collection navigation properties
        public virtual ICollection<CourseModel> Courses { get; set; } = new List<CourseModel>();
    }

}