using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{

  [Table("TblCourse")]
  public class CourseModel
  {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Course code is required.")]
    [StringLength(10, ErrorMessage = "Course code cannot exceed 10 characters.")]
    public string Code { get; set; }

    [Required(ErrorMessage = "Course name is required.")]
    [StringLength(50, ErrorMessage = "Course name cannot exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Hours are required.")]
    public int Hours { get; set; }

    [ForeignKey("IdProfessor"), Required(ErrorMessage = "Professor is required.")]
    public int IdProfessor { get; set; }
    public virtual UserModel Professor { get; set; }

    [ForeignKey("IdProgram"), Required(ErrorMessage = "Program is required.")]
    public int IdProgram { get; set; }
    public virtual ProgramModel Programs { get; set; }

    // navigation properties
    public virtual ICollection<TechClassModel> TechClasses { get; set; } = new List<TechClassModel>();

    // used for handling selected technology IDs in forms and making sure its not mapped to the database.
    [NotMapped]
    public List<int> SelectedTechnologyIds { get; set; } = new List<int>();
  }

}