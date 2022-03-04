using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
#if NETCOREAPP3_0_OR_GREATER
using Microsoft.Extensions.Hosting;
#endif
using Microsoft.Extensions.Primitives;

namespace Sample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
#if NETCOREAPP2_2
            services.AddMvc();
#endif
#if NETCOREAPP3_0_OR_GREATER
            services.AddControllers();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app,
#if NETCOREAPP2_2
            IHostingEnvironment env
#endif
#if NETCOREAPP3_0_OR_GREATER
            IWebHostEnvironment env
#endif
            )
        {
            app.Use(async (context, next) =>
            {
                const string XCorrelationIdHeaderName = "X-Correlation-ID";
                if (!context.Request.Headers.TryGetValue(XCorrelationIdHeaderName, out StringValues correlationId))
                {
                    correlationId = Guid.NewGuid().ToString();
                }

                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(XCorrelationIdHeaderName))
                    {
                        context.Response.Headers.Add(XCorrelationIdHeaderName, correlationId);
                    }
                    return Task.CompletedTask;
                });

                await next.Invoke();
            });

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
#if NETCOREAPP2_2
            app.UseMvc();
#endif
#if NETCOREAPP3_0_OR_GREATER
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
#endif
        }
    }
}
