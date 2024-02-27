using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TblUser")]
public class User
{
  [Key]
  public int Id { get; set; }

  [Required(ErrorMessage = "First name is required.")]
  [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
  public string FirstName { get; set; }

  [Required(ErrorMessage = "Last name is required.")]
  [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
  public string LastName { get; set; }

  [Required]
  public bool SetAsAdmin { get; set; }

  [Required(ErrorMessage = "Username is required.")]
  [StringLength(30, ErrorMessage = "Username cannot exceed 30 characters.")]
  public string UserName { get; set; }

  [Required(ErrorMessage = "Password is required.")]
  public string Password { get; set; }

  public string Salt { get; set; }
}