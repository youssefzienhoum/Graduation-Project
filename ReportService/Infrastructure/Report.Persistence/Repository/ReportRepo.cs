using Microsoft.EntityFrameworkCore;
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
    public class ReportRepo(IssueDbContext issueDb) : IReportRepo
    {
        public async Task<Issue> AddAsync(Issue issue)
        {
            await issueDb.issues.AddAsync(issue);
           
            return issue;
        }

        public async Task DeleteAsync(Guid id)
        {
            var report = await GetByIdAsync(id);
            if (report is null)
                throw new Exception("Report not found");

            issueDb.issues.Remove(report);

        }

        public async Task<IEnumerable<Issue>> GetAllAsync()
        {

           return await Query().ToListAsync();
        }

        public async Task<Issue> GetByIdAsync(Guid id)
        {
            return await Query().FirstOrDefaultAsync(r => r.Id == id);

        }

        public async Task<IEnumerable<Issue>> GetByReporterIdAsync(Guid reporterId)
        {

            return await Query().Where(r => r.ReporterId == reporterId).ToListAsync();


        }

      

        public async Task UpdateAsync(Issue issue)
        {
            issueDb.issues.Update(issue);
           
            
        }
        private IQueryable<Issue> Query()
        {
            return issueDb.issues
                .Include(r => r.LocationId)
                .Include(r => r.reportAttachments)
                .Include(r => r.aiAnalyses);
        }
    }

}

