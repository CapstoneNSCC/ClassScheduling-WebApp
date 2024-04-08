using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{

  [Table("TblClassroom")]
  public class ClassroomModel
  {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Room number is required.")]
    [Range(1, 999, ErrorMessage = "Room number must be between 1 and 999.")]
    public int RoomNumber { get; set; }

    [Required(ErrorMessage = "Building acronym is required.")]
    [StringLength(5, ErrorMessage = "Building acronym cannot exceed 5 characters.")]
    [RegularExpression("^[A-Z]+$", ErrorMessage = "Course code can only contain uppercase letters")]
    public string BuildingAcronym { get; set; }

    [NotMapped]
    public string CombinedRoom => $"{RoomNumber}-{BuildingAcronym}";

    // Collection navigation properties
    public virtual ICollection<TechRoomModel> TechRooms { get; set; } = new List<TechRoomModel>();

    // Used for handling selected technology IDs in forms
    [NotMapped] // Ensures this property is not mapped to the database
    public List<int> SelectedTechnologyIds { get; set; } = new List<int>();
  }

}