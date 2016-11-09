using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Randomizer.Model
{
    public class SimpleElement : IOwnerable, IRandomElement
    {

        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public int DeletedBy { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ElementList ElementList { get; set; }
    }
}
