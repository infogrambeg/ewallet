using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Contracts.ServiceInterfaces;
using ApplicationServices;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAppPoc
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
            services.AddControllers();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ICoreUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IWalletService,WalletService>();

            services.AddDbContext<EfCoreDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PosgtreConnection")));

            //services.AddDbContext<EfCoreDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("MSSQLConnection")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EfCoreDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
           
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
