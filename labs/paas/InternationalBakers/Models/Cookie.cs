using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    public partial class Cookie
    {
        public Cookie()
        {
            OrderLines = new HashSet<OrderLine>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Name { get; set; }
        [StringLength(150)]
        public string? ImageUrl { get; set; }
        public double? Price { get; set; }

        [InverseProperty("Cookie")]
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
