using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class User : IdentityUser
    {
        public Guid RankId { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Points { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public decimal? Coins { get; set; }
        [ForeignKey("RankId")]
        public virtual Ranks? Ranks { get; set; }
        public virtual List<Address>? Address { get; set; }
        public virtual List<UserVoucher>? VoucherUsers { get; set; }
        public virtual List<Post>? Posts { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}