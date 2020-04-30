using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlaygroundsGallery.DataEF;
using Playgrounds.Services;
using System.Text;
using System.Net;
using Serilog;
using PlaygroundsGallery.API;

namespace Playgrounds.FunctionalTests
{
    public class PlaygroundsTestStartup: Startup
    {
        public PlaygroundsTestStartup(IConfiguration configuration)
            :base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void ConfigAuth(IApplicationBuilder app)
        {
        }   

        public override void ConfigureAuthService(IServiceCollection services)
        {
        }

        public override void ConfigurePhotoService(IServiceCollection services)
        {
        }
    }
}