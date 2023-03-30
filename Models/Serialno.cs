using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("serialno")]
public partial class Serialno
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string ModuleNo { get; set; } = null!;

    [StringLength(10)]
    public string Prefix { get; set; } = null!;

    [StringLength(10)]
    public string Ending { get; set; } = null!;

    public long LoopNum { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(200)]
    public string Creator { get; set; } = null!;

    [StringLength(200)]
    public string Remark { get; set; } = null!;
}
