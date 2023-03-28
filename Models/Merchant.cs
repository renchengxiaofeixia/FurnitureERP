using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("merchant")]
public partial class Merchant
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string MerchantName { get; set; } = null!;

    [StringLength(100)]
    public string? Password { get; set; }

    [StringLength(100)]
    public string? MobileNo { get; set; }

    [StringLength(100)]
    public string? CompanyName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }
}
