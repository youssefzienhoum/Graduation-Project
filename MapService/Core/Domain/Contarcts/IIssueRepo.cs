using Map.Domain.Entities.ISSUE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Contarcts
{
    public interface IIssueRepo
    {
        Task<Issue> GetByIdAsync(Guid id);

        Task<IEnumerable<Issue>> GetAllAsync();
    }
}
