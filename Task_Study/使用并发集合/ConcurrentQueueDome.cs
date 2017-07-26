using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 使用并发集合
{
    class ConcurrentQueueDome
    {
        /// <summary>
        /// ConcurrentQueue使用了原子的比较和交换操作，以及SpinWait来保证线程安全，它实现了一个先进先出的集合
        /// Enqueue方法入列，TryDequeue方法试图出列返回第一个元素并删除它，而TryPeek方法试图得到第一个元素并不从队列删除该元素
        /// </summary>
        /// <param name="args"></param>
        static void Main2(string[] args)
        {
            ///多个工作者异步处理一组任务
            Task t = RunProgram();
            t.Wait();
            Console.Read();
        }
        static async Task RunProgram()
        {
            var taskQueue = new ConcurrentQueue<CustomTask>();
            var cts = new CancellationTokenSource();
            var taskSource = Task.Run(() =>
            {
                ///模拟添加任务到队列
                TaskProducer(taskQueue);
            });
            ///模拟4个工作者异步处理任务
            Task[] processors = new Task[4];
            for (var i = 1; i <= 4; i++)
            {
                string processorid = i.ToString();
                processors[i - 1] = Task.Run(() =>
                {
                    TaskProcessor(taskQueue, "processor" + processorid, cts.Token);
                });
            }
            await taskSource;
            ///1秒后取消任务
            cts.CancelAfter(TimeSpan.FromSeconds(1));
            await Task.WhenAll(processors);
        }

        static async Task TaskProducer(ConcurrentQueue<CustomTask> queue)
        {
            for (var i = 1; i <= 20; i++)
            {
                await Task.Delay(2);
                var workItem = new CustomTask() { Id = i };
                queue.Enqueue(workItem);
                Console.WriteLine("task {0} has been posted", workItem.Id);

            }
        }
        static async Task TaskProcessor(ConcurrentQueue<CustomTask> queue, string name, CancellationToken token)
        {
            CustomTask workItem;
            bool dequeueSuccesful = false;
            await GetRandomDelay();
            do
            {
                dequeueSuccesful = queue.TryDequeue(out workItem);
                if (dequeueSuccesful)
                {
                    Console.WriteLine("task {0} has been processed by {1}", workItem.Id, name);
                }
                await GetRandomDelay();

            } while (!token.IsCancellationRequested);

        }
        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }
        class CustomTask
        {
            public int Id { get; set; }
        }
    }
}
