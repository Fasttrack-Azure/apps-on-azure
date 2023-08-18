using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("ProductModelProductDescription", Schema = "SalesLT")]
    [Index("Rowguid", Name = "AK_ProductModelProductDescription_rowguid", IsUnique = true)]
    public partial class ProductModelProductDescription
    {
        [Key]
        [Column("ProductModelID")]
        public int ProductModelId { get; set; }
        [Key]
        [Column("ProductDescriptionID")]
        public int ProductDescriptionId { get; set; }
        [Key]
        [StringLength(6)]
        public string Culture { get; set; } = null!;
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ProductDescriptionId")]
        [InverseProperty("ProductModelProductDescriptions")]
        public virtual ProductDescription ProductDescription { get; set; } = null!;
        [ForeignKey("ProductModelId")]
        [InverseProperty("ProductModelProductDescriptions")]
        public virtual ProductModel ProductModel { get; set; } = null!;
    }
}
