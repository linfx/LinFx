using System;
using System.Threading.Tasks;

namespace LinFx.Threading
{
    public static class AsyncHelper
    {
        /// <summary>
        /// Runs a task with the "Fire and Forget" pattern using Task.Run,
        /// and unwraps and handles exceptions
        /// </summary>
        /// <param name="task">A function that returns the task to run</param>
        /// <param name="handle">Error handling action, null by default</param>
        public static void FireAndForget(Func<Task> task, Action<Exception> handle = null)
        {
            Task.Run(() =>
            {
                ((Func<Task>)(async () =>
                {
                    try
                    {
                        await task().ConfigureAwait(true);
                    }
                    catch (Exception e)
                    {
                        handle?.Invoke(e);
                    }
                }))();
            });
        }

        /// <summary>
        /// Creates a new AsyncBridge. This should always be used in
        /// conjunction with the using statement, to ensure it is disposed
        /// </summary>
        public static AsyncBridge Wait => new AsyncBridge();

        public class AsyncBridge : IDisposable
        {
            /// <summary>
            /// Execute's an async task with a void return type
            /// from a synchronous context
            /// </summary>
            /// <param name="task">Task to execute</param>
            /// <param name="callback">Optional callback</param>
            public void Run(Task task, Action<Task> callback = default)
            {
                //_hasAsyncTasks = true;
                //_currentContext.Post(
                //    async _ =>
                //    {
                //        try
                //        {
                //            Increment();
                //            await task.ConfigureAwait(true);
                //            callback?.Invoke(task);
                //        }
                //        catch (Exception e)
                //        {
                //            _currentContext.InnerException = e;
                //        }
                //        finally
                //        {
                //            Decrement();
                //        }
                //    },
                //    null);
            }

            /// <summary>
            /// Execute's an async task with a T return type
            /// from a synchronous context
            /// </summary>
            /// <typeparam name="T">The type of the task</typeparam>
            /// <param name="task">Task to execute</param>
            /// <param name="callback">Optional callback</param>
            public void Run<T>(Task<T> task, Action<Task<T>> callback = null)
            {
                if (null != callback)
                {
                    Run((Task)task, (finishedTask) => callback((Task<T>)finishedTask));
                }
                else
                {
                    Run((Task)task);
                }
            }

            public void Dispose()
            {
            }
        }
    }
}
