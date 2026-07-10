using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    public enum UserStatus
    {
        Pending,
        Approved,
        Rejected,
        Suspended
    }
}
