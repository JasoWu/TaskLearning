using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Study
{
    class Dome3
    {
        static void Main333(string[] args)
        {
            Task t = AsynchronousProcessing();
            t.Wait();
            Console.Read();
        }
        async static Task AsynchronousProcessing()
        {
            Func<string, Task<string>> asyncLamdba = async name =>
            {
                await Task.Delay(8);
                Console.WriteLine(name);
                return "999";
            };
          // 返回三个task线程
            ///三个任务并行执行
            var ex = SayEx();
            var t = Say();
            var lamba = asyncLamdba("拉姆达");
            await ex;
            await t;
            await lamba;
            ///并行执行
            var list = new List<Task> { Say(), SayEx() };
            list.ForEach(async item => await item);

            ///并行执行
            var listfunc = new List<Func<Task>> { Say, SayEx };
            listfunc.ForEach(async item => await item());

            ///顺序执行
            await Say();
            await SayEx();
            Console.WriteLine("继续执行后续任务");
        }
        static Task Say()
        {
            return Task.Factory.StartNew(() => {
                for (var i = 0; i < 100; i++)
                {
                    Console.WriteLine(i + System.Environment.NewLine);
                }
            });
        }
        static async Task SayEx()
        { 
            await Task.Delay(1);
            //调用await操作符会自动创建一个task.ContinueWith<>后续任务
            for (var i = 0; i < 50; i++)
            {
                Console.WriteLine("A"+i+ System.Environment.NewLine);
            }
            Console.WriteLine("task 并行测试");
        }
    }
}
