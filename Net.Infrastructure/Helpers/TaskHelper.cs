using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace Net.Infrastructure.Helpers
{
    public static class TaskHelper
    {
        /// <summary>
        /// Реализация паттерна Комбинатор для асинхронной обработки.
        /// Определяет первую задачу, которая завершается.
        /// Удаляет выбранное задание из списка, чтобы не обрабатывать его более одного раза.
        /// Работает быстрее чем WhenAny на больших коллекциях
        /// </summary>
        public static Task<Task<T>>[] Interleaved<T>(IEnumerable<Task<T>> tasks)
        {
            var inputTasks = tasks.ToList();

            var buckets = new TaskCompletionSource<Task<T>>[inputTasks.Count];
            var results = new Task<Task<T>>[buckets.Length];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new TaskCompletionSource<Task<T>>();
                results[i] = buckets[i].Task;
            }

            var nextTaskIndex = -1;

            void Continuation(Task<T> completed)
            {
                var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
                bucket.TrySetResult(completed);
            }

            foreach (var inputTask in inputTasks)
                inputTask.ContinueWith(Continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return results;
        }

        /// <summary>
        /// Реализация паттерна Комбинатор для асинхронной обработки.
        /// Определяет первую задачу, которая завершается.
        /// Удаляет выбранное задание из списка, чтобы не обрабатывать его более одного раза.
        /// Работает быстрее чем WhenAny на больших коллекциях
        /// </summary>
        /// <param name="tasks"></param>
        public static Task<Task>[] Interleaved(IEnumerable<Task> tasks)
        {
            var inputTasks = tasks.ToList();

            var buckets = new TaskCompletionSource<Task>[inputTasks.Count];
            var results = new Task<Task>[buckets.Length];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new TaskCompletionSource<Task>();
                results[i] = buckets[i].Task;
            }

            int nextTaskIndex = -1;

            void Continuation(Task completed)
            {
                var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
                bucket.TrySetResult(completed);
            }

            foreach (var inputTask in inputTasks)
                inputTask.ContinueWith(Continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return results;
        }
    }
}