using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("Product", Schema = "SalesLT")]
    [Index("Name", Name = "AK_Product_Name", IsUnique = true)]
    [Index("ProductNumber", Name = "AK_Product_ProductNumber", IsUnique = true)]
    [Index("Rowguid", Name = "AK_Product_rowguid", IsUnique = true)]
    public partial class Product
    {
        public Product()
        {
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(25)]
        public string ProductNumber { get; set; } = null!;
        [StringLength(15)]
        public string? Color { get; set; }
        [Column(TypeName = "money")]
        public decimal StandardCost { get; set; }
        [Column(TypeName = "money")]
        public decimal ListPrice { get; set; }
        [StringLength(5)]
        public string? Size { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Weight { get; set; }
        [Column("ProductCategoryID")]
        public int? ProductCategoryId { get; set; }
        [Column("ProductModelID")]
        public int? ProductModelId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SellStartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SellEndDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DiscontinuedDate { get; set; }
        public byte[]? ThumbNailPhoto { get; set; }
        [StringLength(50)]
        public string? ThumbnailPhotoFileName { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ProductCategoryId")]
        [InverseProperty("Products")]
        public virtual ProductCategory? ProductCategory { get; set; }
        [ForeignKey("ProductModelId")]
        [InverseProperty("Products")]
        public virtual ProductModel? ProductModel { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
