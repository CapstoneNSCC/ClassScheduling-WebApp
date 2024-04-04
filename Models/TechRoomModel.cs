using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{
  [Table("TblTechRoom")]
  public class TechRoomModel
  {
    [ForeignKey("Room")]
    public int IdRoom { get; set; }

    [ForeignKey("Technology")]
    public int IdTechnology { get; set; }

    // Navigation properties
    [Required(ErrorMessage = "Room is required.")]
    public virtual ClassroomModel Room { get; set; }

    [Required(ErrorMessage = "Technology is required.")]
    public virtual TechnologyModel Technology { get; set; }
  }
}
