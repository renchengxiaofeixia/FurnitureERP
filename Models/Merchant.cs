using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("merchant")]
public partial class Merchant
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 商户账号
    /// </summary>
    [StringLength(50)]
    public string MerchantName { get; set; } = null!;

    /// <summary>
    /// 密码
    /// </summary>
    [StringLength(100)]
    public string? Password { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [StringLength(100)]
    public string? MobileNo { get; set; }

    /// <summary>
    /// 商户公司
    /// </summary>
    [StringLength(100)]
    public string? CompanyName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }
}
