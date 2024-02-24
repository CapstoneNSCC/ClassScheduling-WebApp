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
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Building acronym is required.")]
        [StringLength(5, ErrorMessage = "Building acronym cannot exceed 5 characters.")]
        public string BuildingAcronym { get; set; }

        [NotMapped]
        public string CombinedRoom => $"{RoomNumber}{BuildingAcronym}";
    }
    
}