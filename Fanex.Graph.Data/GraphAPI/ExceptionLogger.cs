using Fanex.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace GraphAPI
{
    public class ExceptionLogger : IExceptionLogger
    {
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
#if DEBUG
            throw context.Exception;
#endif

            Logger.Log.Error(context.Exception.Message, context.Exception);

            return Task.CompletedTask;
        }
    }
}