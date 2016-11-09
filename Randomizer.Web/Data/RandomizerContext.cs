using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Web.Data
{
    public class RandomizerContext : IdentityDbContext<ApplicationUser>
    {
        public RandomizerContext(DbContextOptions<RandomizerContext> options)
            : base(options)
        {
        }

        public DbSet<SimpleElement> Elements { get; set; }
        public DbSet<ElementList> ElementLists { get; set; }
    }
}
