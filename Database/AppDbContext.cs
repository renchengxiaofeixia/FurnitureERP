using System;
using System.Collections.Generic;
using FurnitureERP.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Database;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CustomProperty> CustomProperties { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryItemAdjust> InventoryItemAdjusts { get; set; }

    public virtual DbSet<InventoryItemMove> InventoryItemMoves { get; set; }

    public virtual DbSet<Inventorybarcode> Inventorybarcodes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemImp> ItemImps { get; set; }

    public virtual DbSet<ItemPackage> ItemPackages { get; set; }

    public virtual DbSet<ItemPackageImp> ItemPackageImps { get; set; }

    public virtual DbSet<LoginLog> LoginLogs { get; set; }

    public virtual DbSet<LogisImp> LogisImps { get; set; }

    public virtual DbSet<LogisPoint> LogisPoints { get; set; }

    public virtual DbSet<LogisPointImp> LogisPointImps { get; set; }

    public virtual DbSet<Logistic> Logistics { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Merchant> Merchants { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageImp> PackageImps { get; set; }

    public virtual DbSet<PhoneCode> PhoneCodes { get; set; }

    public virtual DbSet<PropertyConfig> PropertyConfigs { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }

    public virtual DbSet<RecordDelete> RecordDeletes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermit> RolePermits { get; set; }

    public virtual DbSet<SerialNo> SerialNos { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<StorageItem> StorageItems { get; set; }

    public virtual DbSet<SubItem> SubItems { get; set; }

    public virtual DbSet<SubItemImp> SubItemImps { get; set; }

    public virtual DbSet<Supp> Supps { get; set; }

    public virtual DbSet<SuppItem> SuppItems { get; set; }

    public virtual DbSet<SuppItemImp> SuppItemImps { get; set; }

    public virtual DbSet<SysDict> SysDicts { get; set; }

    public virtual DbSet<SysDictValue> SysDictValues { get; set; }

    public virtual DbSet<SysModule> SysModules { get; set; }

    public virtual DbSet<Trade> Trades { get; set; }

    public virtual DbSet<TradeItem> TradeItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:SqlConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomProperty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__custom_property");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FromNo).HasDefaultValueSql("(N'‘’')");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_invent__3214EC07035179CE");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<InventoryItemAdjust>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventor__3214EC076916443D");

            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<InventoryItemMove>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventor__3214EC079E498433");

            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Inventorybarcode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventorybarcode3214EC074D6A6A69");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_item__3214EC0710AB74EC");

            entity.Property(e => e.CostPrice).HasComment("采购价");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsCom).HasComment("是否组合商品");
            entity.Property(e => e.IsUsing)
                .HasDefaultValueSql("((1))")
                .HasComment("是否启用");
            entity.Property(e => e.ItemName).HasComment("商品名称");
            entity.Property(e => e.ItemNo).HasComment("商品编码");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.PackageQty).HasComment("包装件数");
            entity.Property(e => e.PicPath).HasComment("商品图");
            entity.Property(e => e.Price).HasComment("销售价");
            entity.Property(e => e.PurchaseDays).HasComment("采购周期");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.SafeQty).HasComment("安全库存");
            entity.Property(e => e.SellerNick).HasComment("所属店铺");
            entity.Property(e => e.SuppName).HasComment("供应商名");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Volume).HasComment("体积");
        });

        modelBuilder.Entity<ItemImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__itemimp14EC0710AB74EC");

            entity.Property(e => e.CostPrice).HasComment("采购价");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsCom).HasComment("是否组合产品");
            entity.Property(e => e.IsUsing)
                .HasDefaultValueSql("((1))")
                .HasComment("是否启用");
            entity.Property(e => e.ItemName).HasComment("商品名称");
            entity.Property(e => e.ItemNo).HasComment("商品编码");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.PackageQty).HasComment("包装件数");
            entity.Property(e => e.PicPath).HasComment("图片地址");
            entity.Property(e => e.Price).HasComment("销售价");
            entity.Property(e => e.PurchaseDays).HasComment("采购周期");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.SafeQty).HasComment("安全库存");
            entity.Property(e => e.SellerNick).HasComment("所属店铺");
            entity.Property(e => e.SuppName).HasComment("供应商");
            entity.Property(e => e.Volume).HasComment("体积");
        });

        modelBuilder.Entity<ItemPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__item_package14EC0710AB74EC");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<ItemPackageImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__item_package_imp_3214EC0754710794");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Num1).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num10).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num2).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num3).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num4).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num5).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num6).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num7).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num8).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num9).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<LoginLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__login_lo__3214EC073338D304");

            entity.Property(e => e.Browser).HasComment("浏览器");
            entity.Property(e => e.BrowserInfo).HasComment("浏览器信息");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Device).HasComment("设备");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Msg).HasComment("登录信息");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasComment("登录状态");
        });

        modelBuilder.Entity<LogisImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__logis_im__3214EC07E85E7C45");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<LogisPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__logis_po__3214EC0753503EFB");

            entity.Property(e => e.City).HasComment("市");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.District).HasComment("区");
            entity.Property(e => e.EstTime).HasComment("预计到货天数");
            entity.Property(e => e.GanPrice).HasComment("干线价格");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsPost).HasComment("是否包邮");
            entity.Property(e => e.IsUsing).HasComment("是否启用");
            entity.Property(e => e.LogisName).HasComment("物流名称");
            entity.Property(e => e.LowestPrice).HasComment("最低价");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.PointAdress).HasComment("物流点地址");
            entity.Property(e => e.PointMobile).HasComment("手机号");
            entity.Property(e => e.PointName).HasComment("物流点名称");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.State).HasComment("省");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.ZhiPrice).HasComment("支线价格");
        });

        modelBuilder.Entity<LogisPointImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__logis_po__3214EC07762695EF");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Logistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__logis__3214EC07C04E3B4C");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DefProv).HasComment("省份");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsDef).HasComment("是否默认物流");
            entity.Property(e => e.IsUsing).HasComment("是否启用");
            entity.Property(e => e.LogisAddr).HasComment("物流公司地址");
            entity.Property(e => e.LogisMobile).HasComment("联系方式");
            entity.Property(e => e.LogisName).HasComment("物流名称");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.Remark).HasComment("备注");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_menu__3214EC0707F6335A");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_merchant_3214EC0747C69FAC");

            entity.Property(e => e.CompanyName).HasComment("商户公司");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.MerchantName).HasComment("商户账号");
            entity.Property(e => e.MobileNo).HasComment("手机号");
            entity.Property(e => e.Password).HasComment("密码");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__packageC0710AB74EC");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<PackageImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__package_impC0710AB74EC");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<PhoneCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SmsCode__3214EC073F466844");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MobileNo).HasComment("手机号");
            entity.Property(e => e.SmsCode).HasComment("验证码");
        });

        modelBuilder.Entity<PropertyConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__property_config");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3214EC07BB243744");

            entity.Property(e => e.AggregateAmount).HasComment("总金额");
            entity.Property(e => e.AuditDate).HasComment("审核日期");
            entity.Property(e => e.AuditUser).HasComment("审核人");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeliveryDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("交付日期");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsAudit).HasComment("是否审核");
            entity.Property(e => e.IsFromTrade).HasComment("是否来源销售单");
            entity.Property(e => e.IsPrint).HasComment("是否打印");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.OuterNo).HasComment("外单号（工厂送货单号）");
            entity.Property(e => e.PaidFee).HasComment("付款金额");
            entity.Property(e => e.PrintDate).HasComment("打印日期");
            entity.Property(e => e.PrintUser).HasComment("打印人");
            entity.Property(e => e.PurchaseNo).HasComment("采购单号");
            entity.Property(e => e.PurchaseOrderDate).HasComment("采购时间");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.SettlementMode).HasComment("结算方式");
            entity.Property(e => e.SuppName).HasComment("供应商名");
            entity.Property(e => e.Tid).HasComment("销售订单号");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.WareName).HasComment("仓库名");
        });

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3214EC07D6033F91");

            entity.Property(e => e.Amount).HasComment("金额");
            entity.Property(e => e.CancelNum).HasComment("取消数量");
            entity.Property(e => e.CostPrice).HasComment("采购价");
            entity.Property(e => e.DeliveryDate).HasComment("交付日期");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsMade).HasComment("是否定制");
            entity.Property(e => e.ItemName).HasComment("商品名称");
            entity.Property(e => e.ItemNo).HasComment("商品编码（可为定制编码）");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.OrderGuid).HasComment("订单商品GUID");
            entity.Property(e => e.PurchaseNo).HasComment("采购单号");
            entity.Property(e => e.PurchaseNum).HasComment("采购数量");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.StdItemNo).HasComment("标准商品编码");
            entity.Property(e => e.StorageNum).HasComment("入库数量");
            entity.Property(e => e.SuppName).HasComment("供应商名称");
        });

        modelBuilder.Entity<RecordDelete>(entity =>
        {
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsUsing)
                .HasDefaultValueSql("((1))")
                .HasComment("是否启用");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.RoleName).HasComment("角色名");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<RolePermit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rolePerm__3214EC0734C8D9D1");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PermitData).HasComment("权限");
            entity.Property(e => e.RoleId).HasComment("角色ID");
            entity.Property(e => e.RoleName).HasComment("角色名");
        });

        modelBuilder.Entity<SerialNo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__serialno__3214EC07741C221B");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Creator).HasDefaultValueSql("('')");
            entity.Property(e => e.Ending)
                .HasDefaultValueSql("('')")
                .HasComment("后缀");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LoopNum).HasComment("数量（同模块当天）");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.ModuleNo)
                .HasDefaultValueSql("('')")
                .HasComment("模块名");
            entity.Property(e => e.Prefix)
                .HasDefaultValueSql("('')")
                .HasComment("前缀");
            entity.Property(e => e.Remark)
                .HasDefaultValueSql("('')")
                .HasComment("备注");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_shop__3214EC076C390A4C");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Phone).HasComment("手机号");
            entity.Property(e => e.Points).HasComment("平台扣点");
            entity.Property(e => e.SellerNick).HasComment("店铺名称");
            entity.Property(e => e.ShopType).HasComment("店铺类型：淘宝，拼多多，抖音");
            entity.Property(e => e.VenderId).HasComment("京东店铺ID");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__storage__3214EC07F20AE1A3");

            entity.Property(e => e.AggregateAmount).HasComment("总金额");
            entity.Property(e => e.AuditDate).HasComment("审核日期");
            entity.Property(e => e.AuditUser).HasComment("审核人");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsAudit).HasComment("是否审核");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.PaidFee).HasComment("支付金额");
            entity.Property(e => e.PurchaseNo).HasComment("采购单号");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.StorageDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("入库日期");
            entity.Property(e => e.StorageNo).HasComment("入库单号");
            entity.Property(e => e.StorageType).HasComment("入库类型：正常添加入库，采购单入库");
            entity.Property(e => e.SuppName).HasComment("供应商名");
            entity.Property(e => e.Tid).HasComment("来源于采购单的销售单号");
            entity.Property(e => e.WareName).HasComment("仓库名");
        });

        modelBuilder.Entity<StorageItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__storage___3214EC0771BF2F77");

            entity.Property(e => e.Amount).HasComment("金额");
            entity.Property(e => e.CostPrice).HasComment("采购价");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsMade).HasComment("是否定制");
            entity.Property(e => e.ItemName).HasComment("商品名称");
            entity.Property(e => e.ItemNo).HasComment("商品编码（可为定制编码）");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.PurchaseNo).HasComment("采购单号");
            entity.Property(e => e.PurchaseNum).HasComment("采购数量");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.StdItemNo).HasComment("标准商品编码");
            entity.Property(e => e.StorageNo).HasComment("入库单号");
            entity.Property(e => e.StorageNum).HasComment("入库数量");
            entity.Property(e => e.SuppName).HasComment("供应商名");
        });

        modelBuilder.Entity<SubItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subItem__3214EC07C2F9DC4E");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ItemNo).HasComment("商品编码");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.Num)
                .HasDefaultValueSql("((1))")
                .HasComment("数量");
            entity.Property(e => e.SubItemNo).HasComment("下级编码");
        });

        modelBuilder.Entity<SubItemImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sub_item__3214EC0754710794");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ItemNo).HasComment("商品编码");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.Num1)
                .HasDefaultValueSql("((1))")
                .HasComment("数量");
            entity.Property(e => e.Num10).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num2).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num3).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num4).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num5).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num6).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num7).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num8).HasDefaultValueSql("((1))");
            entity.Property(e => e.Num9).HasDefaultValueSql("((1))");
            entity.Property(e => e.SubItemNo1).HasComment("下级编码");
        });

        modelBuilder.Entity<Supp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_supp__3214EC071367E606");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing)
                .HasDefaultValueSql("((1))")
                .HasComment("是否启用");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.SuppCompany).HasComment("供应商公司");
            entity.Property(e => e.SuppMobile).HasComment("供应商手机号");
            entity.Property(e => e.SuppName).HasComment("供应商名");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<SuppItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supp_ite__3214EC07EC22863F");

            entity.Property(e => e.CostPrice).HasComment("成本价");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ItemNo).HasComment("商品编码");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.SuppName).HasComment("商户名");
        });

        modelBuilder.Entity<SuppItemImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supp_ite__3214EC07E48C6DCB");

            entity.Property(e => e.CostPrice).HasComment("成本价");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ItemNo).HasComment("商品编码");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.SuppName).HasComment("供应商名");
        });

        modelBuilder.Entity<SysDict>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sys_dict__3214EC076092F3A5");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<SysDictValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sys_dict__3214EC074F4981B0");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<SysModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__module");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Trade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__trade__3214EC0775F516FD");

            entity.Property(e => e.CommissionFee).HasDefaultValueSql("((0))");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreditCardFee).HasDefaultValueSql("((0))");
            entity.Property(e => e.DiscountFee).HasDefaultValueSql("((0))");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PurchaseDate).HasDefaultValueSql("((0))");
            entity.Property(e => e.StepPaidFee).HasDefaultValueSql("((0))");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<TradeItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__trade_it__3214EC071D398B0D");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_user__3214EC0747C69FAC");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing)
                .HasDefaultValueSql("((1))")
                .HasComment("是否启用");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.MerchantName).HasComment("商户名");
            entity.Property(e => e.Password).HasComment("密码");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UserName).HasComment("账号");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__userRole__3214EC072E1BDC42");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsUsing)
                .HasDefaultValueSql("((1))")
                .HasComment("是否启用");
            entity.Property(e => e.MerchantGuid).HasComment("商户GUID");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.WarehouseName).HasComment("仓库名称");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
