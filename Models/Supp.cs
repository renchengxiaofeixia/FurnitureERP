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

    /// <summary>
    /// 供应商名
    /// </summary>
    [StringLength(50)]
    public string? SuppName { get; set; }

    /// <summary>
    /// 供应商手机号
    /// </summary>
    [StringLength(20)]
    public string? SuppMobile { get; set; }

    /// <summary>
    /// 供应商公司
    /// </summary>
    [StringLength(20)]
    public string? SuppCompany { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [Required]
    public bool? IsUsing { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
