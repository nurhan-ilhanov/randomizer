using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Model
{
    public interface IRandomElement
    {
        int ID { get; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
