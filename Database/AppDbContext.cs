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

    public virtual DbSet<ErpModule> ErpModules { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemImp> ItemImps { get; set; }

    public virtual DbSet<ItemPackage> ItemPackages { get; set; }

    public virtual DbSet<LogisImp> LogisImps { get; set; }

    public virtual DbSet<LogisPoint> LogisPoints { get; set; }

    public virtual DbSet<LogisPointImp> LogisPointImps { get; set; }

    public virtual DbSet<Logistic> Logistics { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Merchant> Merchants { get; set; }

    public virtual DbSet<PhoneCode> PhoneCodes { get; set; }

    public virtual DbSet<PropertyConfig> PropertyConfigs { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermit> RolePermits { get; set; }

    public virtual DbSet<SerialNo> SerialNos { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<StorageOrder> StorageOrders { get; set; }

    public virtual DbSet<SubItem> SubItems { get; set; }

    public virtual DbSet<SubItemImp> SubItemImps { get; set; }

    public virtual DbSet<Supp> Supps { get; set; }

    public virtual DbSet<SuppItem> SuppItems { get; set; }

    public virtual DbSet<SuppItemImp> SuppItemImps { get; set; }

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

        modelBuilder.Entity<ErpModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__module");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_invent__3214EC07035179CE");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_item__3214EC0710AB74EC");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<ItemImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__itemimp14EC0710AB74EC");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<ItemPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__item_package14EC0710AB74EC");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
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

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
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
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
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

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<PhoneCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SmsCode__3214EC073F466844");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
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

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeliveryDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3214EC07D6033F91");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<RolePermit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rolePerm__3214EC0734C8D9D1");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<SerialNo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__serialno__3214EC07741C221B");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Creator).HasDefaultValueSql("('')");
            entity.Property(e => e.Ending).HasDefaultValueSql("('')");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ModuleNo).HasDefaultValueSql("('')");
            entity.Property(e => e.Prefix).HasDefaultValueSql("('')");
            entity.Property(e => e.Remark).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_shop__3214EC076C390A4C");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__storage__3214EC07F20AE1A3");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.StorageDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<StorageOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__storage___3214EC0771BF2F77");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<SubItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subItem__3214EC07C2F9DC4E");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Num).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<SubItemImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sub_item__3214EC0754710794");

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

        modelBuilder.Entity<Supp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_supp__3214EC071367E606");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<SuppItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supp_ite__3214EC071E0BF171");

            entity.ToTable("supp_item", tb => tb.HasComment("供应商产品表"));

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<SuppItemImp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supp_ite__3214EC07BF48BEAF");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_user__3214EC0747C69FAC");

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
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
            entity.Property(e => e.IsUsing).HasDefaultValueSql("((1))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
