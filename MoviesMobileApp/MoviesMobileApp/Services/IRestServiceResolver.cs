namespace MoviesMobileApp.Services
{
    public interface IRestServiceResolver
    {
        TApi For<TApi>();
    }
}