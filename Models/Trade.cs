using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("trade")]
public partial class Trade
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AdjustFee { get; set; }

    [StringLength(500)]
    public string? AlipayNo { get; set; }

    [StringLength(500)]
    public string? BuyerAlipayNo { get; set; }

    [StringLength(2000)]
    public string? BuyerMemo { get; set; }

    [StringLength(500)]
    public string? BuyerNick { get; set; }

    [StringLength(100)]
    public string? BuyerOpenUid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CommissionFee { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ConsignTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Created { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CreditCardFee { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DiscountFee { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Payment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PayTime { get; set; }

    [StringLength(500)]
    public string? ReceiverAddress { get; set; }

    [StringLength(50)]
    public string? ReceiverCity { get; set; }

    [StringLength(50)]
    public string? ReceiverDistrict { get; set; }

    [StringLength(50)]
    public string? ReceiverTown { get; set; }

    [StringLength(200)]
    public string? ReceiverName { get; set; }

    [StringLength(50)]
    public string? ReceiverMobile { get; set; }

    [StringLength(50)]
    public string? ReceiverPhone { get; set; }

    [StringLength(50)]
    public string? ReceiverState { get; set; }

    [StringLength(20)]
    public string? ReceiverZip { get; set; }

    public int? SellerFlag { get; set; }

    [StringLength(500)]
    public string? SellerMemo { get; set; }

    [StringLength(100)]
    public string? SellerName { get; set; }

    [StringLength(50)]
    public string? SellerNick { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? StepPaidFee { get; set; }

    [StringLength(50)]
    public string? StepTradeStatus { get; set; }

    [StringLength(50)]
    public string? Tid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TimeoutActionTime { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalFee { get; set; }

    [StringLength(50)]
    public string? TradeFrom { get; set; }

    [StringLength(1000)]
    public string? TradeMemo { get; set; }

    public bool IsAudit { get; set; }

    [StringLength(50)]
    public string? AuditUser { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AuditDate { get; set; }

    public bool IsGoodsAudit { get; set; }

    [StringLength(50)]
    public string? GoodsAuditUser { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? GoodsAuditDate { get; set; }

    public bool IsFinAudit { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinAuditDate { get; set; }

    [StringLength(50)]
    public string? FinAuditUser { get; set; }

    public bool IsPrint { get; set; }

    [StringLength(50)]
    public string? PrintUser { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PrintDate { get; set; }

    public bool IsSend { get; set; }

    [StringLength(50)]
    public string? SendUser { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SendDate { get; set; }

    public bool IsPurchase { get; set; }

    [StringLength(50)]
    public string? PurchaseUser { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PurchaseDate { get; set; }

    [StringLength(50)]
    public string? WareName { get; set; }

    [StringLength(50)]
    public string? LogisNo { get; set; }

    [StringLength(500)]
    public string? LogisName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal LogisFee { get; set; }

    [StringLength(50)]
    public string? InstallName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal InstallFee { get; set; }

    public Guid? PrintSession { get; set; }

    public bool IsSelfAdd { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
