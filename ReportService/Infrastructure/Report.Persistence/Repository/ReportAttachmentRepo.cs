
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;

using Report.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Repository
{
    public class ReportAttachmentRepo(IssueDbContext issueDbContext) : IReportAttachmentRepo
    {
        public async Task AddAsync(ReportAttachment attachment)
        {
            await issueDbContext.AddAsync(attachment);
        }

        public async Task Delete(Guid id)
        {
            var attachment = await GetByIdAsync(id ) ;
            if (attachment is null)
                throw new Exception("Attachment not found");
            issueDbContext.ReportAttachments.Remove(attachment);

        }

        public async Task<ReportAttachment?> GetByIdAsync(Guid id)
        {
            return await issueDbContext.ReportAttachments.FindAsync(id);
        }

        public Task<IEnumerable<ReportAttachment>> GetByReportIdAsync(Guid issueid)
        {
            return Task.FromResult<IEnumerable<ReportAttachment>>(issueDbContext.ReportAttachments.Where(a => a.IssueId == issueid ).ToList());
        }

      
    }
}
