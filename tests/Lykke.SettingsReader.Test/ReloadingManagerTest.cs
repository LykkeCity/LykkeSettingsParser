using System;
using System.Threading.Tasks;

using Lykke.SettingsReader.Test.Models;
using Microsoft.Extensions.Configuration;

using Moq;

using Xunit;

namespace Lykke.SettingsReader.Test
{
    public class ReloadingManagerTest
    {
        [Fact]
        public void LoadSettings_SettingUrlEmpty_Test()
        {
            //Arrange
            var servicesMock = new Mock<IConfiguration>();
            servicesMock.Setup(x => x[It.IsAny<string>()]).Returns("");

            //Act / Assert
            Assert.Throws<InvalidOperationException>(() => servicesMock.Object.LoadSettings<TestModel>());
            servicesMock.Verify(x => x[SettingsConfiguratorExtensions.DefaultConfigurationKey], Times.Once());
        }

        public class TestSettings
        {
            public string ConnectionString { get; set; }
        }

        [Fact]
        public void ConnectionStringReloadingManager_Test()
        {
            //Arrange
            var connectionString = "connectionString";
            var newConnectionString = "newConnectionString";

            Task<TestSettings> currentTask = null;

            var rootSettingsMock = new Mock<IReloadingManager<TestSettings>>();
            rootSettingsMock
                .Setup(x => x.Reload())
                .Returns(() => currentTask = Task.FromResult(new TestSettings { ConnectionString = connectionString }));

            rootSettingsMock
                .Setup(x => x.CurrentValue)
                .Returns(() => (currentTask ?? rootSettingsMock.Object.Reload()).Result);

            var connectionStringManager = rootSettingsMock.Object.ConnectionString(x => x.ConnectionString);

            //Act / Assert

            var firstValue = connectionStringManager.CurrentValue;
            rootSettingsMock.Verify(x => x.Reload(), Times.Once());
            Assert.Equal(connectionString, firstValue);

            var secondValue = connectionStringManager.CurrentValue;
            rootSettingsMock.Verify(x => x.Reload(), Times.Once());
            Assert.Equal(connectionString, secondValue);

            connectionString = newConnectionString;
            Assert.Equal(newConnectionString, rootSettingsMock.Object.Reload().Result.ConnectionString);
            rootSettingsMock.Verify(x => x.Reload(), Times.Exactly(2));

            var thirdValue = connectionStringManager.Reload().Result;
            rootSettingsMock.Verify(x => x.Reload(), Times.Exactly(2));
            Assert.Equal(newConnectionString, thirdValue);

            var fourthValue = connectionStringManager.Reload().Result;
            rootSettingsMock.Verify(x => x.Reload(), Times.Exactly(3));
            Assert.Equal(newConnectionString, fourthValue);
        }

        [Fact]
        public void ReloadingManager_ShouldThrowUnwrappedException()
        {
            var reloadingManager = new ExceptionalReloadingManager();
            Assert.Throws<InvalidOperationException>(() => reloadingManager.CurrentValue);
        }

        private sealed class ExceptionalReloadingManager : ReloadingManagerBase<string>
        {
            protected override Task<string> Load()
            {
                return Task.FromException<string>(new InvalidOperationException("Hi!"));
            }
        }
    }
}