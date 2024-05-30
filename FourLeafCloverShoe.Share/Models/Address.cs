using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        
        public string? RecipientName { get; set; }
        public string? RecipientPhone { get; set; }
        public int? ProvinceID { get; set; } //Mã tỉnh thành
        public string? ProvinceName { get; set; }
        public int? DistrictID { get; set; } //Mã Quận/Huyện.
        public string? DistrictName { get; set; } // thành phố
       public int? WardCode { get; set; }// Mã Phường/Xã.
        public string? WardName { get; set; } //Tên Phường/Xã.

        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        [ForeignKey("UserId")]
        public virtual User? Users { get; set; }
    }
}
