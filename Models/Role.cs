using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("role")]
public partial class Role
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 角色名
    /// </summary>
    [StringLength(100)]
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(200)]
    public string Remark { get; set; } = null!;

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
