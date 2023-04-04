using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("logis_point")]
public partial class LogisPoint
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(20)]
    public string LogisName { get; set; } = null!;

    [StringLength(20)]
    public string? State { get; set; }

    [StringLength(20)]
    public string? City { get; set; }

    [StringLength(20)]
    public string? District { get; set; }

    [StringLength(200)]
    public string PointName { get; set; } = null!;

    [StringLength(500)]
    public string? PointAdress { get; set; }

    [StringLength(50)]
    public string? PointMobile { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal LowestPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal GanPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ZhiPrice { get; set; }

    public bool IsPost { get; set; }

    public int EstTime { get; set; }

    [StringLength(200)]
    public string? Remark { get; set; }

    public bool IsUsing { get; set; }

    [StringLength(20)]
    public string Creator { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    public Guid MerchantGuid { get; set; }
}
