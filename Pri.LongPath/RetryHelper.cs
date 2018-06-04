using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;
#if !NET_2_0
using System.Linq;
#endif

namespace Pri.LongPath
{
    public class RetryHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static T Retry<T>(Func<T> func, int retryCount)
        {
            return RetryWithDelay(func, retryCount, TimeSpan.Zero, new[] { typeof(Exception) });
        }

        public static T Retry<T>(Func<T> func, int retryCount, Type[] retryOnExceptions)
        {
            return RetryWithDelay(func, retryCount, TimeSpan.Zero, retryOnExceptions);
        }

        public static T RetryWithDelay<T>(Func<T> func, int retryCount, TimeSpan retryDelay)
        {
            return RetryWithDelay(func, retryCount, retryDelay, new[] { typeof(Exception) });
        }

        public static T RetryWithDelay<T>(Func<T> func, int retryCount, TimeSpan retryDelay, Type[] retryOnExceptions)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "Retry count is negative.");

            if (retryDelay < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(retryDelay), retryDelay, "Retry delay is negative.");

            if (retryOnExceptions == null || retryOnExceptions.Length == 0)
                throw new ArgumentNullException(nameof(retryOnExceptions));

            if (retryOnExceptions.Any(re => !typeof(Exception).IsAssignableFrom(re)))
            {
                throw new ArgumentException("Retriable exceptions list contains element(s) that are not exception types.");
            }

            var result = default(T);
            for (var i = 1; i <= retryCount; i++)
            {
                try
                {
                    result = func();
                    break;
                }
                catch (Exception ex)
                {
                    Log.Warn($"Exception on #{i} retry", ex);

                    if (i >= retryCount) throw;

                    if (!retryOnExceptions.Any(roe => roe.IsInstanceOfType(ex)))
                    {
                        // For aggregate exceptions we need to also check inner exceptions.
#if !NET_2_0

                        var aggregateException = ex as AggregateException;
                        if (aggregateException == null || !aggregateException.InnerExceptions.Any(ie => retryOnExceptions.Any(re => re.IsInstanceOfType(ie))))
                            throw;
#endif

                        throw;
                    }

                    Thread.Sleep(retryDelay);
                }
            }

            return result;
        }
    }

}
