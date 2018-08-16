namespace Fanex.Graph.Data
{
    using System.Collections.Generic;

    public interface ICommandExecutor<TDriver>
    {
        IEnumerable<T> Execute<T>(TDriver driver, string command, object parameters = null);

        void Execute(TDriver driver, string command, object parameters = null);

        T ExecuteSingle<T>(TDriver driver, string command, object parameters = null);
    }
}