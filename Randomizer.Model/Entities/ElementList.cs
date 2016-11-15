using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Randomizer.Model
{
    public class ElementList : BaseEntity, IOwnerable
    {
        public ElementList()
        {
            this.Elements = new List<SimpleElement>();
        }

        [ForeignKey("User")]
        public string UserID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual IList<SimpleElement> Elements { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
