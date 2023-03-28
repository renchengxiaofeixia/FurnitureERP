using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("role_permit")]
public partial class RolePermit
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(100)]
    public string RoleName { get; set; } = null!;

    public int RoleId { get; set; }

    public string? PermitData { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }
}
