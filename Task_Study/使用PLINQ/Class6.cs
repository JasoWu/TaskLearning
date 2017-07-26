using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 使用PLINQ
{
    class Class6
    {
        static void Mains(string[] args)
        {
            var partitioner = new StringPartitoner(GetTypes());
            var parallerquery = from t in partitioner.AsParallel() select EmulateProcessing(t);
            parallerquery.ForAll(PrintInfo);
            Console.ReadLine();
        }
        static void PrintInfo(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine("{0} type was printed on a thread id {1}", typeName,Thread.CurrentThread.ManagedThreadId);
        }
        static string EmulateProcessing(string typename)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine("{0} type was processed on a thread id {1} .has {2} length.",typename,Thread.CurrentThread.ManagedThreadId,typename.Length % 2==0?"even":"odd");
            return typename;
        }
        static IEnumerable<string> GetTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetExportedTypes());
            return from type in types where type.Name.StartsWith("Web") select type.Name;
        }
        
    }
    class StringPartitoner : Partitioner<string>
    {
        private readonly IEnumerable<string> _data;
        public StringPartitoner(IEnumerable<string> data)
        {
            _data = data;
        }
        public override bool SupportsDynamicPartitions
        {
            get { return true; }
        }
        public override IList<IEnumerator<string>> GetPartitions(int partitionCount)
        {
            var result = new List<IEnumerator<string>>(2);
            result.Add(CreateEnumerator(true));
            result.Add(CreateEnumerator(false));
            return result;
        }
        IEnumerator<string> CreateEnumerator(bool isEven)
        {
            foreach (var d in _data)
            {
                if (!(d.Length % 2 == 0 ^ isEven))
                    yield return d;
            }
        }
      
    }
}
