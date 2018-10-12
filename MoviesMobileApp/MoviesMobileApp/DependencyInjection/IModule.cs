using DryIoc;

namespace MoviesMobileApp.DependencyInjection
{
    public interface IModule
    {
        void Load(IRegistrator builder);
    }
}
