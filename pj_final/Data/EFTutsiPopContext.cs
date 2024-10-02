using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pj_final.Models;

namespace pj_final.Data
{
    public class EFTutsiPopContext : DbContext
    {
        public EFTutsiPopContext (DbContextOptions<EFTutsiPopContext> options)
            : base(options)
        {
        }

        public DbSet<pj_final.Models.users> users { get; set; } = default!;
    }
}
