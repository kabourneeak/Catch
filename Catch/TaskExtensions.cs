using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Catch
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
