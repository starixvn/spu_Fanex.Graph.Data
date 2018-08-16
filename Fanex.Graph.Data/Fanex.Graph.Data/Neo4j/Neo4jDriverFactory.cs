using Neo4j.Driver.V1;
using System;

namespace Fanex.Graph.Data.Neo4j
{
    public class Neo4jDriverFactory : IDriverFactory
    {
        public (Type driverType, object driver) GetDriver(string config)
        {
            var configs = config.Split(';');

            return (typeof(IDriver), GraphDatabase.Driver(configs[0], AuthTokens.Basic(configs[1], configs[2])));
        }
    }
}