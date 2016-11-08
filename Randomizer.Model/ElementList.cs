using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Model
{
    public class ElementList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<SimpleElement> List { get; set; }
    }
}
