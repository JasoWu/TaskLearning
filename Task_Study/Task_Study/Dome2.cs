using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Task_Study
{
    public class Dome2
    {
        async static Task<string> getInfoAsync(string name)
        {
            Console.WriteLine("1");
            await Task.Delay(5000);
            Console.WriteLine("2");
            return string.Format("task {0} is running on a thread id {1},is thread pool thread :{2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
        async static Task AsynchronyWithAwait()
        {
            try
            {

                var info = getInfoAsync("0").ContinueWith(t => {
                    Console.WriteLine(t.Result);
                });
                await info;
                var run = Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    Console.WriteLine("使用Task.Run启动线程");
                });
                await run;
                Console.WriteLine("3");
                Task dy = Task.Delay(5000);
                Console.WriteLine("do some thing");
                var Delay = dy.ContinueWith((t) =>
                {
                    Console.WriteLine("delay 异步等待");
                });
                await Delay;
                var a = Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(5000);
                    return "4";
                }).ContinueWith(c => {
                    Console.WriteLine(c.Result);
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
                await a;
                Task str = new Task(async () => {
                    await Task.Delay(5000);
                    Console.WriteLine("使用task构造函数启动线程");
                });
                str.Start();
                await str;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        static void Main2(string[] args)
        {
            DateTime dt = DateTime.Now;
            Task t = AsynchronyWithAwait();
            t.Wait();

            Console.WriteLine((DateTime.Now - dt).Seconds);
            Console.Read();
        }




    }

}
