using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AplicationCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.QueueManager.Infrastructure.Data
{
    public class QueueContext : IdentityDbContext<ApplicationUser>
    {
        public QueueContext(DbContextOptions<QueueContext> options)
            : base(options)
        {
        }

        public DbSet<Ventanilla> Ventanillas { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<VentanillaTarea> VentanillasTareas { get; set; }
        public DbSet<VentanillaOperador> VentanillasOperadores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VentanillaTarea>().HasKey(x => new { x.VentanillaId, x.TareaId });
            builder.Entity<VentanillaOperador>().HasKey(x => new { x.VentanillaId, x.ApplicationUserId });

            builder.Entity<ApplicationUser>().HasQueryFilter(x => x.Activo == true);
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted &&
                e.Metadata.GetProperties().Any(x => x.Name == "Activo")))
            {
                item.State = EntityState.Unchanged;
                item.CurrentValues["Activo"] = false;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
