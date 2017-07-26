using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
namespace 使用Reactive_Extensions
{
    class Class1
    {
        static IEnumerable<int> EnumerableEventSequence()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
                yield return i;
            }
        }
        static void Main2(string[] args)
        {
            foreach (int i in EnumerableEventSequence())
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
            Console.WriteLine("IEnumerable");
            IObservable<int> o = EnumerableEventSequence().ToObservable();
            using (IDisposable subscription = o.Subscribe((i) => { Console.WriteLine("{0}---", i); }))
            {
                Console.WriteLine();
                Console.WriteLine("IObservable");
            }
            o = EnumerableEventSequence().ToObservable().SubscribeOn(TaskPoolScheduler.Default);
            using (IDisposable subscription = o.Subscribe(Console.WriteLine))
            {
                Console.WriteLine();
                Console.WriteLine("IObservable async");
                Console.ReadLine();
            }
            Console.ReadLine();
        }
    }
}
