using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Catch.Services
{
    public static class TaskExtensions
    {
        public static async void Forget(this Task task, params Type[] acceptableExceptions)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (!acceptableExceptions.Contains(ex.GetType()))
                    throw;
            }
        }

        public static async void Forget(this IAsyncAction task, params Type[] acceptableExceptions)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                if (!acceptableExceptions.Contains(ex.GetType()))
                    throw;
            }
        }
    }
}
