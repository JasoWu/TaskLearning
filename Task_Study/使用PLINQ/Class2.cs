using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 使用PLINQ
{
    class Class2
    {
        static void Mains(string[] args)
        {
            try
            {
                NormalFor();
                ParallelFor();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception Message:{0}", ex.Message.Trim()));
            }
            finally
            {
                Console.ReadLine();
            }
        }
        static int _forCount = 100;
        static void NormalFor()
        {
            Stopwatch _watch = new Stopwatch();
            _watch.Start();
            for (int i = 0; i < _forCount; i++)
            {
                DoSomeWork(i);
            }
            _watch.Stop();
            Console.WriteLine(string.Format("Normal For Cost Time:{0}", _watch.ElapsedMilliseconds));
        }
        static void ParallelFor()
        {
            Stopwatch _watch = new Stopwatch();
            _watch.Start();
            //写法一
            Parallel.For(0, _forCount, i =>
            {
                DoSomeWork(i);
            });
            _watch.Stop();
            Console.WriteLine(string.Format("Parallel For Cost Time:{0}", _watch.ElapsedMilliseconds));
            /*
            Parallel.For(0, _forCount, DoSomeWork);//写法二
            Parallel.For(0, _forCount, (int i) => { DoSomeWork(i); });//写法三
            */
        }
        static void DoSomeWork(int i)
        {
            if (i < 0)
                throw new ArgumentException("i");
            Thread.Sleep(100);
        }
    }
}
    

