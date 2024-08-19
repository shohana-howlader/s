using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Pactice15_8_24.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Experience> Experiences { get; set; }
    }
   
   
}
   
