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
                            Prefix = "M",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new TaskEntity
                        {
                            Name = "Pago de mensualidad",
                            Prefix = "P",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new TaskEntity
                        {
                            Name = "Pago de equivalencias",
                            Prefix = "E",
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
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 2",
                            Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nisi, eos!",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 3",
                            Description = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Labore, aspernatur.",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 4",
                            Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nisi, eos!",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 5",
                            Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nisi, eos!",
                            CreationDate = DateTime.Now,
                            Activo = true
                        },
                        new Office
                        {
                            Name = "Ventanilla 6",
                            Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nisi, eos!",
                            CreationDate = DateTime.Now,
                            Activo = true
                        }
                    );

                    await context.SaveChangesAsync();
                }

                if(!await context.Priorities.AnyAsync())
                {
                    await context.Priorities.AddRangeAsync(
                        new Priority { Name = "Normal" },
                        new Priority { Name = "Embarazo" },
                        new Priority { Name = "Incapacidad" },
                        new Priority { Name = "Tercera Edad" },
                        new Priority { Name = "Otros" }
                    );

                    await context.SaveChangesAsync();

                }
            }
        }
    }
}
