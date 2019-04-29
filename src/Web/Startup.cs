using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Models.IdentityError;
using ApplicationCore.Entities;
using Microsoft.QueueManager.Infrastructure.Data;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Microsoft.QueueManager.Infrastructure.Logging;
using AutoMapper;
using Web.Profiles;
using Web.Interfaces;
using Web.ViewModels;
using Web.Services;
using Web.Hubs;
using Web.Extensions;
using Web.Models;
using Rotativa.AspNetCore;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<QueueContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("QueueConnection")));

            ConfigureServices(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<QueueContext>(options => 
                options.UseInMemoryDatabase("Queue"));

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAutoMapper();
            Mapper.Initialize(cfg => cfg.AddProfile<AppMapperProfile>());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<QueueContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<IdentityError_es>();

            services.Configure<IdentityOptions>(config =>
            {
                //Password settings
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;

            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IOperatorService, OperatorService>();
            services.AddScoped<IDisplayMediaService, DisplayMediaService>();

            services.AddScoped<IOfficeViewModel, OfficeViewModelService>();
            services.AddScoped<ITaskIndexViewModel, TaskViewModelService>();
            services.AddScoped<IMediaViewModel, MediaViewModelService>();
            services.AddScoped<IAddTasksOperatorsToNewOfficeViewModel, AddTasksOperatorsToNewOfficeViewModelService>();

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.ConfigureWritable<DisplayTickets>(Configuration.GetSection("DisplayTickets"), "websettings.json");
            services.ConfigureWritable<DisplayCustom>(Configuration.GetSection("DisplayCustom"), "websettings.json");
            services.ConfigureWritable<TouchCustom>(Configuration.GetSection("TouchCustom"), "websettings.json");

            services.AddAntiforgery(options => options.HeaderName = "MY-XSRF-TOKEN");

            services.AddSignalR();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<QueueHub>("/queueHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            RotativaConfiguration.Setup(env);
        }
    }
}
