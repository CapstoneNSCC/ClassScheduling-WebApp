using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{
  [Table("TblTechRoom")]
  public class TechRoomModel
  {
    [ForeignKey("Classroom")]
    public int IdClassroom { get; set; }

    [ForeignKey("Technology")]
    public int IdTechnology { get; set; }

    // Navigation properties
    [Required(ErrorMessage = "Room is required.")]
    public virtual ClassroomModel Classroom { get; set; }

    [Required(ErrorMessage = "Technology is required.")]
    public virtual TechnologyModel Technology { get; set; }
  }
}