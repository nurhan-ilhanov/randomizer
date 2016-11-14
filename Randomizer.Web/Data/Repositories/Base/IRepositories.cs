using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Web.Data.Repositories
{
    public interface IElementsRepository : IRepository<SimpleElement> { }
    public interface IElementListsRepository : IRepository<ElementList> { }
}
