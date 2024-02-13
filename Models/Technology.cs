using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TblTechnology")]
public class Technology
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Technology description is required.")]
    [StringLength(20, ErrorMessage = "Technology description cannot exceed 20 characters.")]
    public required string Description { get; set; }
}
