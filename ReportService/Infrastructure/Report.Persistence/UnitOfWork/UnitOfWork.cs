using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;
using Report.Persistence.Context;
using Report.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.UnitOfWork
{
    public class Unitofwork(IssueDbContext issueDb) : IUnitOfWork
    {
        public IReportRepo ReportRepo { get; }= new ReportRepo(issueDb);

        public IReportAttachmentRepo ReportAttachmentRepo { get; } = new ReportAttachmentRepo(issueDb);

    

        public void Dispose()
        {
           issueDb.Dispose();
        }

        public async Task<int> SaveChangesAsync(
       CancellationToken cancellationToken = default)
        {
            return await issueDb.SaveChangesAsync(cancellationToken);
        }
    }
}
