using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Model
{
    public class RandomElement : IRandomElement
    {
        private int _id;
        public int ID {
            get
            {
                return this._id;
            }
        }

        public RandomElement(int id)
        {
            this._id = id;
        }
    }
}
