﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("phone_code")]
public partial class PhoneCode
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [StringLength(100)]
    public string? MobileNo { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [StringLength(100)]
    public string? SmsCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }
}
