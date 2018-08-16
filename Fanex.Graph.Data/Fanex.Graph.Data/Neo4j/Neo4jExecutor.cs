using Neo4j.Driver.V1;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Graph.Data.Neo4j
{
    public class Neo4jExecutor : ICommandExecutor<IDriver>
    {
        public IEnumerable<T> Query<T>(IDriver driver, string command, object parameters = null)
        {
            using (var session = driver.Session())
            {
                return session.WriteTransaction(tx =>
                {
                    var result = parameters is null ? tx.Run(command) : tx.Run(command, parameters);

                    return result.Select(ParseRecord<T>);
                });
            }
        }

        public void Query(IDriver driver, string command, object parameters = null)
        {
            using (var session = driver.Session())
            {
                session.WriteTransaction(tx => parameters is null ? tx.Run(command) : tx.Run(command, parameters));
            }
        }

        public T QuerySingle<T>(IDriver driver, string command, object parameters = null)
        {
            using (var session = driver.Session())
            {
                return session.WriteTransaction(tx =>
                {
                    var result = parameters is null ? tx.Run(command) : tx.Run(command, parameters);

                    return ParseRecord<T>(result.Single());
                });
            }
        }

        private T ParseRecord<T>(IRecord record) => JObject.FromObject(record.Values).ToObject<T>();
    }
}