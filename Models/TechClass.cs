using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TblTechClass")]
public class TblTechClass
{
    [Key, Column(Order = 0), ForeignKey("Course")]
    public int IdCourse { get; set; }

    [Key, Column(Order = 1), ForeignKey("Technology")]
    public int IdTechnology { get; set; }

    public virtual required Courses Course { get; set; }
    public virtual required Technology Technology { get; set; }
}
