using Blink.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blink.Data
{
    public partial class BLINKContext : DbContext
    {
        public BLINKContext(DbContextOptions<BLINKContext> options)
    : base(options)
        { }
        public virtual DbSet<User> User { get; set; }      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.HomePhone).HasMaxLength(50);

                entity.Property(e => e.MaternalSurname).HasMaxLength(150);

                entity.Property(e => e.MobilePhone).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.PasswordHash).HasMaxLength(8000);

                entity.Property(e => e.PasswordSalt).HasMaxLength(8000);

                entity.Property(e => e.PaternalSurname).HasMaxLength(150);

                entity.Property(e => e.TypeUser).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Username).HasMaxLength(150);               
            });
        }
    }
}
