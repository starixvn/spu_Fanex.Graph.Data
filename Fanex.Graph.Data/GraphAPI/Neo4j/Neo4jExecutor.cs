using Fanex.Graph.Data;
using Neo4j.Driver.V1;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAPI.Neo4j
{
    public class Neo4jExecutor : ICommandExecutor<IDriver>
    {
        public IEnumerable<T> Execute<T>(IDriver driver, string command, object parameters = null)
        {
            using (var session = driver.Session())
            {
                return session.WriteTransaction(tx =>
                {
                    var result = parameters is null ? tx.Run(command) : tx.Run(command, parameters);

                    return result.Select(ParseRecord<T>).ToArray();
                });
            }
        }

        public void Execute(IDriver driver, string command, object parameters = null)
        {
            using (var session = driver.Session())
            {
                session.WriteTransaction(tx => parameters is null ? tx.Run(command) : tx.Run(command, parameters));
            }
        }

        public T ExecuteSingle<T>(IDriver driver, string command, object parameters = null)
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

        private T ParseRecord<T>(IRecord record)
        {
            var t = typeof(T);

            if (t.IsPrimitive || t == typeof(string))
            {
                return record[0].As<T>();
            }
            else if (t == typeof(DateTime))
            {
                object obj = DateTime.Parse(record[0].As<string>());

                return (T)obj;
            }

            return JObject.FromObject(record.Values).ToObject<T>();
        }
    }
}