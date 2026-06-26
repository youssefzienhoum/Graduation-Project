using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.shared.DTOS
{
    public record UserDetailsRespones(
        Guid Id,
        string FulltName,
        string pictures,
        string village,
        string Region
        )
    {
    }
}
