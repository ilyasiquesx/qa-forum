using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QAForum.Application.Common.Interfaces;
using QAForum.Domain.Entities;
using QAForum.Domain.Interfaces;
using QAForum.Infrastructure.Identity;

namespace QAForum.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly ICurrentUserService _currentUserService;

        public AppDbContext(DbContextOptions options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public new DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Question>().HasOne(q => q.Created).WithMany(u => u.Questions)
                .HasForeignKey(q => q.CreatedBy).IsRequired();

            builder.Entity<Question>().HasOne(q => q.BestAnswer)
                .WithOne(a => a.Question)
                .HasForeignKey<Question>(q => q.BestAnswerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Answer>().HasOne(q => q.Created).WithMany(u => u.Answers)
                .HasForeignKey(a => a.CreatedBy).IsRequired();

            builder.Entity<User>().HasIndex(s => s.UserName).IsUnique();
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var addedAuditedEntities = ChangeTracker.Entries<IAuditableEntity>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<IAuditableEntity>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

            var now = DateTime.Now;
            foreach (var added in addedAuditedEntities)
            {
                added.CreatedAt = now;
                added.UpdatedAt = now;
                added.CreatedBy = _currentUserService.UserId;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.UpdatedAt = now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}