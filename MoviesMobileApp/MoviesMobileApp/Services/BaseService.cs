using System;
using System.Net;
using System.Threading.Tasks;
using Akavache;
using Polly;

namespace MoviesMobileApp.Services
{
    public abstract class BaseService
    {
        private const int DefaultRequestRetryCount = 3;
        private static readonly TimeSpan DefaultRequestDelay = TimeSpan.FromSeconds(2);
        
        protected IBlobCache Cache => BlobCache.LocalMachine;

        protected static readonly Policy DefaultPolicy = Policy
            .Handle<Exception>(IsNetworkException)
            .WaitAndRetryAsync(DefaultRequestRetryCount, i => DefaultRequestDelay);

        protected BaseService()
        {
            BlobCache.ApplicationName = ApplicationConfiguration.AppName;
        }

        protected Func<Task<T>> WithDefaultPolicy<T>(Func<Task<T>> func)
        {
            return WithPolicy(func, DefaultPolicy);
        }

        protected Func<Task<T>> WithPolicy<T>(Func<Task<T>> func, Policy policy)
        {
            return () => Execute(func, policy);
        }

        protected async Task<T> Execute<T>(Func<Task<T>> func, Policy policy = null)
        {
            try
            {
                if (policy != null)
                {
                    return await policy.ExecuteAsync(func);
                }

                return await func();
            }
            catch (Exception e) when (IsNetworkException(e))
            {
                throw new NetworkProblemException(e);
            }
        }

        private static bool IsJavaUnknownHostException(Exception exception)
        {
            return exception?.GetType().ToString() == "Java.Net.UnknownHostException";
        }

        private static bool IsCancelledWithoutCancellationRequest(Exception exception)
        {
            return exception is OperationCanceledException cancelledException 
                   && !cancelledException.CancellationToken.IsCancellationRequested;
        }

        private static bool IsNetworkException(Exception exception)
        {
            return exception is WebException || IsJavaUnknownHostException(exception) ||
                   IsCancelledWithoutCancellationRequest(exception);
        }
    }
}
