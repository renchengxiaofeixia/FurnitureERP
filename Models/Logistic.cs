using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("logistic")]
public partial class Logistic
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 物流名称
    /// </summary>
    [StringLength(200)]
    public string LogisName { get; set; } = null!;

    /// <summary>
    /// 联系方式
    /// </summary>
    [StringLength(100)]
    public string? LogisMobile { get; set; }

    /// <summary>
    /// 省份
    /// </summary>
    [StringLength(200)]
    public string? DefProv { get; set; }

    /// <summary>
    /// 物流公司地址
    /// </summary>
    [StringLength(200)]
    public string? LogisAddr { get; set; }

    /// <summary>
    /// 是否默认物流
    /// </summary>
    public bool IsDef { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500)]
    public string? Remark { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsUsing { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(20)]
    public string? Creator { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }
}
