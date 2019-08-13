using System;
using System.Threading.Tasks;

namespace GraphQl.Server.Annotations.Common.Helpers
{
    public static class ProcessingUtils
    {
        public static object ProcessSyncOrAsync(object value, Func<object, object> processFunc)
        {
            async Task<object> ProcessAsync(Task task)
            {
                await task.ConfigureAwait(false);
                var result = task is Task<object> objectTask
                    ? objectTask.Result
                    : ((dynamic) task).Result;

                return processFunc(result);
            }

            return value is Task taskResult
                ? ProcessAsync(taskResult)
                : processFunc(value);
        }
    }
}