using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("sys_dict")]
public partial class SysDict
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string? DictCode { get; set; }

    [StringLength(200)]
    public string? DictName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
