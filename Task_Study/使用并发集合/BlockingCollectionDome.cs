using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 使用并发集合
{
    class BlockingCollectionDome
    {
        /// <summary>
        /// BlockingCollection可以实现管道场景：分块，调整内部集合容量，取消集合操作，从多个块集合中获取元素
        /// </summary>
        /// <param name="args"></param>
     
        static void Mains(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Uing a stack inside of blockingcollection");
            Console.WriteLine();
            Task t = Runprogram(new ConcurrentStack<CustomTak>());
            t.Wait();
            Console.Read();
        }
        static async Task Runprogram(IProducerConsumerCollection<CustomTak> collection = null)
        {
            var taskCollection = new BlockingCollection<CustomTak>();
            if (collection != null)
            {
                taskCollection = new BlockingCollection<CustomTak>(collection);
            }
            var taskSource = Task.Run(() => TaskProducer(taskCollection));
            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorid = "processor " + i;
                processors[i - 1] = Task.Run(() => TaskProcessor(taskCollection, processorid));
            }
            var t1 = taskSource;
            var t2 = Task.WhenAll(processors);
            ///并行执行
            await t1;
            await t2;
        }
        static async Task TaskProducer(BlockingCollection<CustomTak> collection)
        {
            for (int i = 01; i <= 20; i++)
            {
                await Task.Delay(20);
                var workItem = new CustomTak() { Id = i };
                collection.Add(workItem);
                Console.WriteLine("Task {0} have been posted", workItem.Id);
            }
            collection.CompleteAdding();///不在接受任务
        }
        static async Task TaskProcessor(BlockingCollection<CustomTak> collection, string name)
        {
            await GetRandomDelay();
            ///GetConsumingEnumerable()获取工作项，如果集合中没有任何元素，GetConsumingEnumerable方法会阻塞工作线程直到有元素
            ///被放置到集合中
            foreach (var item in collection.GetConsumingEnumerable())
            {
                Console.WriteLine("Task {0} have been processed by {1}", item.Id, name);
                await GetRandomDelay();
            }
        }
        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }
    }
    class CustomTak
    {
        public int Id { get; set; }
    }
}
