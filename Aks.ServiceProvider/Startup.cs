using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aks.ServiceProvider.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aks.ServiceProvider
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
            services.AddOptions().Configure<Connections>(Configuration.GetSection("Connections"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<BookService>();
//            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IBookService>(sp =>
            {
                var bookService = sp.GetRequiredService<BookService>();
                var distributedCache = sp.GetRequiredService<IDistributedCache>();

                return new BookServiceDecorator(bookService, distributedCache);
            });

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["Connections:Redis"];
                options.InstanceName = "Aks";
            });
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
