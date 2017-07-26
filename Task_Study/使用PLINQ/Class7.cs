using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
namespace 使用PLINQ
{
    class Class7
    {
        static void Main(string[] args)
        {
            var parallelquery = from t in GetTypes().AsParallel() select t;
            var parallelaggreator = parallelquery.Aggregate(() => new ConcurrentDictionary<char, int>(), (taskTotal, item) => AccumulateLettersInformation(taskTotal, item), (total, taskTotal) => MerageAccumulators(total, taskTotal), total => total);
            Console.WriteLine();
            Console.WriteLine("There were thr following letters in type names;");
            var orderedkeys = from k in parallelaggreator.Keys orderby parallelaggreator[k] descending select k;
            foreach (var c in orderedkeys)
            {
                Console.WriteLine("Letter {0}--------------{1} times",c,parallelaggreator[c]);
            }



            Console.ReadLine();
        }
        static ConcurrentDictionary<char, int> AccumulateLettersInformation(ConcurrentDictionary<char, int> taskTotal, string item)
        {
            foreach (var c in item)
            {
                if (taskTotal.ContainsKey(c))
                {
                    taskTotal[c] = taskTotal[c] + 1;
                }
                else
                {
                    taskTotal[c] = 1;
                }
            }
            Console.WriteLine("{0} type was aggregated on a thread id {1}",item,Thread.CurrentThread.ManagedThreadId);
            return taskTotal;
        }
        static ConcurrentDictionary<char, int> MerageAccumulators(ConcurrentDictionary<char, int> total, ConcurrentDictionary<char, int> taskTotal)
        {
            foreach (var key in taskTotal.Keys)
            {
                if (total.ContainsKey(key))
                {
                    total[key] = total[key] + taskTotal[key];
                }
                else
                {
                    total[key] = taskTotal[key];
                }
            }
            Console.WriteLine("--------------------");
            Console.WriteLine("Total aggregate value was calculated on a thread id {0}",Thread.CurrentThread.ManagedThreadId);
            return total;
        }
        static IEnumerable<string> GetTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetExportedTypes());
            return from type in types where type.Name.StartsWith("Web") select type.Name;
        }
    }
}
