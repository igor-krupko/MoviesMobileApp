using AutoMapper;

namespace MoviesMobileApp.Mappers
{
    public class MappingConfigurator
    {
        private static MapperConfiguration configuration;

        private static IMapper mapper;
        public static IMapper Mapper => mapper ?? (mapper = configuration.CreateMapper());

        public static MapperConfiguration Configure()
        {
            configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<MoviesProfile>();
            });
            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}