using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("user")]
public partial class User
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string MerchantName { get; set; } = null!;

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(100)]
    public string? Password { get; set; }

    [StringLength(2000)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [Required]
    public bool? IsUsing { get; set; }

    [StringLength(500)]
    public string? SellerNick { get; set; }

    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
