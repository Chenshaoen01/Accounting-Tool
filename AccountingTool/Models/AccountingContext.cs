using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace AccountingTool.Models;

public partial class AccountingContext : DbContext
{
    private readonly IConfiguration? _configuration;
    public AccountingContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AccountingContext(DbContextOptions<AccountingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountingData> AccountingDatas { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetValue<string>("ConnectionStrings: Default"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountingData>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Category)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Time)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
