using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("SalesOrderDetail", Schema = "SalesLT")]
    [Index("Rowguid", Name = "AK_SalesOrderDetail_rowguid", IsUnique = true)]
    [Index("ProductId", Name = "IX_SalesOrderDetail_ProductID")]
    public partial class SalesOrderDetail
    {
        [Key]
        [Column("SalesOrderID")]
        public int SalesOrderId { get; set; }
        [Key]
        [Column("SalesOrderDetailID")]
        public int SalesOrderDetailId { get; set; }
        public short OrderQty { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPriceDiscount { get; set; }
        [Column(TypeName = "numeric(38, 6)")]
        public decimal LineTotal { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("SalesOrderDetails")]
        public virtual Product Product { get; set; } = null!;
        [ForeignKey("SalesOrderId")]
        [InverseProperty("SalesOrderDetails")]
        public virtual SalesOrderHeader SalesOrder { get; set; } = null!;
    }
}
