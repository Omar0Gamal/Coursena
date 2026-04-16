using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Infrastrcuter.Identity
{
    public interface IIdentitySeeder
    {
        Task SeedRolesAsync();
        Task SeedAdminAsync();
    }
}
