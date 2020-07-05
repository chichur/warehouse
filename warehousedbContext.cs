using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace warehouse
{
    public partial class warehousedbContext : DbContext
    {
        public warehousedbContext()
        {
        }

        public warehousedbContext(DbContextOptions<warehousedbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CargoHistory> CargoHistory { get; set; }
        public virtual DbSet<Platforms> Platforms { get; set; }
        public virtual DbSet<PlatformsHistory> PlatformsHistory { get; set; }
        public virtual DbSet<Stocks> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=warehousedb;Username=test;Password=test");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CargoHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cargo_history");

                entity.Property(e => e.Cargo).HasColumnName("cargo");

                entity.Property(e => e.IdPlatform).HasColumnName("id_platform");

                entity.Property(e => e.Stamp).HasColumnName("stamp");
            });

            modelBuilder.Entity<Platforms>(entity =>
            {
                entity.HasKey(e => e.IdPlatform)
                    .HasName("platforms_pkey");

                entity.ToTable("platforms");

                entity.Property(e => e.IdPlatform).HasColumnName("id_platform");

                entity.Property(e => e.Cargo).HasColumnName("cargo");
            });

            modelBuilder.Entity<PlatformsHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("platforms_history");

                entity.Property(e => e.IdPlatform).HasColumnName("id_platform");

                entity.Property(e => e.IdStock).HasColumnName("id_stock");

                entity.Property(e => e.NameStock)
                    .IsRequired()
                    .HasColumnName("name_stock")
                    .HasMaxLength(20);

                entity.Property(e => e.Picket).HasColumnName("picket");

                entity.Property(e => e.Stamp).HasColumnName("stamp");
            });

            modelBuilder.Entity<Stocks>(entity =>
            {
                entity.HasKey(e => e.IdStock)
                    .HasName("stocks_pkey");

                entity.ToTable("stocks");

                entity.Property(e => e.IdStock).HasColumnName("id_stock");

                entity.Property(e => e.IdPlatform).HasColumnName("id_platform");

                entity.Property(e => e.NameStock)
                    .HasColumnName("name_stock")
                    .HasMaxLength(20);

                entity.Property(e => e.Picket).HasColumnName("picket");

                entity.HasOne(d => d.IdPlatformNavigation)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.IdPlatform)
                    .HasConstraintName("stocks_id_platform_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //using(var de = new warehousedbContext());
    }
}
