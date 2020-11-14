using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Project0.DataModels.Entities
{
    public partial class Project0Context : DbContext
    {
        public Project0Context()
        {
        }

        public Project0Context(DbContextOptions<Project0Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationInventory> LocationInventories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderContent> OrderContents { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasIndex(e => e.Email, "UQ__Customer__A9D105342BEC5F29")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(99);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(99);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(99);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.HasIndex(e => e.Address, "UQ__Location__7D0C3F3263A96C06")
                    .IsUnique();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(99);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(99);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(99);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(99);

                entity.Property(e => e.Phone).HasMaxLength(99);

                entity.Property(e => e.PostalCode).HasMaxLength(99);

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<LocationInventory>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.ProductId })
                    .HasName("PK__Location__2CBE68FBF025792F");

                entity.ToTable("LocationInventory");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationInventories)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LocationI__Locat__2739D489");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.LocationInventories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LocationI__Produ__282DF8C2");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__CustomerI__2CF2ADDF");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__LocationI__2DE6D218");
            });

            modelBuilder.Entity<OrderContent>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__OrderCon__08D097A32220DF43");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderContents)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderCont__Order__30C33EC3");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderContents)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderCont__Produ__31B762FC");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(99);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
