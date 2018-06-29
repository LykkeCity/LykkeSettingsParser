using System;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

namespace Lykke.SettingsReader.Checkers
{
    internal class MongoChecker : ISettingsFieldChecker
    {
        private readonly bool _throwExceptionOnFail;

        internal MongoChecker(bool throwExceptionOnFail)
        {
            _throwExceptionOnFail = throwExceptionOnFail;
        }

        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            try
            {
                var mongoUrl = MongoUrl.Create(value);

                var client = new MongoClient(mongoUrl);
                
                client.ListDatabases();
                
                return client.Cluster.Description.State == ClusterState.Connected
                    ? CheckFieldResult.Ok(propertyName, value)
                    : CheckFieldResult.Failed(propertyName, value, _throwExceptionOnFail);
            }
            catch (MongoConfigurationException)
            {
                return CheckFieldResult.Failed(propertyName, value, _throwExceptionOnFail);
            }
            catch (TimeoutException)
            {
                return CheckFieldResult.Failed(propertyName, value, _throwExceptionOnFail);
            }
        }
    }
}