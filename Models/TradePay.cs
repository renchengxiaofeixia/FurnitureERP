using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("trade_pay")]
public partial class TradePay
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string? Tid { get; set; }

    [StringLength(50)]
    public string? PayWay { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Payment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PayTime { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    public Guid MerchantGuid { get; set; }
}
