using Coursna.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IJwtService
    {
        Task<string> CreateJwtToken(ApplicationUser user);
    }
}
