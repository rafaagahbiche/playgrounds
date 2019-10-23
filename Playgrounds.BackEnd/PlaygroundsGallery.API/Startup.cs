using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.Helper;
using AutoMapper;
using PlaygroundsGallery.DataEF.Repositories;
using PlaygroundsGallery.Domain.Repositories;
using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Domain.Managers;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        public void ConfigureServices(IServiceCollection services)
        {
            var cloudinaryAccount = Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            var tokenSecretKey = Configuration.GetSection("AppSettings:Token").Value;
            
            services.AddDbContext<GalleryContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IAccountSettings, CloudinarySettings>();
            services.AddScoped<IPhotoUploader, CloudinaryPhotoUploader>(_ =>
                new CloudinaryPhotoUploader(cloudinaryAccount));
            services.AddScoped<ITokenManager, TokenCreator>(_ => new TokenCreator(tokenSecretKey));
            services.AddScoped<IRepository<Photo>, Repository<Photo>>();
            services.AddScoped<IRepository<Member>, Repository<Member>>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IPasswordManager, PasswordManager>();
            services.AddScoped<IFrontManager, FrontManager>();
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
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();
        }
    }
}
