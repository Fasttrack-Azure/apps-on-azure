using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("CustomerAddress", Schema = "SalesLT")]
    [Index("Rowguid", Name = "AK_CustomerAddress_rowguid", IsUnique = true)]
    public partial class CustomerAddress
    {
        [Key]
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Key]
        [Column("AddressID")]
        public int AddressId { get; set; }
        [StringLength(50)]
        public string AddressType { get; set; } = null!;
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("AddressId")]
        [InverseProperty("CustomerAddresses")]
        public virtual Address Address { get; set; } = null!;
        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerAddresses")]
        public virtual Customer Customer { get; set; } = null!;
    }
}
