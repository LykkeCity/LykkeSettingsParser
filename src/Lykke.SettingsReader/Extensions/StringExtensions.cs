namespace Lykke.SettingsReader.Extensions
{
    internal static class StringExtensions
    {
        internal static bool SplitParts(this string value, char splitChar, int length, out string[] values)
        {
            values = value.Split(splitChar);
            return values != null && values.Length == length;
        }
    }
}
