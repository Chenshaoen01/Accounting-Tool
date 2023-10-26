using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AccountingTool.Models;

public partial class AccountingContext : DbContext
{
    public AccountingContext()
    {
    }

    public AccountingContext(DbContextOptions<AccountingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountingData> AccountingDatas { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Database=Accounting;User ID=sa;Password=Tp6gl4p19980515;TrustServerCertificate=true");

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
            entity.Property(e => e.Time).HasColumnType("datetime");
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
