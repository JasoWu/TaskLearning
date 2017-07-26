using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Study
{
    class Dome5
    {
        // 异步线程共享变量
        static AsyncLocal<int> local = new AsyncLocal<int>();
        static int a;
        static async Task RunAsync()
        {
            // 输出当前线程的ID
            Console.WriteLine($"异步等待前，当前线程ID：{Thread.CurrentThread.ManagedThreadId}");
            // 开始执行异步方法，并等待完成
            await Task.Delay(50);
            // 异步等待完成后，再次输出当前线程的ID
            Console.WriteLine($"异步等待后，当前线程ID：{Thread.CurrentThread.ManagedThreadId}");
        }
        static void Mains(string[] args)
        {
            // 声明一个委托实例
            Action act = async () =>
            {
                await RunAsync();
            };
            // 从代码上看，await前后是连续的，但实际上，在执行阶段，它们已经处于不同的线程上了
            // 执行委托
            act();

            Console.Read();
        }
    }
}
