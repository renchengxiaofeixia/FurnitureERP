using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("custom_property")]
public partial class CustomProperty
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string FromNo { get; set; } = null!;

    [StringLength(50)]
    public string ModuleNo { get; set; } = null!;

    [StringLength(100)]
    public string? PropertyName { get; set; }

    [StringLength(100)]
    public string? PropertyValue { get; set; }

    [StringLength(50)]
    public string? PropertyType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    public Guid MerchantGuid { get; set; }
}
