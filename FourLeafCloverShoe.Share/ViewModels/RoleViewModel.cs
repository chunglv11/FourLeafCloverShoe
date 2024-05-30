using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class RoleViewModels : IdentityRole
    {
        public IEnumerable<string> Claims { get; set; }
    }
}
