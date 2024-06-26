using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClassScheduling_WebApp.Models
{
  [Table("TblProgram")]
  public class ProgramModel
  {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Program name is required.")]
    [StringLength(60, ErrorMessage = "Program name cannot exceed 60 characters.")]
    [RegularExpression(@"^[a-zA-Z]+(?:\s[a-zA-Z]+)*$", ErrorMessage = "Program name can only contain letters and must have at least one letter.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Year is required.")]
    [Range(1, 2, ErrorMessage = "Year must be either 1 (First Year) or 2 (Second Year).")]
    public int Year { get; set; }

    // navigation properties
    public virtual ICollection<CourseModel> Courses { get; set; } = new List<CourseModel>();



  }

}
