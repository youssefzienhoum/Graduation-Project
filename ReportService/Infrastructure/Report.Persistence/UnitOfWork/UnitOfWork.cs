using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;
using Report.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.UnitOfWork
{
    public class Unitofwork(ReportDbContext reportDb) : IUnitOfWork
    {
        public IReportRepo ReportRepo { get; }

        public IReportAttachmentRepo ReportAttachmentRepo { get; }

        public void Dispose()
        {
           reportDb.Dispose();
        }

        public async Task<int> SaveChangesAsync(
       CancellationToken cancellationToken = default)
        {
            return await reportDb.SaveChangesAsync(cancellationToken);
        }
    }
}
