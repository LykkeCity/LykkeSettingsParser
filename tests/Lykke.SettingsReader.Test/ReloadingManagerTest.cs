using System;
using System.Threading.Tasks;

using AzureStorage.Tables.Decorators;

using Common.Log;

using Lykke.SettingsReader.Test.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Table;

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
            var servicesMock = new Mock<IServiceCollection>();

            //Act / Assert
            Assert.Throws<SettingsSourceException>(() => servicesMock.Object.LoadSettings<TestModel>(""));
            servicesMock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Never());
        }

        [Fact]
        public void LoadSettings_Test()
        {
            //Arrange
            var servicesMock = new Mock<IServiceCollection>();
            ServiceDescriptor addedDescriptor = null;
            servicesMock
                .Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Callback<ServiceDescriptor>(item => addedDescriptor = item);

            //Act
            servicesMock.Object.LoadSettings<TestModel>("Url");

            // Assert
            servicesMock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
            Assert.Equal(typeof(SettingsServiceReloadingManager<TestModel>), addedDescriptor?.ImplementationInstance?.GetType());
        }

        [Fact]
        public void LoadLocalSettings_Test()
        {
            //Arrange
            var servicesMock = new Mock<IServiceCollection>();
            ServiceDescriptor addedDescriptor = null;
            servicesMock
                .Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Callback<ServiceDescriptor>(item => addedDescriptor = item);

            //Act
            servicesMock.Object.LoadLocalSettings<TestModel>("Url");

            // Assert
            servicesMock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
            Assert.Equal(typeof(LocalSettingsReloadingManager<TestModel>), addedDescriptor?.ImplementationInstance?.GetType());
        }

        [Fact]
        public void AddTableStorage_Test()
        {
            //Arrange
            var servicesMock = new Mock<IServiceCollection>();
            ServiceDescriptor addedDescriptor = null;
            servicesMock
                .Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Callback<ServiceDescriptor>(item => addedDescriptor = item);

            var servicesProviderMock = new Mock<IServiceProvider>();
            Type resolvedService = null;
            servicesProviderMock
                .Setup(x => x.GetService(It.IsAny<Type>()))
                .Callback<Type>(item => resolvedService = item);

            var reloadingMock = new Mock<IReloadingManager<string>>();
            reloadingMock
                .Setup(x => x.Reload())
                .Returns(() => Task.FromResult("connectionString"));

            //Act / Assert
            servicesMock.Object.AddTableStorage<TableEntity>(reloadingMock.Object, "tabble");
            servicesMock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());

            var tableSorage = addedDescriptor?.ImplementationFactory?.Invoke(servicesProviderMock.Object);
            Assert.Equal(typeof(ReloadingConnectionStringOnFailureAzureTableStorageDecorator<TableEntity>), tableSorage?.GetType());

            servicesProviderMock.Verify(x => x.GetService(It.IsAny<Type>()), Times.Once());
            Assert.Equal(typeof(ILog), resolvedService);
        }
    }
}