using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;
using Lykke.SettingsReader.Test.Models;

namespace Lykke.SettingsReader.Test
{
    public class ConfigurationTests
    {
        [Fact]
        public void TestPlainPropertiesByKey()
        {
            //Arrange
            const string strValue = "Test";
            DateTime dateValue = DateTime.UtcNow;
            const ConfigurationModelEnum enumValue = ConfigurationModelEnum.One;
            const string missingPropertyName = "Missing";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    StringProperty = strValue,
                    DateTimeProperty = dateValue,
                    NullableDateTimeProperty = dateValue,
                    EnumAsStringProperty = enumValue,
                    EnumAsIntProperty = enumValue,
                });

            //Assert
            Assert.Equal(configuration[nameof(ConfigurationModel.StringProperty)], strValue);
            Assert.Equal(configuration[nameof(ConfigurationModel.DateTimeProperty)], dateValue.ToString());
            Assert.Equal(configuration[nameof(ConfigurationModel.NullableDateTimeProperty)], dateValue.ToString());
            Assert.Equal(configuration[nameof(ConfigurationModel.EnumAsStringProperty)], enumValue.ToString());
            Assert.Equal(configuration[nameof(ConfigurationModel.EnumAsIntProperty)], ((int)enumValue).ToString());
            Assert.Null(configuration[missingPropertyName]);
        }

        [Fact]
        public void TestCollectionPropertiesByKey()
        {
            //Arrange
            const string expectedCollectionAsStr = "[\"1\",\"2\"]";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    ListProperty = new List<string> { "1", "2" },
                    ArrayProperty = new string[] { "1", "2" },
                });

            //Assert
            Assert.Equal(configuration[nameof(ConfigurationModel.ListProperty)], expectedCollectionAsStr);
            Assert.Equal(configuration[nameof(ConfigurationModel.ArrayProperty)], expectedCollectionAsStr);
        }

        [Fact]
        public void TestObjectPropertyByKey()
        {
            //Arrange
            const string strValue = "Test";
            string expectedValueAsStr = $"{{\"{nameof(ConfigurationModelSection1.StringProperty1)}\":\"{strValue}\"}}";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    Section1 = new ConfigurationModelSection1
                    {
                        StringProperty1 = strValue,
                    },
                }, true);

            //Assert
            Assert.Equal(configuration[nameof(ConfigurationModel.Section1)], expectedValueAsStr);
        }

        [Fact]
        public void TestPlainPropertiesBySection()
        {
            //Arrange
            const string strValue = "Test";
            DateTime dateValue = DateTime.UtcNow;
            const ConfigurationModelEnum enumValue = ConfigurationModelEnum.One;
            const string missingPropertyName = "Missing";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    StringProperty = strValue,
                    DateTimeProperty = dateValue,
                    NullableDateTimeProperty = dateValue,
                    EnumAsStringProperty = enumValue,
                    EnumAsIntProperty = enumValue,
                });

            //Assert
            var strSection = GetAndAssertSection(
                configuration,
                nameof(ConfigurationModel.StringProperty),
                nameof(ConfigurationModel.StringProperty),
                strValue);
            GetAndAssertSection(
                configuration,
                nameof(ConfigurationModel.DateTimeProperty),
                nameof(ConfigurationModel.DateTimeProperty),
                dateValue.ToString());
            GetAndAssertSection(
                configuration,
                nameof(ConfigurationModel.NullableDateTimeProperty),
                nameof(ConfigurationModel.NullableDateTimeProperty),
                dateValue.ToString());
            GetAndAssertSection(
                configuration,
                nameof(ConfigurationModel.EnumAsStringProperty),
                nameof(ConfigurationModel.EnumAsStringProperty),
                enumValue.ToString());
            GetAndAssertSection(
                configuration,
                nameof(ConfigurationModel.EnumAsIntProperty),
                nameof(ConfigurationModel.EnumAsIntProperty),
                ((int)enumValue).ToString());
            GetAndAssertSection(
                configuration,
                missingPropertyName,
                missingPropertyName,
                null);
            GetAndAssertSection(
                strSection,
                missingPropertyName,
                $"{nameof(ConfigurationModel.StringProperty)}:{ missingPropertyName}",
                null);
        }

        [Fact]
        public void TestCollectionPropertiesBySection()
        {
            //Arrange
            const string expectedCollectionAsStr = "[\"1\",\"2\"]";
            const string missingPropertyName = "Missing";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    ListProperty = new List<string> { "1", "2" },
                    ArrayProperty = new string[] { "1", "2" },
                });

            //Assert
            var listSection = GetAndAssertSection(
                configuration,
                nameof(ConfigurationModel.ListProperty),
                nameof(ConfigurationModel.ListProperty),
                expectedCollectionAsStr);
            GetAndAssertSection(
                configuration,
                nameof(ConfigurationModel.ArrayProperty),
                nameof(ConfigurationModel.ArrayProperty),
                expectedCollectionAsStr);
            GetAndAssertSection(
                listSection,
                missingPropertyName,
                $"{nameof(ConfigurationModel.ListProperty)}:{missingPropertyName}",
                null);
        }

        [Fact]
        public void TestObjectPropertyBySection()
        {
            //Arrange
            const string strValue = "Test";
            string expectedValueAsStr = $"{{\"{nameof(ConfigurationModelSection1.StringProperty1)}\":\"{strValue}\"}}";
            const string missingPropertyName = "Missing";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    Section1 = new ConfigurationModelSection1
                    {
                        StringProperty1 = strValue,
                    },
                }, true);

            //Assert
            var section1 = GetAndAssertSection(
                configuration,
                nameof(ConfigurationModel.Section1),
                nameof(ConfigurationModel.Section1),
                expectedValueAsStr);
            GetAndAssertSection(
                configuration,
                missingPropertyName,
                missingPropertyName,
                null);

            GetAndAssertSection(
                section1,
                nameof(ConfigurationModelSection1.StringProperty1),
                $"{nameof(ConfigurationModel.Section1)}:{nameof(ConfigurationModelSection1.StringProperty1)}",
                strValue);
            GetAndAssertSection(
                section1,
                missingPropertyName,
                $"{nameof(ConfigurationModel.Section1)}:{missingPropertyName}",
                null);
        }

        [Fact]
        public void TestPlainPropertiesByChildren()
        {
            //Arrange
            const string strValue = "Test";
            DateTime dateValue = DateTime.UtcNow;
            const ConfigurationModelEnum enumValue = ConfigurationModelEnum.One;

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    StringProperty = strValue,
                    DateTimeProperty = dateValue,
                    NullableDateTimeProperty = dateValue,
                    EnumAsStringProperty = enumValue,
                    EnumAsIntProperty = enumValue,
                });

            //Assert
            var sections = configuration.GetChildren();
            Assert.Equal(5, sections.Count());

            var strSection = sections.First(s => s.Key == nameof(ConfigurationModel.StringProperty));
            AssertSection(strSection, nameof(ConfigurationModel.StringProperty), strValue);
            var dateSection = sections.First(s => s.Key == nameof(ConfigurationModel.DateTimeProperty));
            AssertSection(dateSection, nameof(ConfigurationModel.DateTimeProperty), dateValue.ToString());
            var nullDateSection = sections.First(s => s.Key == nameof(ConfigurationModel.NullableDateTimeProperty));
            AssertSection(nullDateSection, nameof(ConfigurationModel.NullableDateTimeProperty), dateValue.ToString());
            var strEnumSection = sections.First(s => s.Key == nameof(ConfigurationModel.EnumAsStringProperty));
            AssertSection(strEnumSection, nameof(ConfigurationModel.EnumAsStringProperty), enumValue.ToString());
            var intEnumSection = sections.First(s => s.Key == nameof(ConfigurationModel.EnumAsIntProperty));
            AssertSection(intEnumSection, nameof(ConfigurationModel.EnumAsIntProperty), ((int)enumValue).ToString());
        }

        [Fact]
        public void TestPlainPropertyByChildren()
        {
            //Arrange
            const string strValue = "Test";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    StringProperty = strValue,
                }, true);

            //Assert
            var sections = configuration.GetChildren();
            Assert.Single(sections);

            var strSection = sections.First();
            AssertSection(strSection, nameof(ConfigurationModel.StringProperty), strValue);

            var childChildren = strSection.GetChildren();
            Assert.Empty(childChildren);
        }

        [Fact]
        public void TestCollectionPropertyByChildren()
        {
            //Arrange
            const string expectedCollectionAsStr = "[\"1\",\"2\"]";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    ListProperty = new List<string> { "1", "2" },
                }, true);

            //Assert
            var sections = configuration.GetChildren();
            Assert.Single(sections);

            var strSection = sections.First();
            AssertSection(strSection, nameof(ConfigurationModel.ListProperty), expectedCollectionAsStr);

            var childChildren = strSection.GetChildren();
            Assert.Empty(childChildren);
        }

        [Fact]
        public void TestObjectPropertyByChildren()
        {
            //Arrange
            const string strValue = "Test";
            string expectedValueAsStr = $"{{\"{nameof(ConfigurationModelSection1.StringProperty1)}\":\"{strValue}\"}}";

            var configuration = SetupTest(
                new ConfigurationModel
                {
                    Section1 = new ConfigurationModelSection1
                    {
                        StringProperty1 = strValue,
                    },
                }, true);

            //Assert
            var sections = configuration.GetChildren();
            Assert.Single(sections);

            var objSection = sections.First();
            AssertSection(objSection, nameof(ConfigurationModel.Section1), expectedValueAsStr);

            var lastLevelChildren = objSection.GetChildren();
            Assert.Single(lastLevelChildren);

            var strSection = lastLevelChildren.First();
            Assert.Empty(strSection.GetChildren());
        }

        private IConfigurationSection GetAndAssertSection(
            IConfiguration sectionParent,
            string key,
            string path,
            string value)
        {
            var section = sectionParent.GetSection(key);
            Assert.Equal(key, section.Key);
            Assert.Equal(path, section.Path);
            Assert.Equal(value, section.Value);
            return section;
        }

        private void AssertSection(
            IConfigurationSection section,
            string path,
            string value)
        {
            Assert.Equal(path, section.Path);
            Assert.Equal(value, section.Value);
        }

        private IConfiguration SetupTest(ConfigurationModel configurationModel, bool ignoreDefaults = false)
        {
            var manager = new ReloadingManagerForConfigurations(configurationModel, ignoreDefaults);
            return manager.SettingsConfiguration;
        }
    }
}
