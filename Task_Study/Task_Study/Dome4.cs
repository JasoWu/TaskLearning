using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Study
{
    class Dome4
    {
        static void Mains(string[] args)
        {
            Stopwatch timeer = new Stopwatch();
            timeer.Start();
            Task t = asynchronousprocess();
            t.Wait();
            timeer.Stop();
            Console.WriteLine(timeer.ElapsedMilliseconds);
            Console.Read();
        }

        async static Task asynchronousprocess()
        {
            Task<string> t1 = Getinfoasync("task1", 1);
            Task<string> t2 = Getinfoasync("task2", 4);
            string[] results = await Task.WhenAll(t1, t2);
            foreach (var x in results)
            {
                Console.WriteLine(x);
            }
        }
        async static Task<string> Getinfoasync(string name, int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            return name;
        }
    }
}
