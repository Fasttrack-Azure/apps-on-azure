using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InternationalBakers.Models
{
    [Table("BuildVersion")]
    public partial class BuildVersion
    {
        [Key]
        [Column("SystemInformationID")]
        public byte SystemInformationId { get; set; }
        [Column("Database Version")]
        [StringLength(25)]
        public string DatabaseVersion { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime VersionDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }
    }
}
