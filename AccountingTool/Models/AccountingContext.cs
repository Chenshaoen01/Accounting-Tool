using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AccountingTool.Models;

public partial class AccountingContext : DbContext
{
    public readonly string _connectionString;
    public AccountingContext(DbContextOptions<AccountingContext> options, IConfiguration configuration)
        : base(options)
    {
        _connectionString = configuration.GetValue<string>("ConnectionStrings:Default");
    }

    public virtual DbSet<AccountingData> AccountingDatas { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountingData>(entity =>
        {
            entity.HasKey(e => e.Id);

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
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
