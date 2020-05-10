using Framework.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace Chan_UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient<IBoardService, BoardService>(client =>
            {
               // client.BaseAddress = new Uri("https://192.168.1.3:5002/api/post/");
                client.BaseAddress = new Uri("https://localhost:44317/api/post/");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                return handler;
            });
            services.AddHttpClient<IImageService, ImageService>(client =>
            {
              //  client.BaseAddress = new Uri("https://192.168.1.3:5002");
                client.BaseAddress = new Uri("https://localhost:44317");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                return handler;
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
