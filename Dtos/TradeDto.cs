namespace FurnitureERP.Dtos
{
    public class TradeDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public decimal? AdjustFee { get; set; }
        public string? AlipayNo { get; set; }
        public string? BuyerAlipayNo { get; set; }
        public string? BuyerMemo { get; set; }
        public string? BuyerNick { get; set; }
        public string? BuyerOpenUid { get; set; }
        public decimal? CommissionFee { get; set; }
        public DateTime? ConsignTime { get; set; }
        public DateTime? Created { get; set; }
        public decimal? CreditCardFee { get; set; }
        public decimal? DiscountFee { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? Modified { get; set; }
        public decimal? Payment { get; set; }
        public DateTime? PayTime { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverCity { get; set; }
        public string? ReceiverDistrict { get; set; }
        public string? ReceiverTown { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverMobile { get; set; }
        public string? ReceiverPhone { get; set; }
        public string? ReceiverState { get; set; }
        public string? ReceiverZip { get; set; }
        public int SellerFlag { get; set; }
        public string? SellerMemo { get; set; }
        public string? SellerName { get; set; }
        public string? SellerNick { get; set; }
        public string? Status { get; set; }
        public decimal? StepPaidFee { get; set; }
        public string? StepTradeStatus { get; set; }
        public string? Tid { get; set; }
        public DateTime? TimeoutActionTime { get; set; }
        public decimal? TotalFee { get; set; }
        public string? TradeFrom { get; set; }
        public string? TradeMemo { get; set; }
        public bool IsAudit { get; set; }
        public string? AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }
        public bool IsGoodsAudit { get; set; }
        public string? GoodsAuditUser { get; set; }
        public DateTime? GoodsAuditDate { get; set; }
        public bool IsFinAudit { get; set; }
        public DateTime? FinAuditDate { get; set; }
        public string? FinAuditUser { get; set; }
        public bool IsPrint { get; set; }
        public string? PrintUser { get; set; }
        public DateTime? PrintDate { get; set; }
        public bool IsSend { get; set; }
        public string? SendUser { get; set; }
        public DateTime? SendDate { get; set; }
        public bool IsPurchase { get; set; }
        public string? PurchaseUser { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? WareName { get; set; }
        public string? LogisNo { get; set; }
        public string? LogisName { get; set; }
        public decimal LogisFee { get; set; }
        public string? InstallName { get; set; }
        public decimal InstallFee { get; set; }
        public Guid? PrintSession { get; set; }
        public bool IsSelfAdd { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public List<TradeItemDto> ItemDtos { get; set; }
    }

    public class CreateTradeDto
    {
        public decimal? AdjustFee { get; set; }
        public string? AlipayNo { get; set; }
        public string? BuyerAlipayNo { get; set; }
        public string? BuyerMemo { get; set; }
        public string? BuyerNick { get; set; }
        public string? BuyerOpenUid { get; set; }
        public decimal? CommissionFee { get; set; }
        public DateTime? ConsignTime { get; set; }
        public DateTime? Created { get; set; }
        public decimal? CreditCardFee { get; set; }
        public decimal? DiscountFee { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? Modified { get; set; }
        public decimal? Payment { get; set; }
        public DateTime? PayTime { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverCity { get; set; }
        public string? ReceiverDistrict { get; set; }
        public string? ReceiverTown { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverMobile { get; set; }
        public string? ReceiverPhone { get; set; }
        public string? ReceiverState { get; set; }
        public string? ReceiverZip { get; set; }
        public int SellerFlag { get; set; }
        public string? SellerMemo { get; set; }
        public string? SellerName { get; set; }
        public string? SellerNick { get; set; }
        public string? Status { get; set; }
        public decimal? StepPaidFee { get; set; }
        public string? StepTradeStatus { get; set; }
        public string? Tid { get; set; }
        public DateTime? TimeoutActionTime { get; set; }
        public decimal? TotalFee { get; set; }
        public string? TradeFrom { get; set; }
        public string? TradeMemo { get; set; }
        public bool IsAudit { get; set; }
        public string? AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }
        public bool IsGoodsAudit { get; set; }
        public string? GoodsAuditUser { get; set; }
        public DateTime? GoodsAuditDate { get; set; }
        public bool IsFinAudit { get; set; }
        public DateTime? FinAuditDate { get; set; }
        public string? FinAuditUser { get; set; }
        public bool IsPrint { get; set; }
        public string? PrintUser { get; set; }
        public DateTime? PrintDate { get; set; }
        public bool IsSend { get; set; }
        public string? SendUser { get; set; }
        public DateTime? SendDate { get; set; }
        public bool IsPurchase { get; set; }
        public string? PurchaseUser { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? WareName { get; set; }
        public string? LogisNo { get; set; }
        public string? LogisName { get; set; }
        public decimal LogisFee { get; set; }
        public string? InstallName { get; set; }
        public decimal InstallFee { get; set; }
        public Guid? PrintSession { get; set; }
        public bool IsSelfAdd { get; set; }
        public List<CreateTradeItemDto> ItemDtos { get; set; }
    }

    public class SendTradeDto
    {
        public long Id { get; set; }
        public string Tid { get; set; }
        public string? LogisNo { get; set; }
        public string? LogisName { get; set; }
        public decimal LogisFee { get; set; }
        public string? InstallName { get; set; }
        public decimal InstallFee { get; set; }
        public Guid? PrintSession { get; set; }
    }

    public class TradeItemDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string? Tid { get; set; }
        public string? PicPath { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public string? StdItemNo { get; set; }
        public DateTime? Modified { get; set; }
        public decimal Num { get; set; }
        public long? NumIid { get; set; }
        public string? OrderFrom { get; set; }
        public decimal Payment { get; set; }
        public decimal DivideOrderFee { get; set; }
        public decimal PartMjzDiscount { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public string? SellerNick { get; set; }
        public bool IsMade { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creator { get; set; }
    }

    public class CreateTradeItemDto
    {
        public string? PicPath { get; set; }
        public required string ItemName { get; set; }
        public required string ItemNo { get; set; }
        public string StdItemNo { get; set; }
        public DateTime? Modified { get; set; }
        public decimal Num { get; set; }
        public long? NumIid { get; set; }
        public string? OrderFrom { get; set; }
        public decimal Payment { get; set; }
        public decimal DivideOrderFee { get; set; }
        public decimal PartMjzDiscount { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public string? SellerNick { get; set; }
        public bool IsMade { get; set; }
        public string? Remark { get; set; }
    }

    public class TradeItemMatchInventoryDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string? Tid { get; set; }
        public string? PicPath { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public string? StdItemNo { get; set; }
        public DateTime? Modified { get; set; }
        public decimal Num { get; set; }
        public long? NumIid { get; set; }
        public string? OrderFrom { get; set; }
        public decimal Payment { get; set; }
        public decimal DivideOrderFee { get; set; }
        public decimal PartMjzDiscount { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public string? SellerNick { get; set; }
        public bool IsMade { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creator { get; set; }
        public DateTime? AllocInventoryDate { get; set; }
        public string? AllocInventoryUser { get; set; }
        public long? InventoryId { get; set; }
        public string? SuppName { get; set; }
    }

    public class CreateTradeItemMatchInventoryDto
    {
        public required string Tid { get; set; }
        public string? PicPath { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public string? StdItemNo { get; set; }
        public DateTime? Modified { get; set; }
        public decimal Num { get; set; }
        public long? NumIid { get; set; }
        public string? OrderFrom { get; set; }
        public decimal Payment { get; set; }
        public decimal DivideOrderFee { get; set; }
        public decimal PartMjzDiscount { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public string? SellerNick { get; set; }
        public bool IsMade { get; set; }
        public string? Remark { get; set; }
        public DateTime? AllocInventoryDate { get; set; }
        public string? AllocInventoryUser { get; set; }
        public long? InventoryId { get; set; }
        public string? SuppName { get; set; }
    }

    public partial class TradePayDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public required string Tid { get; set; }
        public string? PayWay { get; set; }
        public decimal Payment { get; set; }
        public DateTime? PayTime { get; set; }
        public string? Remark { get; set; }
        public string? Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public partial class CreateTradePayDto
    {
        public required string Tid { get; set; }
        public string? PayWay { get; set; }
        public decimal Payment { get; set; }
        public DateTime? PayTime { get; set; }
        public string? Remark { get; set; }
    }

}
