using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Model
{
    public interface IOwnerable
    {
        int UserID { get; set; }
        int CreatedBy { get; set; }
        int ModifiedBy { get; set; }
        int DeletedBy { get; set; }
    }
}
