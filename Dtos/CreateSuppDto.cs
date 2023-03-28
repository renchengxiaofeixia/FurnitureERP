using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.Dtos
{
    public class CreateSuppDto
    {
        public string? SuppName { get; set; }
        public string? SuppMobile { get; set; }
        public string? SuppCompany { get; set; }
        public string? Remark { get; set; }
        public bool? IsUsing { get; set; }
        public Guid MerchantGuid { get; set; }
    }
}
