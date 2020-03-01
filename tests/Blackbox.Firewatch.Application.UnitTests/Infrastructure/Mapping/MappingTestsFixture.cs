using AutoMapper;
using Blackbox.Firewatch.Application.Infrastructure.Mapping;

namespace Blackbox.Firewatch.Application.UnitTests.Infrastructure.Mapping
{
    public class MappingTestsFixture
    {
        public MappingTestsFixture()
        {
            ConfigurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = ConfigurationProvider.CreateMapper();
        }

        public IConfigurationProvider ConfigurationProvider { get; }

        public IMapper Mapper { get; }
    }
}