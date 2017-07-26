using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 使用PLINQ
{
    class Class3
    {
        static void PrintInfo(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine("{0} type was printed on a thread id {1}", typeName,Thread.CurrentThread.ManagedThreadId);
        }
        static string EmulateProcessing(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine("{0} type was processed on a thread id {1}", typeName,Thread.CurrentThread.ManagedThreadId);
            return typeName;
        }
        static IEnumerable<string> GetTypes()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies() from type in assembly.GetExportedTypes() where type.Name.StartsWith("Web") select type.Name;
        }
        static void Mains(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            var query = (from t in GetTypes() select EmulateProcessing(t)).ToList();
            foreach (string typename in query)
            {
                PrintInfo(typename);
            }
            sw.Stop();
            Console.WriteLine("-----------------");
            Console.WriteLine("sequential linq query.");
            Console.WriteLine("time elapsed :{0}",sw.Elapsed);
            Console.WriteLine("press  enter to continue...............");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();
            sw.Start();
            var parallelquery = from t in ParallelEnumerable.AsParallel(GetTypes()) select EmulateProcessing(t);
            parallelquery.ForAll(PrintInfo);
            foreach (string typeName in parallelquery)
            {
                PrintInfo(typeName);
            }
            sw.Stop();
            Console.WriteLine("-----------------------");
            Console.WriteLine("parallel linq query the results are  being merged on a single thread ");
             Console.WriteLine("time elapsed :{0}",sw.Elapsed);
            Console.WriteLine("press enter to continue ...........");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();
            sw.Start();
            var parallelquerys = (from t in GetTypes().AsParallel() select EmulateProcessing(t)).ToList();
            //parallelquerys.ForAll(PrintInfo);
            Parallel.ForEach(parallelquerys, (i) => { PrintInfo(i); });
            sw.Stop();
            Console.WriteLine("----------");
            Console.WriteLine("parallel linq query the results are being  processed in  parallel ");
            Console.WriteLine("time elapsed :{0}",sw.Elapsed);
            Console.WriteLine("press enter to continue .........");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();
            sw.Start();
            query = (from t in GetTypes().AsParallel().AsSequential() select EmulateProcessing(t)).ToList();
            foreach (var typeName in query)
            {
                PrintInfo(typeName);
            }
            sw.Stop();
            Console.WriteLine("--------------------------");
            Console.WriteLine("parallel linq query transformed into sequential");
            Console.WriteLine("time elapsed:{0}",sw.Elapsed);
            Console.WriteLine("press enter to continue ....");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
