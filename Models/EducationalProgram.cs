using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TblEducationalProgram")]
public class EducationalProgram
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Program name is required.")]
    [StringLength(30, ErrorMessage = "Program name cannot exceed 30 characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Year is required.")]
    [Range(1, 2, ErrorMessage = "Year must be either 1 (First Year) or 2 (Second Year).")]
    public int Year { get; set; }
}
