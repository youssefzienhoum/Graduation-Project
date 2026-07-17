using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Context
{
    public  class ReportDbContext(DbContextOptions<ReportDbContext> options) : DbContext (options)
    {
     



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly
               (Assembly.GetExecutingAssembly());
        }


    }
}
