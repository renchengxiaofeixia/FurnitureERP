using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.Dtos
{
    public class CreateSubItemDto
    {
        public string? ItemNo { get; set; }

        public string? SubItemNo { get; set; }

        public int Num { get; set; }

        public string? Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public string? Creator { get; set; }

        public string? SellerNick { get; set; }

        public Guid MerchantGuid { get; set; }
    }
}
