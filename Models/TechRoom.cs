using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TblTechRoom")]
public class TblTechRoom
{
    [Key, Column(Order = 0), ForeignKey("Room")]
    public int IdRoom { get; set; }

    [Key, Column(Order = 1), ForeignKey("Technology")]
    public int IdTechnology { get; set; }

    public virtual required Classroom Room { get; set; }
    public virtual required Technology Technology { get; set; }
}
