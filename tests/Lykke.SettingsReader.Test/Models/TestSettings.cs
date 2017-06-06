namespace Lykke.SettingsReader.Test.Models
{
    internal class TestSettings<TValue>
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public TValue Value { get; set; }

        public static TestSettings<TValue> FormatJsonString(string value)
        {
            if (null == value)
                return SettingsProcessor.Process<TestSettings<TValue>>("{ 'value': null }");

            value = value
                .Replace(@"\", @"\\")
                .Replace(@"""", @"\\""")
                .Replace(@"'", @"\\'");
            value = "\"" + value + "\"";
            return FormatJson(value);
        }

        public static TestSettings<TValue> FormatJson(string value)
        {
            return null == value
                ? SettingsProcessor.Process<TestSettings<TValue>>("{ 'value': null }")
                : SettingsProcessor.Process<TestSettings<TValue>>("{ 'value': " + value + " }");
        }
    }
}