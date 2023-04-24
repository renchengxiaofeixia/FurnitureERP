using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("sub_item")]
public partial class SubItem
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 商品编码
    /// </summary>
    [StringLength(200)]
    public string? ItemNo { get; set; }

    /// <summary>
    /// 下级编码
    /// </summary>
    [StringLength(200)]
    public string? SubItemNo { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Num { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }
}
