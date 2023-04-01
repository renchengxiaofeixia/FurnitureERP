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

    [StringLength(200)]
    public string LogisName { get; set; } = null!;

    [StringLength(100)]
    public string? LogisMobile { get; set; }

    [StringLength(200)]
    public string? DefProv { get; set; }

    [StringLength(200)]
    public string? LogisAddr { get; set; }

    public bool IsDef { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    public bool IsUsing { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(20)]
    public string? Creator { get; set; }

    public Guid MerchantGuid { get; set; }
}
