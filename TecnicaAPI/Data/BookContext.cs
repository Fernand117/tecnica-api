using Microsoft.EntityFrameworkCore;
using TecnicaAPI.Models;

namespace TecnicaAPI.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) {}
        public DbSet<BookEntity> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookEntity>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_BookId");
                entity.Property(e => e.Id).UseIdentityColumn();

                entity.ToTable("Books");

                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ISBN).HasMaxLength(13);
                entity.Property(e => e.PublishedDate);
                entity.Property(e => e.Sumary).HasMaxLength(1000);
            });
        }
    }
}
