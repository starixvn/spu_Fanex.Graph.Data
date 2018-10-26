using GraphAPI.Factory;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace GraphAPI
{
    public class ExceptionLogger : IExceptionLogger
    {
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            Logger.Error(context.Exception.Message, context.Exception);

            return Task.CompletedTask;
        }
    }
}