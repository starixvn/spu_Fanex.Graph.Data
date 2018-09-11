namespace Fanex.Graph.Data
{
    using System.Collections.Generic;

    public interface ICommandExecutor<TDriver>
    {
        ExecutionResult<IEnumerable<T>> Execute<T>(TDriver driver, string command, object parameters = null);

        ExecutionResult Execute(TDriver driver, string command, object parameters = null);

        ExecutionResult<T> ExecuteSingle<T>(TDriver driver, string command, object parameters = null);
    }
}