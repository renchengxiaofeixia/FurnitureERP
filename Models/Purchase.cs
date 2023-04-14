using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("purchase")]
public partial class Purchase
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string PurchaseNo { get; set; } = null!;

    [StringLength(50)]
    public string SuppName { get; set; } = null!;

    [StringLength(50)]
    public string WareName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AggregateAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PurchaseOrderDate { get; set; }

    [StringLength(50)]
    public string SettlementMode { get; set; } = null!;

    public bool IsAudit { get; set; }

    [StringLength(50)]
    public string? AuditUser { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AuditDate { get; set; }

    public bool IsPrint { get; set; }

    [StringLength(50)]
    public string? PrintUser { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PrintDate { get; set; }

    [StringLength(50)]
    public string? OuterNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Created { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DeliveryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PaidFee { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    public bool IsFromTrade { get; set; }

    [StringLength(50)]
    public string? Tid { get; set; }

    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
