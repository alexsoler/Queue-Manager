using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entities;

namespace Microsoft.QueueManager.Infrastructure.Data
{
    public class QueueDbContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var context = new QueueContext(
                serviceProvider.GetRequiredService<DbContextOptions<QueueContext>>()))
            {
                if (!await context.Tasks.AnyAsync())
                {
                    await context.Tasks.AddRangeAsync(
                        new TaskEntity
                        {
                            Name = "Matricula",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new TaskEntity
                        {
                            Name = "Pago de mensualidad",
                            CreationDate = DateTime.Now,
                            Activo = true
                        }
                    );

                    await context.SaveChangesAsync();
                }

                if (!await context.Offices.AnyAsync())
                {
                    await context.Offices.AddRangeAsync(
                        new Office
                        {
                            Name = "Ventanilla 1",
                            Description = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Labore, aspernatur.",
                            Prefix = "A",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 2",
                            Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nisi, eos!",
                            Prefix = "B",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 3",
                            Description = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Labore, aspernatur.",
                            Prefix = "C",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 4",
                            Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nisi, eos!",
                            Prefix = "B",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 5",
                            Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nisi, eos!",
                            Prefix = "C",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 6",
                            Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nisi, eos!",
                            Prefix = "D",
                            CreationDate = DateTime.Now,
                            Activo = true
                        }
                    );

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
