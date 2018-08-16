namespace Fanex.Graph.Data
{
    using System.Collections.Generic;

    public interface ICommandExecutor<TDriver>
    {
        IEnumerable<T> Query<T>(TDriver driver, string command, object parameters = null);

        void Query(TDriver driver, string command, object parameters = null);

        T QuerySingle<T>(TDriver driver, string command, object parameters = null);
    }
}