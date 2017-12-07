namespace BookShop.Data
{
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;

    public class BookShopDbContext : DbContext
    {
        public BookShopDbContext(DbContextOptions<BookShopDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId});

            builder
                .Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            builder
                .Entity<BookCategory>()
                .HasOne(cb => cb.Book)
                .WithMany(b => b.Categories)
                .HasForeignKey(cb => cb.BookId);

            builder
                .Entity<BookCategory>()
                .HasOne(cb => cb.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(cb => cb.CategoryId);

            base.OnModelCreating(builder);
        }
    }
}