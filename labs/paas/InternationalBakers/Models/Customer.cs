using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("Customer", Schema = "SalesLT")]
    [Index("Rowguid", Name = "AK_Customer_rowguid", IsUnique = true)]
    [Index("EmailAddress", Name = "IX_Customer_EmailAddress")]
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
        }

        [Key]
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        public bool NameStyle { get; set; }
        [StringLength(8)]
        public string? Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        public string? MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; } = null!;
        [StringLength(10)]
        public string? Suffix { get; set; }
        [StringLength(128)]
        public string? CompanyName { get; set; }
        [StringLength(256)]
        public string? SalesPerson { get; set; }
        [StringLength(50)]
        public string? EmailAddress { get; set; }
        [StringLength(25)]
        public string? Phone { get; set; }
        [StringLength(128)]
        [Unicode(false)]
        public string PasswordHash { get; set; } = null!;
        [StringLength(10)]
        [Unicode(false)]
        public string PasswordSalt { get; set; } = null!;
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
