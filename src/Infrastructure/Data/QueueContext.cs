using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.QueueManager.Infrastructure.Data
{
    public class QueueContext : IdentityDbContext<ApplicationUser>
    {
        public QueueContext(DbContextOptions<QueueContext> options)
            : base(options)
        {
        }

        public DbSet<Office> Offices { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<OfficeTask> OfficesTasks { get; set; }
        public DbSet<OfficeOperator> OfficesOperators { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<DisplayMedia> DisplayMedias { get; set; }
        public DbSet<DisplayMessage> DisplayMessages { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Office>(ConfigureOffice);
            builder.Entity<TaskEntity>(ConfigureTask);
            builder.Entity<OfficeTask>(ConfigureOfficeTask);
            builder.Entity<OfficeOperator>(ConfigureOfficeOperator);
            builder.Entity<Media>(ConfigureMedia);
            builder.Entity<Priority>(ConfigurePriority);
            builder.Entity<Status>(ConfigureStatus);
            builder.Entity<Ticket>(ConfigureTicket);
            builder.Entity<DisplayMedia>(ConfigureDisplayMedia);
            builder.Entity<DisplayMessage>(ConfigureDisplayMessages);
            builder.Entity<Comment>(ConfigureComments);
            builder.Entity<ApplicationUser>(ConfigureApplicationUser);

            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted &&
                e.Metadata.GetProperties().Any(x => x.Name == "Activo")))
            {
                if ((bool)item.CurrentValues["Activo"])
                {
                    item.State = EntityState.Unchanged;
                    item.CurrentValues["Activo"] = false;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ConfigureOffice(EntityTypeBuilder<Office> builder)
        {
            builder.HasQueryFilter(x => x.Activo == true);
            builder.Property(x => x.Name).HasMaxLength(30).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);
        }

        private void ConfigureTask(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Prefix).HasMaxLength(1).IsRequired();
            builder.HasQueryFilter(x => x.Activo == true);
        }

        private void ConfigureOfficeTask(EntityTypeBuilder<OfficeTask> builder)
        {
            builder.HasKey(x => new { x.OfficeId, x.TaskId });
            builder.HasQueryFilter(x => x.Activo == true);
        }

        private void ConfigureOfficeOperator(EntityTypeBuilder<OfficeOperator> builder)
        {
            builder.HasKey(x => new { x.OfficeId, x.ApplicationUserId });
            builder.HasQueryFilter(x => x.Activo == true);
        }

        private void ConfigureMedia(EntityTypeBuilder<Media> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ContentType).HasMaxLength(30).IsRequired();
            builder.Property(x => x.FullPath).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Url).HasMaxLength(300).IsRequired();
        }

        private void ConfigurePriority(EntityTypeBuilder<Priority> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(30).IsRequired();
        }

        private void ConfigureStatus(EntityTypeBuilder<Status> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).HasMaxLength(30).IsRequired();
        }

        private void ConfigureTicket(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(x => x.DisplayTokenName).HasMaxLength(4);
        }

        private void ConfigureDisplayMedia(EntityTypeBuilder<DisplayMedia> builder)
        {
            builder.HasIndex(x => x.MediaId).IsUnique();
        }

        private void ConfigureDisplayMessages(EntityTypeBuilder<DisplayMessage> builder)
        {
            builder.Property(x => x.Message).HasMaxLength(200).IsRequired();
        }

        private void ConfigureComments(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(x => x.Message).HasMaxLength(500).IsRequired();
        }

        private void ConfigureApplicationUser(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasQueryFilter(x => x.Activo == true);
        }
    }
}
