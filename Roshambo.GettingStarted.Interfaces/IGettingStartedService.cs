using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Roshambo.GettingStarted.Interfaces
{
    public interface IGettingStartedService: IService
    {
        Task DoSomething();
    }
}
