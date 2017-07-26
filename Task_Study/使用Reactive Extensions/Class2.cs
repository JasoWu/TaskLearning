using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading;

namespace 使用Reactive_Extensions
{
    class Class2
    {
        static void Main1(string[] args)
        {
            var observer = new CustomObserver();
            var goodObservable = new CustomSequence(new []{1,2,3,4,5});
            var badObservable = new CustomSequence(null);
            using (IDisposable subscription = goodObservable.Subscribe(observer))
            {

            }
            using (IDisposable subscription = goodObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
            {
                Thread.Sleep(100);
            }
            using (IDisposable subscription = badObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
            {
                Console.ReadLine();
            }
            Console.ReadLine();
        }
    }
    class CustomObserver : IObserver<int>
    {
        public void OnNext(int value)
        {
            Console.WriteLine("Next value:{0}; thread Id:{1}",value,Thread.CurrentThread.ManagedThreadId);
        }
        public void OnError(Exception error)
        {
            Console.WriteLine("Error:{0}",error.Message);
        }
        public void OnCompleted()
        {
            Console.WriteLine("Completed");
        }
    }
    class CustomSequence : IObservable<int>
    {
        private readonly IEnumerable<int> _numbers;
        public CustomSequence(IEnumerable<int> numbers)
        {
            _numbers = numbers;
        }
        public IDisposable Subscribe(IObserver<int> observer)
        {
            foreach (var number in _numbers)
            {
                observer.OnNext(number);
            }
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
