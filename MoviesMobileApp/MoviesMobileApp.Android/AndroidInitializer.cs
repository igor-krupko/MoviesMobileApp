using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Prism;
using Prism.Ioc;

namespace MoviesMobileApp.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterSingleton<IConnectivity, ConnectivityImplementation>();
        }
    }
}