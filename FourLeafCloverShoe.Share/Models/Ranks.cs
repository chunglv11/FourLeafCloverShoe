using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourLeafCloverShoe.Share.Models
{
    public class Ranks
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? PointsMin { get; set; }
        public int? PoinsMax { get; set; }
        public string? Description { get; set; }
        public virtual List<User>? Users { get; set; }
    }
}
