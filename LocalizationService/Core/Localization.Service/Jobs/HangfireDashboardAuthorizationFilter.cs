using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.ServiceAbstraction.Jobs
{
    public class HangfireDashboardAuthorizationFilter(string role = "Admin") : Hangfire.Dashboard.IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var user = context.GetHttpContext().User;
            return user.Identity?.IsAuthenticated == true && user.IsInRole(role);
        }
    }
}
