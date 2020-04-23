using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Checkins.Services;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PlaygroundsGallery.DataEF.Models;

namespace Checkin.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper(
                typeof(Startup), 
                typeof(Checkins.Services.CheckinEntityAutoMapperProfile));
            services.AddDbContext<GalleryContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICheckinManager, CheckinManager>();
            services.AddScoped<ICheckinMember, CheckinMember>();
            services.AddScoped<ILocationCheckinsSchedule, LocationCheckinsSchedule>();
            services.AddScoped<IPlaygroundCheckinsSchedule, PlaygroundCheckinsSchedule>();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
