using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("sys_dict_value")]
public partial class SysDictValue
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string? DictCode { get; set; }

    [StringLength(200)]
    public string? DataValue { get; set; }

    [StringLength(2000)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [Required]
    public bool? IsUsing { get; set; }

    public byte[] TimeStamp { get; set; } = null!;

    public Guid MerchantGuid { get; set; }
}
