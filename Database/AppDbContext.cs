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

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemImp> ItemImps { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Merchant> Merchants { get; set; }

    public virtual DbSet<PhoneCode> PhoneCodes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermit> RolePermits { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<SubItem> SubItems { get; set; }

    public virtual DbSet<SubItemImp> SubItemImps { get; set; }

    public virtual DbSet<Supp> Supps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:SqlConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__t_shop__3214EC076C390A4C");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
