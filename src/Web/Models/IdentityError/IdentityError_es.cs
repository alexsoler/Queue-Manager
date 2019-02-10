using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.IdentityError
{
    public class IdentityError_es : IdentityErrorDescriber
    {
        public override Microsoft.AspNetCore.Identity.IdentityError DuplicateUserName(string userName)
        {
            return new Microsoft.AspNetCore.Identity.IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "Este usuario ya se encuentra registrado en la base de datos"
            };
        }
    }
}
