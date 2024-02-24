using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{
    [Table("TblTechnology")]
    public class TechnologyModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Technology description is required.")]
        [StringLength(20, ErrorMessage = "Technology description cannot exceed 20 characters.")]
        public string Description { get; set; }

        // Propriedades de navegação para relações muitos-para-muitos
        public virtual ICollection<TechClassModel> TechClasses { get; set; } = new List<TechClassModel>();
    }

}