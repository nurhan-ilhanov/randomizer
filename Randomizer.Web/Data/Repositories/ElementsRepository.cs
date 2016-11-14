using Randomizer.Model;
using Randomizer.Web.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Web.Data.Repositories
{
    public class ElementsRepository : Repository<SimpleElement>, IElementsRepository
    {
        public ElementsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
