using System.Data.SqlClient;

namespace Lykke.SettingsReader.Checkers
{
    internal class SqlChecker : ISettingsFieldChecker
    {
        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            //TODO use   var sb = new SqlConnectionStringBuilder(value); to parse the connection string
            string url = string.Empty;
            try
            {
                using (var connection = new SqlConnection(value))
                {
                    url = connection.DataSource;
                    connection.Open();
                }
                return CheckFieldResult.Ok(propertyName, url);
            }
            catch
            {
                return CheckFieldResult.Failed(propertyName, url);
            }
        }
    }
}
