namespace Lykke.SettingsReader
{
    /// <summary>
    /// Interface for settings field check
    /// </summary>
    public interface ISettingsFieldChecker
    {
        /// <summary>
        /// Check of field value with additional data from model
        /// </summary>
        /// <param name="model">Parent data model</param>
        /// <param name="propertyName">Field name</param>
        /// <param name="value">Field value</param>
        /// <returns></returns>
        CheckFieldResult CheckField(object model, string propertyName, string value);
    }
}
