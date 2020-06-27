using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace Roshambo.GettingStarted
{
    class GameListener
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<GameEngine>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Map("/turn", appBuilder =>
            {
                appBuilder.UseMiddleware<GameMiddleware>();
            });
        }
    }
}
