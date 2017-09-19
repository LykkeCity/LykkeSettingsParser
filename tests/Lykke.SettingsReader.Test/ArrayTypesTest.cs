using System.Collections.Generic;
using Lykke.SettingsReader.Test.Models;
using Newtonsoft.Json;
using Xunit;
using System.Linq;

namespace Lykke.SettingsReader.Test
{
    public class ArrayTypesTest
    {
        [Fact(DisplayName = "Array: " + nameof(TestEnumArray))]
        public void TestEnumArray()
        {
            var data = new[] { TestEnum.SomeValue, TestEnum.SomeOtherValue, TestEnum.AndThirdValue };
            var value = JsonConvert.SerializeObject(data);
            var result = TestSettings<TestEnum[]>.FormatJson(value);

            for (var i = 0; i < data.Count(); i++)
                Assert.Equal(data[i], result.Value[i]);
        }

        [Fact(DisplayName = "Array: " + nameof(TestEnumList))]
        public void TestEnumList()
        {
            var data = new[] { TestEnum.SomeValue, TestEnum.SomeOtherValue, TestEnum.AndThirdValue };
            var value = JsonConvert.SerializeObject(data);
            var result = TestSettings<IList<TestEnum>>.FormatJson(value);

            for (var i = 0; i < data.Count(); i++)
                Assert.Equal(data[i], result.Value[i]);
        }

        [Fact(DisplayName = "Array: " + nameof(TestEnumCollection))]
        public void TestEnumCollection()
        {
            var data = new[] { TestEnum.SomeValue, TestEnum.AndThirdValue };
            var value = JsonConvert.SerializeObject(data);
            var result = TestSettings<ICollection<TestEnum>>.FormatJson(value);

            for (var i = 0; i < data.Count(); i++)
                Assert.True(result.Value.Contains(data[i]));
            Assert.False(result.Value.Contains(TestEnum.SomeOtherValue));
        }

        [Fact(DisplayName = "Array: " + nameof(TestDirectList))]
        public void TestDirectList()
        {
            var data = new[] { 123, 456, 789 };
            var value = JsonConvert.SerializeObject(data);
            var result = TestSettings<List<int>>.FormatJson(value);

            for (var i = 0; i < data.Count(); i++)
                Assert.Equal(data[i], result.Value[i]);
            Assert.False(result.Value.Contains(65756));
        }
    }
}
