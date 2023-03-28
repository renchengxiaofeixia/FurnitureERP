using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("menu")]
public partial class Menu
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    public string? MenuJson { get; set; }

    public int MenuVersion { get; set; }

    public Guid MerchantGuid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }
}
