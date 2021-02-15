using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QAForum.Infrastructure.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=QaForum;User Id=postgres;Password=mysecretpassword;");
            return new AppDbContext(builder.Options, null);
        }
    }
}