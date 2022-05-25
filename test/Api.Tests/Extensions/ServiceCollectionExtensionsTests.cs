using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly ServiceCollection _services;

        public ServiceCollectionExtensionsTests()
        {
            this._services = new ServiceCollection();
        }

        [Fact]
        public void AddSettingsDbContextMdiExtended_NullArgumentsPassed_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>("services", () => ServiceCollectionExtensions.AddPgDbContext(null, null));
            Assert.Throws<ArgumentException>("connectionString", () => ServiceCollectionExtensions.AddPgDbContext(this._services, null));
            Assert.Throws<ArgumentException>("connectionString", () => ServiceCollectionExtensions.AddPgDbContext(this._services, string.Empty));
            _ = this._services.AddPgDbContext("Foo");
        }

        /*[Fact]
        public void AddSettingsDbContextMdiExtended_ValidConfigPassed_RequiredServicesAdded()
        {
            // Arrange
            var config = new ConfigurationBuilder().Build();

            // Act
            this._services.AddPgDbContext("connectionString");
        }*/
    }
}