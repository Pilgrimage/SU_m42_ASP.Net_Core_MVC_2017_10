namespace FluffyDuffyMunchkinCatsRefactoring.Data
{
    using Microsoft.EntityFrameworkCore;

    public class CatsRefactoringDbContext : DbContext
    {
        public CatsRefactoringDbContext(DbContextOptions<CatsRefactoringDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Cat> Cats { get; set; }

    }
}