using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.FileAccess;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KooliProjekt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddControllersWithViews();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped<IStorageRepository, StorageRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ISongScheduleRepository, SongScheduleRepository>();

            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IStorageService, StorageService>();

            services.AddScoped<IFileClient, FileClient>();

            services.AddAutoMapper(GetType().Assembly);

            //services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            EnsureDatabase(app);
        }

        private void EnsureDatabase(IApplicationBuilder app)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            using (var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
            {
                var scheduleService = serviceScope.ServiceProvider.GetService<IScheduleService>();
                dbContext.Database.EnsureCreated();

                DemoData.Initialize(dbContext, scheduleService);
            }
        }
    }
}