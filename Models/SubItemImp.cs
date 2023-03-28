using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("sub_item_imp")]
public partial class SubItemImp
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string? ItemNo { get; set; }

    [StringLength(200)]
    public string? SubItemNo1 { get; set; }

    public int Num1 { get; set; }

    [StringLength(200)]
    public string? SubItemNo2 { get; set; }

    public int Num2 { get; set; }

    [StringLength(200)]
    public string? SubItemNo3 { get; set; }

    public int Num3 { get; set; }

    [StringLength(200)]
    public string? SubItemNo4 { get; set; }

    public int Num4 { get; set; }

    [StringLength(200)]
    public string? SubItemNo5 { get; set; }

    public int Num5 { get; set; }

    [StringLength(200)]
    public string? SubItemNo6 { get; set; }

    public int Num6 { get; set; }

    [StringLength(200)]
    public string? SubItemNo7 { get; set; }

    public int Num7 { get; set; }

    [StringLength(200)]
    public string? SubItemNo8 { get; set; }

    public int Num8 { get; set; }

    [StringLength(200)]
    public string? SubItemNo9 { get; set; }

    public int Num9 { get; set; }

    [StringLength(200)]
    public string? SubItemNo10 { get; set; }

    public int Num10 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }
}
