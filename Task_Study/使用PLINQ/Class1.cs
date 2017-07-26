using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 使用PLINQ
{
    class Class1
    {
        static string Emulateprocessing(string taskname)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine("{0} task was processed on a thread id {1}",taskname,Thread.CurrentThread.ManagedThreadId);
            return taskname;
        }
        static void Mains(string[] args)
        {
            try
            {
                Parallel.Invoke(
              () => Emulateprocessing("task1"),
                () => Emulateprocessing("task2"),
                  () => Emulateprocessing("task3")
              );
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
            }

            var cts = new CancellationTokenSource();
            var result = Parallel.ForEach(Enumerable.Range(1,1000),
                new ParallelOptions() {
                    CancellationToken =cts.Token,
                    MaxDegreeOfParallelism =Environment.ProcessorCount,//指定并发操作数
                    TaskScheduler =TaskScheduler.Default 
                },
                (i, state)=>{
                    Console.WriteLine(i);
                    if (i == 20)
                    {
                        state.Break();//停止之后的迭代，当之前的还要继续工作

                    }
                }
                );
            Console.WriteLine(result.LowestBreakIteration);

            Stopwatch sw = new Stopwatch();
            sw.Start();
           
            List<int> list = new List<int>();
            list.AddRange(Enumerable.Range(1, 300));
            var results = Parallel.For(1, 100, (i) => { DoSomeWork(i); }
            );
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds+"----------1");
            Console.WriteLine("------");
            Console.WriteLine("iscompleted :{0}",result.IsCompleted);
            Console.WriteLine(result.LowestBreakIteration);
            Stopwatch sws = new Stopwatch();
            sws.Start();
            for (int i = 0; i < 100; i++)
            {
                DoSomeWork(i);
            }
            sws.Stop();
            Console.WriteLine(sws.ElapsedMilliseconds);
            Console.Read();
        }
        static void DoSomeWork(int i)
        {
            if (i < 0)
                throw new ArgumentException("i");
            Thread.Sleep(100);
        }
    }
}
