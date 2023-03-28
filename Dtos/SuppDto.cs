using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.Dtos
{
    public class SuppDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string? SuppName { get; set; }
        public string? SuppMobile { get; set; }
        public string? SuppCompany { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool? IsUsing { get; set; }
        public Guid MerchantGuid { get; set; }
    }
}
