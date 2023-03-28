using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.Dtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Remark { get; set; }
        public string MerchantName { get; set; }
        public Guid MerchantGuid { get; set; }
        public bool IsUsing { get; set; }
    }
}
