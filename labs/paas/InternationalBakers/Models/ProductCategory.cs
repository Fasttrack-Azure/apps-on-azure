using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("ProductCategory", Schema = "SalesLT")]
    [Index("Name", Name = "AK_ProductCategory_Name", IsUnique = true)]
    [Index("Rowguid", Name = "AK_ProductCategory_rowguid", IsUnique = true)]
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            InverseParentProductCategory = new HashSet<ProductCategory>();
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("ProductCategoryID")]
        public int ProductCategoryId { get; set; }
        [Column("ParentProductCategoryID")]
        public int? ParentProductCategoryId { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ParentProductCategoryId")]
        [InverseProperty("InverseParentProductCategory")]
        public virtual ProductCategory? ParentProductCategory { get; set; }
        [InverseProperty("ParentProductCategory")]
        public virtual ICollection<ProductCategory> InverseParentProductCategory { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
