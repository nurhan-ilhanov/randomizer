using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Web.Models
{
    public class ListElementViewModel
    {
        public int ElementID { get; set; }
        public string ElementName { get; set; }
        public bool InList { get; set; }
    }
}
