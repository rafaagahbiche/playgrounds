using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Checkins.Services;
using Playgrounds.Services;
using Photos.Services.ThirdPartyManagers;
using Photos.Services.Managers;
using Photos.Infrastructure.Uploader;
using Auth.Infrastructure.PasswordStuff;
using Auth.Services;

namespace PlaygroundsGallery.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<GalleryContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<GalleryContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            ConfigureServices(services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var cloudinaryAccount = Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            var tokenSecretKey = Configuration.GetSection("AppSettings:Token").Value;
            services.AddAutoMapper(
                typeof(Startup), 
                typeof(Photos.Services.DTOs.PhotoEntityAutoMapperProfile),
                typeof(Playgrounds.Services.PlaygroundEntityAutoMapperProfile),
                typeof(Auth.Services.ManagerEntityAutoMapperProfile),
                typeof(Checkins.Services.CheckinEntityAutoMapperProfile));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IAccountSettings, CloudinarySettings>();
            services.AddScoped<IPhotoUploader, CloudinaryPhotoUploader>(_ =>
                new CloudinaryPhotoUploader(cloudinaryAccount));
            services.AddScoped<ITokenManager, TokenCreator>(_ => new TokenCreator(tokenSecretKey));
            services.AddScoped<IRepository<Photo>, Repository<Photo>>();
            services.AddScoped<IRepository<Member>, Repository<Member>>();
            services.AddScoped<IRepository<Playground>, Repository<Playground>>();
            services.AddScoped<IRepository<Location>, Repository<Location>>();
            services.AddScoped<IRepository<CheckIn>, Repository<CheckIn>>();
            services.AddScoped<IMemberManager, MemberManager>();
            services.AddScoped<IPasswordManager, PasswordManager>();
            services.AddScoped<IPlaygroundManager, PlaygroundManager>();
            services.AddScoped<IThirdPartyStorageManager, ThirdPartyStorageManager>();
            services.AddScoped<IPhotoManager, PhotoManager>();
            services.AddScoped<ICheckinManager, CheckinManager>();
            services.AddScoped<IGalleryContext, GalleryContext>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(tokenSecretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
                // .AddGoogle(googleOptions =>  
                //     {  
                //         googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];  
                //         googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];  
                //     })
                // .AddTwitter(twitterOptions =>
                // {
                //     twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerAPIKey"];
                //     twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                // })
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
                app.UseExceptionHandler(builder => 
                {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            // context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapSpaFallbackRoute(
                    name: "spafallback",
                    defaults: new  { controller = "Fallback", action = "Index"}
                );
            });
        }
    }
}
