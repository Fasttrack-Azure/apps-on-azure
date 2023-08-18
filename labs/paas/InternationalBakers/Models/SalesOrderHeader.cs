using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("SalesOrderHeader", Schema = "SalesLT")]
    [Index("SalesOrderNumber", Name = "AK_SalesOrderHeader_SalesOrderNumber", IsUnique = true)]
    [Index("Rowguid", Name = "AK_SalesOrderHeader_rowguid", IsUnique = true)]
    [Index("CustomerId", Name = "IX_SalesOrderHeader_CustomerID")]
    public partial class SalesOrderHeader
    {
        public SalesOrderHeader()
        {
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        [Key]
        [Column("SalesOrderID")]
        public int SalesOrderId { get; set; }
        public byte RevisionNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DueDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShipDate { get; set; }
        public byte Status { get; set; }
        [Required]
        public bool? OnlineOrderFlag { get; set; }
        [StringLength(25)]
        public string SalesOrderNumber { get; set; } = null!;
        [StringLength(25)]
        public string? PurchaseOrderNumber { get; set; }
        [StringLength(15)]
        public string? AccountNumber { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("ShipToAddressID")]
        public int? ShipToAddressId { get; set; }
        [Column("BillToAddressID")]
        public int? BillToAddressId { get; set; }
        [StringLength(50)]
        public string ShipMethod { get; set; } = null!;
        [StringLength(15)]
        [Unicode(false)]
        public string? CreditCardApprovalCode { get; set; }
        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }
        [Column(TypeName = "money")]
        public decimal TaxAmt { get; set; }
        [Column(TypeName = "money")]
        public decimal Freight { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalDue { get; set; }
        public string? Comment { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("BillToAddressId")]
        [InverseProperty("SalesOrderHeaderBillToAddresses")]
        public virtual Address? BillToAddress { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("SalesOrderHeaders")]
        public virtual Customer Customer { get; set; } = null!;
        [ForeignKey("ShipToAddressId")]
        [InverseProperty("SalesOrderHeaderShipToAddresses")]
        public virtual Address? ShipToAddress { get; set; }
        [InverseProperty("SalesOrder")]
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
