using System;
using System.Globalization;
using Lykke.SettingsReader.Test.Models;
using Xunit;

namespace Lykke.SettingsReader.Test
{
    public class ValueTypesTest
    {
        [Theory(DisplayName = "Value:" + nameof(TestEnum))]
        [InlineData(nameof(Models.TestEnum.SomeValue))]
        [InlineData(nameof(Models.TestEnum.SomeOtherValue))]
        [InlineData(nameof(Models.TestEnum.AndThirdValue))]
        public void TestEnum(string value)
        {
            var result = TestSettings<TestEnum>.FormatJsonString(value.ToLower());
            Assert.Equal(Enum.Parse(typeof(TestEnum), value), result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestEnumN))]
        [InlineData(null)]
        [InlineData(nameof(Models.TestEnum.SomeValue))]
        [InlineData(nameof(Models.TestEnum.SomeOtherValue))]
        [InlineData(nameof(Models.TestEnum.AndThirdValue))]
        public void TestEnumN(string value)
        {
            var result = TestSettings<TestEnum?>.FormatJsonString(value);
            Assert.Equal(null == value ? null : Enum.Parse(typeof(TestEnum), value), result.Value);
        }

        [Theory(DisplayName = "Value: " + nameof(TestTimeSpan))]
        [InlineData("5")]
        [InlineData("20:37")]
        [InlineData("20:37:16")]
        [InlineData("0:0:16.23")]
        public void TestTimeSpan(string value)
        {
            var result = TestSettings<TimeSpan>.FormatJsonString(value);
            Assert.Equal(TimeSpan.Parse(value), result.Value);
        }

        [Theory(DisplayName = "Value: " + nameof(TestTimeSpanN))]
        [InlineData(null)]
        [InlineData("5")]
        [InlineData("20:37")]
        [InlineData("20:37:16")]
        [InlineData("0:0:16.23")]
        public void TestTimeSpanN(string value)
        {
            var result = TestSettings<TimeSpan?>.FormatJsonString(value);
            Assert.Equal(null == value ? (object)null : TimeSpan.Parse(value), result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestDateTime))]
        [InlineData("2017-01-01T22:00")]
        public void TestDateTime(string value)
        {
            var result = TestSettings<DateTime>.FormatJsonString(value);
            Assert.Equal(DateTime.Parse(value), result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestDateTimeN))]
        [InlineData(null)]
        [InlineData("2017-01-01T22:00")]
        public void TestDateTimeN(string value)
        {
            var result = TestSettings<DateTime?>.FormatJsonString(value);
            Assert.Equal(null == value ? (object)null : DateTime.Parse(value), result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestDoubleS))]
        [InlineData("0.2")]
        public void TestDoubleS(string value)
        {
            var result = TestSettings<double>.FormatJsonString(value);
            Assert.Equal(double.Parse(value, CultureInfo.InvariantCulture), result.Value);
        }

        
        
        [Theory(DisplayName = "Value:" + nameof(TestGuid))]
        [InlineData("b2f1216a-a792-4d08-8a7a-f51c2304eb4c")]
        [InlineData("{b0e48349-be76-4785-bda2-d938c5553ef0}")]
        public void TestGuid(string value)
        {
            var result = TestSettings<Guid>.FormatJsonString(value);
            Assert.Equal(Guid.Parse(value), result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestGuidN))]
        [InlineData(null)]
        [InlineData("b2f1216a-a792-4d08-8a7a-f51c2304eb4c")]
        [InlineData("{b0e48349-be76-4785-bda2-d938c5553ef0}")]
        public void TestGuidN(string value)
        {
            var result = TestSettings<Guid?>.FormatJsonString(value);
            Assert.Equal(null == value ? (object)null : Guid.Parse(value), result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestInt))]
        [InlineData(0)]
        [InlineData(-37)]
        [InlineData(3465)]
        public void TestInt(int value)
        {
            var result = TestSettings<int>.FormatJson(value.ToString());
            Assert.Equal(value, result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestIntS))]
        [InlineData(0)]
        [InlineData(-37)]
        [InlineData(3465)]
        public void TestIntS(int value)
        {
            var result = TestSettings<int>.FormatJsonString(value.ToString());
            Assert.Equal(value, result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestIntN))]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-37)]
        [InlineData(3465)]
        public void TestIntN(int? value)
        {
            var result = TestSettings<int?>.FormatJson(value?.ToString());
            Assert.Equal(value, result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestBool))]
        [InlineData(true)]
        [InlineData(false)]
        public void TestBool(bool value)
        {
            var result = TestSettings<bool>.FormatJson(value.ToString().ToLower());
            Assert.Equal(value, result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestBoolS))]
        [InlineData(true)]
        [InlineData(false)]
        public void TestBoolS(bool value)
        {
            var result = TestSettings<bool>.FormatJsonString(value.ToString());
            Assert.Equal(value, result.Value);
        }

        [Theory(DisplayName = "Value:" + nameof(TestBoolN))]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        public void TestBoolN(bool? value)
        {
            var result = TestSettings<bool?>.FormatJson(value?.ToString().ToLower());
            Assert.Equal(value, result.Value);
        }
    }
}
