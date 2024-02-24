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
        public virtual ClassroomModel Room { get; set; }
        public virtual TechnologyModel Technology { get; set; }
    }
}

