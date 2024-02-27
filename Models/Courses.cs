using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//added name space to be used with the schedule manager.
namespace ClassScheduling_WebApp.Models
{
  [Table("TblCourses")]
  public class Courses
  {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Course code is required.")]
    [StringLength(10, ErrorMessage = "Course code cannot exceed 10 characters.")]
    public required string Code { get; set; }

    [Required(ErrorMessage = "Course name is required.")]
    [StringLength(50, ErrorMessage = "Course name cannot exceed 50 characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Hours are required.")]
    public int Hours { get; set; }

    [Required(ErrorMessage = "Professor ID is required.")]
    public int IdProfessor { get; set; }

    [Required(ErrorMessage = "Educational Program ID is required.")]
    public int IdEducationalProgram { get; set; }

    [ForeignKey("IdProfessor")]
    public virtual required User Professor { get; set; }

    [ForeignKey("IdEducationalProgram")]
    public virtual required EducationalProgram EducationalProgram { get; set; }
  }
}
