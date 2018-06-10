using System;
using Blink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Blink.Data.Context
{
    public partial class BLINKContext : DbContext
    {
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Affiliate> Affiliate { get; set; }
        public virtual DbSet<BinnacleAddress> BinnacleAddress { get; set; }
        public virtual DbSet<Person> Person { get; set; }

        public BLINKContext(DbContextOptions<BLINKContext> options)
: base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.InteriorNumber).HasMaxLength(50);

                entity.Property(e => e.OutdoorNumber).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Address_Person");
            });

            modelBuilder.Entity<Affiliate>(entity =>
            {
                entity.HasIndex(e => e.CodeAffiliate)
                   .IsUnique();

                entity.HasKey(e => e.PersonId);

                entity.Property(e => e.PersonId).ValueGeneratedNever();

                entity.Property(e => e.CodeAffiliate).HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Affiliate)
                    .HasForeignKey<Affiliate>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Affiliate_Person");
            });

            modelBuilder.Entity<BinnacleAddress>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.InteriorNumber).HasMaxLength(50);

                entity.Property(e => e.OutdoorNumber).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(500);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.BinnacleAddress)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_BinnacleAddress_Address");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.BinnacleAddress)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_BinnacleAddress_Person");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.Rfc)
                    .IsUnique();

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.HomePhone).HasMaxLength(50);

                entity.Property(e => e.MaternalSurname).HasMaxLength(150);

                entity.Property(e => e.MobilePhone).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.PasswordHash).HasMaxLength(8000);

                entity.Property(e => e.PasswordSalt).HasMaxLength(8000);

                entity.Property(e => e.PaternalSurname).HasMaxLength(150);

                entity.Property(e => e.Rfc).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });
        }
    }
}
