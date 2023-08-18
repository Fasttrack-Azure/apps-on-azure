using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    public partial class OrderLine
    {
        [Key]
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public int? CookieId { get; set; }
        public int? OrderId { get; set; }

        [ForeignKey("CookieId")]
        [InverseProperty("OrderLines")]
        public virtual Cookie? Cookie { get; set; }
        [ForeignKey("OrderId")]
        [InverseProperty("OrderLines")]
        public virtual Order? Order { get; set; }
    }
}
