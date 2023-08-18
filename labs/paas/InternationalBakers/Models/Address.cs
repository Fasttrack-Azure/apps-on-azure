using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("Address", Schema = "SalesLT")]
    [Index("Rowguid", Name = "AK_Address_rowguid", IsUnique = true)]
    [Index("AddressLine1", "AddressLine2", "City", "StateProvince", "PostalCode", "CountryRegion", Name = "IX_Address_AddressLine1_AddressLine2_City_StateProvince_PostalCode_CountryRegion")]
    [Index("StateProvince", Name = "IX_Address_StateProvince")]
    public partial class Address
    {
        public Address()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            SalesOrderHeaderBillToAddresses = new HashSet<SalesOrderHeader>();
            SalesOrderHeaderShipToAddresses = new HashSet<SalesOrderHeader>();
        }

        [Key]
        [Column("AddressID")]
        public int AddressId { get; set; }
        [StringLength(60)]
        public string AddressLine1 { get; set; } = null!;
        [StringLength(60)]
        public string? AddressLine2 { get; set; }
        [StringLength(30)]
        public string City { get; set; } = null!;
        [StringLength(50)]
        public string StateProvince { get; set; } = null!;
        [StringLength(50)]
        public string CountryRegion { get; set; } = null!;
        [StringLength(15)]
        public string PostalCode { get; set; } = null!;
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [InverseProperty("Address")]
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        [InverseProperty("BillToAddress")]
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaderBillToAddresses { get; set; }
        [InverseProperty("ShipToAddress")]
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaderShipToAddresses { get; set; }
    }
}
