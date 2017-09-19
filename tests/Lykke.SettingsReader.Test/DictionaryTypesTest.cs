using System;
using System.Collections.Generic;
using Lykke.SettingsReader.Test.Models;
using Newtonsoft.Json;
using Xunit;

namespace Lykke.SettingsReader.Test
{
    public class DictionaryTypesTest
    {
        [Fact(DisplayName = "Dictionary: " + nameof(TestEnumDictionary))]
        public void TestEnumDictionary()
        {
            var data = new Dictionary<TestEnum, string>
            {
                {TestEnum.SomeValue, "test some value" },
                {TestEnum.SomeOtherValue, "test some other value" },
                {TestEnum.AndThirdValue, "and third value is here" }
            };

            var value = JsonConvert.SerializeObject(data);
            var result = TestSettings<IDictionary<TestEnum, string>>.FormatJson(value);

            foreach (var item in data)
                Assert.Equal(item.Value, result.Value[item.Key]);
        }

        [Fact(DisplayName = "Dictionary: " + nameof(TestDateTimeDictionary))]
        public void TestDateTimeDictionary()
        {
            var data = new Dictionary<DateTime, string>
            {
                {DateTime.Now, "something that is going now" },
                {DateTime.UtcNow.AddDays(1), "something that is goint to be tomorrow" },
                {DateTime.MinValue, "a long time ago in a galaxy far, far away..." }
            };

            var value = JsonConvert.SerializeObject(data);
            var result = TestSettings<IDictionary<DateTime, string>>.FormatJson(value);

            foreach(var item in data)
                Assert.Equal(item.Value, result.Value[item.Key]);
        }

        [Fact(DisplayName = "Dictionary: " + nameof(TestDirectDictionary))]
        public void TestDirectDictionary()
        {
            var data = new Dictionary<DateTime, string>
            {
                {DateTime.Now, "something that is going now" },
                {DateTime.UtcNow.AddDays(1), "something that is goint to be tomorrow" },
                {DateTime.MinValue, "a long time ago in a galaxy far, far away..." }
            };

            var value = JsonConvert.SerializeObject(data);
            var result = TestSettings<Dictionary<DateTime, string>>.FormatJson(value);

            foreach (var item in data)
                Assert.Equal(item.Value, result.Value[item.Key]);
        }
    }
}
