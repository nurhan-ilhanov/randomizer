using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Core.Tests
{
    public class TestElement : IRandomElement
    {
        private int _id;
        public int ID
        {
            get
            {
                return this._id;
            }
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public TestElement(int id)
        {
            this._id = id;
        }
    }
}
