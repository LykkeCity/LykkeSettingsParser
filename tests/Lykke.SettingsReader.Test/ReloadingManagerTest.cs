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
            Assert.Throws<ArgumentException>(() => servicesMock.Object.LoadSettings<TestModel>());
            servicesMock.Verify(x => x[SettingsConfiguratorExtensions.DefaultConfigurationKey], Times.Once());
        }

        public class TestSettings {
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

        public class TestReloadingManager : ReloadingManagerBase<object> {
            protected override async Task<object> Load() {
                await Task.Delay(100);
                return new object();
            }
        }

        [Fact]
        public void ReloadingManagerBase_Test()
        {
            //Arrange
            var manager = new TestReloadingManager();

            //Act / Assert

            var task1 = manager.Reload();
            var task2 = manager.Reload();
            Assert.Equal(task1, task2);

            var value1 = manager.CurrentValue;
            var value2 = manager.CurrentValue;
            Assert.Equal(value1, value2);

            var task3 = manager.Reload();
            Assert.NotEqual(task1, task3);

            var value3 = manager.CurrentValue;
            Assert.NotEqual(value1, value3);
        }
    }
}