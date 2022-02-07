using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Net.Infrastructure.Extensions
{
    public static class ParallelExtensions
    {
        public static async Task ParallelForeach<T>(this IEnumerable<T> collection, Func<T, Task> action, ILogger logger, int threadsCount = 256)
        {
            var throttler = new SemaphoreSlim(threadsCount);
            var tasks = collection.Select(async item =>
            {
                await throttler.WaitAsync();
                try
                {
                    await action(item);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Exception on loop processing");
                }
                finally
                {
                    throttler.Release();
                }
            });
            
            await Task.WhenAll(tasks);
        }
    }
}