using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("storage")]
public partial class Storage
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string? StorageNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StorageDate { get; set; }

    [StringLength(20)]
    public string? StorageType { get; set; }

    [StringLength(50)]
    public string? SuppName { get; set; }

    [StringLength(20)]
    public string? WareName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AggregateAmount { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    public bool IsAudit { get; set; }

    [StringLength(50)]
    public string? AuditUser { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AuditDate { get; set; }

    [StringLength(50)]
    public string? PurchaseNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PaidFee { get; set; }

    public Guid MerchantGuid { get; set; }
}
