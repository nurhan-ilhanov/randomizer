using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Randomizer.Model
{
    public class SimpleElement : BaseEntity, IOwnerable, IRandomElement
    {
        [ForeignKey("User")]
        public string UserID { get; set; }

        [ForeignKey("ElementList")]
        public int ElementListID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ElementList ElementList { get; set; }
    }
}
