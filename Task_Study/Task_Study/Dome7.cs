using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Study
{
    class Dome7
    {
        static void Main333(string[] args)
        {
            var t = Delay3000Async();
            var tt = t.Result;
            Console.WriteLine(tt+"sdfsdfsdfsdfsdf");//等待任务完成
            Console.WriteLine(1);
            Console.Read();
        }
        static async Task<string> Delay3000Async()
        {
            
            var action = new Func<Task>(async () => {
                await Task.Factory.StartNew( () => {
                    Task.Delay(3000);
                    Console.Write(5);
                });
            });
            await action();///不会阻塞线程，会先输出3000，然后输出时间
            Console.WriteLine(3000);
            Console.WriteLine(DateTime.Now);
            return "888";
        }
    }
}
