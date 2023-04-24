using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("printing")]
public partial class Printing
{
    [Key]
    public long Id { get; set; }

    public long? TradeId { get; set; }

    public Guid? PrintSession { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }
}
