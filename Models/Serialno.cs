using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("serial_no")]
public partial class SerialNo
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 模块名
    /// </summary>
    [StringLength(50)]
    public string ModuleNo { get; set; } = null!;

    /// <summary>
    /// 前缀
    /// </summary>
    [StringLength(10)]
    public string Prefix { get; set; } = null!;

    /// <summary>
    /// 后缀
    /// </summary>
    [StringLength(10)]
    public string Ending { get; set; } = null!;

    /// <summary>
    /// 数量（同模块当天）
    /// </summary>
    public long LoopNum { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(200)]
    public string Creator { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(200)]
    public string Remark { get; set; } = null!;

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }
}
