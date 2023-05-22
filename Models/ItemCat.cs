using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("item_cat")]
public partial class ItemCat
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 分类名
    /// </summary>
    [StringLength(50)]
    public string? CateName { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [StringLength(200)]
    public string? Type { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsUsing { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    public Guid MerchantGuid { get; set; }

    /// <summary>
    /// 父级id
    /// </summary>
    public int Pid { get; set; }

    public int Sort { get; set; }
}
