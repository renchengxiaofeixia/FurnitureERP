using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("supp_item")]
public partial class SuppItem
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 商户名
    /// </summary>
    [StringLength(50)]
    public string? SuppName { get; set; }

    /// <summary>
    /// 商品编码
    /// </summary>
    [StringLength(200)]
    public string? ItemNo { get; set; }

    /// <summary>
    /// 成本价
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }
}
