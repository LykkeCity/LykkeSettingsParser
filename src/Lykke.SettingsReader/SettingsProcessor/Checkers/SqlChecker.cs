using System.Reflection;
using System.Data.SqlClient;
using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader.Checkers
{
    internal class SqlChecker : ISettingsFieldChecker
    {
        public CheckFieldResult CheckField(object model, PropertyInfo property, object value)
        {
            if (value == null)
                throw new CheckFieldException(property.Name, value, "Setting can not be null");

            string val = value.ToString();
            if (string.IsNullOrWhiteSpace(val))
                throw new CheckFieldException(property.Name, value, "Empty setting value");

            string url = string.Empty;
            try
            {
                using (var connection = new SqlConnection(val))
                {
                    url = connection.DataSource;
                    connection.Open();
                }
                return CheckFieldResult.Ok(url);
            }
            catch
            {
                return CheckFieldResult.Failed(url);
            }
        }
    }
}
