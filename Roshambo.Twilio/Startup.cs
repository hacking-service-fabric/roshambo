using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Roshambo.Common;
using Roshambo.Twilio.Implementations;
using Twilio.Security;

namespace Roshambo.Twilio
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
            services
                .AddTranslationService()
                .AddPlayerSession()
                .AddGameService();

            services
                .AddHttpContextAccessor()
                .AddScoped<IRequestDataProvider, RequestDataProvider>();

            services.AddSingleton(new RequestValidator(
                Configuration.GetValue<string>("TwilioAuthToken")));

            services
                .AddTransient<TwilioValidationMiddleware>()
                .AddTransient<TwilioMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<TwilioValidationMiddleware>();
            app.UseMiddleware<TwilioMiddleware>();
        }
    }
}
