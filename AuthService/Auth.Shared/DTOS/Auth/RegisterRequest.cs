using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Shared.DTOS.Auth
{
    public  record RegisterRequest(
       string FullName,
       string village,
       IFormFile? picture,
       string Region,
       string email,
       string password,
       string PhoneNumber
        )
    {
    }
}
