using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 使用并发集合
{
    class ConcurrentStackDome
    {
        /// <summary>
        /// 后进先出
        /// ConcurrentStack使用了原子的比较和交换操作,是一个后进先出的集合,使用Push和PushRange方法添加元素
        ///使用TryPop和TryPopRange方法获取元素（出栈后会删除元素），使用TryPeek方法检查元素（不会删除元素）
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
            var taskStack= new ConcurrentStack<CustomTask>();
            var cts = new CancellationTokenSource();
            var taskSource = Task.Run(() =>
            {
                ///模拟添加任务到队列
                TaskProducer(taskStack);
            });
            ///模拟4个工作者异步处理任务
            Task[] processors = new Task[4];
            for (var i = 1; i <= 4; i++)
            {
                string processorid = i.ToString();
                processors[i - 1] = Task.Run(() =>
                {
                    TaskProcessor(taskStack, "processor" + processorid, cts.Token);
                });
            }
            await taskSource;
            Console.WriteLine("aaaaaaaaaaaaaaaaaaaa");
            ///1秒后取消任务
            cts.CancelAfter(TimeSpan.FromSeconds(1));
            await Task.WhenAll(processors);
        }

        static async Task TaskProducer(ConcurrentStack<CustomTask> stack)
        {
            for (var i = 1; i <= 20; i++)
            {
                await Task.Delay(2);
                var workItem = new CustomTask() { Id = i };
                stack.Push(workItem);
                Console.WriteLine("task {0} has been posted", workItem.Id);

            }
        }
        static async Task TaskProcessor(ConcurrentStack<CustomTask> stack, string name, CancellationToken token)
        {
            CustomTask workItem;
            bool dequeueSuccesful = false;
            await GetRandomDelay();
            do
            {
                dequeueSuccesful = stack.TryPop(out workItem);
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
