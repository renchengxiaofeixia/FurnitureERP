namespace FurnitureERP.Dtos
{
    public class CreateRoleDto
    {
        public string RoleName { get; set; }
        public string Remark { get; set; }
        public bool IsUsing { get; set; }
        public Guid MerchantGuid { get; set; }
    }
}
