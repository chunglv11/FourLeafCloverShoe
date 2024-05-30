using FourLeafCloverShoe.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FourLeafCloverShoe.Share.ViewModels
{
    public class UserViewModel : User
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Display(Name = "Role")]
        public string SelectedRole { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
