using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 使用并发集合
{
    class ConcurrentBagDome
    {
        static Dictionary<string, string[]> _contentEmulation = new Dictionary<string, string[]>();
        /// <summary>
        /// ConcurrentBag是一个支持重复元素的无序集合，可以用于该场景（多线程）：每个线程产生和消费自己的任务
        /// ，极少与其他线程的任务交互（如果要交互则使用锁lock），Add方法添加元素，TryPeek方法检查元素，TryTake方法获取元素并删除
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Task t = RunProgram();
            t.Wait();
            Console.Read();
        }
        static async Task RunProgram()
        {
            var bag = new ConcurrentBag<CrawLingTask>();
            string[] urls = new[] { "1","2","3","4"};
            var crawlers = new Task[4];
            for(var i =1; i <= 4; i++)
            {
                string name = "crawlere" + i;
                bag.Add(new CrawLingTask() { name = name });
                crawlers[i - 1] = Task.Run(()=>Crawl(bag,name));
            }
            await Task.WhenAll(crawlers);
        }
        static async Task Crawl(ConcurrentBag<CrawLingTask> bag,string name)
        {
            ///每个线程使用自己分配的任务，只有当本地队列没有任何任务时，才尝试执行锁操作从其他线程中获取任务
            CrawLingTask task;
            while(bag.TryTake(out task))
            {
                var fn = new Func<CrawLingTask,Task<IEnumerable<string>>>(async(t)=> {
                    await GetRandomDelay();
                    if (_contentEmulation.ContainsKey(t.name))
                    {
                        return _contentEmulation[t.name];
                    }
                    else
                    {
                        return null;
                    }
                });
                IEnumerable<string> urls = await fn(task);
                if (urls != null)
                {
                    foreach(var url in urls)
                    {
                        var tt = new CrawLingTask() { name = url };
                        bag.Add(tt);
                    }
                }
                Console.WriteLine("indexing url {0} posted by {1} is completed by {2}", task.name,task.name, name);
            }
        }
        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }
        class CrawLingTask
        {
            public string name { get; set; }
        }
    }
}
