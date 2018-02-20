namespace Checkout.Scrutinizer.UI
{
    using System.Threading.Tasks;
   
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using ElectronNET.API;

    using Checkout.Scrutinizer.UI.Utilities;
    using ElectronNET.API.Entities;
    using System;

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
            services.AddMvc();

            services.AddScoped<IViewRenderingService, ViewRenderingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Open the Electron-Window here
            ElectronBootstrap(env.WebRootPath);
        }

        public async void ElectronBootstrap(string webRootPath)
        {
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Show = false,
                Icon = webRootPath + "/favicon.png",
                Title = "Checkout.Scruntinizer"
            });

            browserWindow.OnReadyToShow += () => browserWindow.Show();
        }
    }
}
