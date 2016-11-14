using Randomizer.Model;
using Randomizer.Web.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Web.Data.Repositories
{
    public class ElementListsRepository : Repository<ElementList>, IElementListsRepository
    {
        public ElementListsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
