using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("supp")]
public partial class Supp
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string? SuppName { get; set; }

    [StringLength(20)]
    public string? SuppMobile { get; set; }

    [StringLength(20)]
    public string? SuppCompany { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [Required]
    public bool? IsUsing { get; set; }

    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
