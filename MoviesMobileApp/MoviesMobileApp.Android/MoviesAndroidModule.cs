using DryIoc;
using MoviesMobileApp.DependencyInjection;

namespace MoviesMobileApp.Droid
{
    public class MoviesAndroidModule : IModule
    {
        public void Load(IRegistrator builder)
        {
            builder.RegisterMany<MoviesModule>();
        }
    }
}