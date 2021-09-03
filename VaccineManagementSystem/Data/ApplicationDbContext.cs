using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaccineManagementSystem.Models;

namespace VaccineManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<VaccineManagementSystem.Models.Vaccine> Vaccine { get; set; }
        public DbSet<VaccineManagementSystem.Models.Patient> Patient { get; set; }
    }
}
