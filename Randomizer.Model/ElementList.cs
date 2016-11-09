using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Model
{
    public class ElementList
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<SimpleElement> List { get; set; }

        public ElementList()
        {
            this.List = new List<SimpleElement>();
        }
    }
}
