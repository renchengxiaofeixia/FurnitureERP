using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("login_log")]
public partial class LoginLog
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [Column("IP")]
    [StringLength(20)]
    public string? Ip { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [StringLength(20)]
    public string? Browser { get; set; }

    [StringLength(20)]
    public string? Os { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    [StringLength(20)]
    public string? Device { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    [StringLength(20)]
    public string? BrowserInfo { get; set; }

    /// <summary>
    /// 登录状态
    /// </summary>
    [Required]
    public bool? Status { get; set; }

    /// <summary>
    /// 登录信息
    /// </summary>
    [StringLength(50)]
    public string? Msg { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(20)]
    public string? Creator { get; set; }

    public Guid MerchantGuid { get; set; }
}
