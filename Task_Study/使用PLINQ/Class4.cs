using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 使用PLINQ
{
    class Class4
    {
        static string EmulateProcessing(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            Console.WriteLine("{0} type was processed on a thread id {1}",typeName,Thread.CurrentThread.ManagedThreadId);
            return typeName;
        }
        static IEnumerable<string> GetTypes()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies() from type in assembly.GetExportedTypes() where type.Name.StartsWith("Web") select type.Name;
        }
        static void Mains(string[] args)
        {
            var parallelQuery = from t in GetTypes().AsParallel() select EmulateProcessing(t);
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(3));
            try
            {
                parallelQuery.WithDegreeOfParallelism(Environment.ProcessorCount).WithExecutionMode(ParallelExecutionMode.ForceParallelism).WithMergeOptions(ParallelMergeOptions.Default).WithCancellation(cts.Token).ForAll(Console.WriteLine);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("operation has been canceled");
            }
            Console.WriteLine("------------------------");
            Console.WriteLine("Unordered Plinq query execution");
            var unorderedquery = from i in ParallelEnumerable.Range(1, 30) select i;
            foreach (var i in unorderedquery)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("-------------------------");
            Console.WriteLine("ordered plinq query execution");
            var orderedquery = from i in ParallelEnumerable.Range(1, 30).AsOrdered() select i;
            foreach (var i in orderedquery)
            {
                Console.WriteLine(i);
            }
            Console.ReadLine();
        }
    }
}
