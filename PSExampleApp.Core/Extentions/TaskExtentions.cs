using System;
using System.Threading.Tasks;

namespace PSExampleApp.Core.Extentions
{
    public static class TaskExtentions
    {
        public static async void WithCallback<TResult>(
            this Task<TResult> asyncMethod,
            Action<TResult> onResult = null,
            Action<Exception> onError = null)
        {
            try
            {
                var result = await asyncMethod;
                onResult?.Invoke(result);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }

        public static async void WithCallback(
            this Task asyncMethod,
            Action onResult = null,
            Action<Exception> onError = null)
        {
            try
            {
                await asyncMethod;
                onResult?.Invoke();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }
    }
}