using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Threading;


namespace 使用Reactive_Extensions
{
    class Class3
    {
        static IDisposable OutputToConsole<T>(IObservable<T> sequence) 
        {
            
            return sequence.Subscribe(obj => { Console.WriteLine(obj); }, ex => Console.WriteLine("error:{0}", ex.Message), () => Console.WriteLine("Completed"));
        }

        static void Main(string[] args)
        {
            Console.WriteLine("subject");
            var subject = new Subject<int>();
            subject.OnNext(1);
            using (var subscription = OutputToConsole(subject))
            {
                subject.OnNext(1);
                subject.OnNext(1);
                subject.OnNext(1);
                subject.OnCompleted();
                subject.OnNext(1);
            }
            Console.WriteLine("ReplaySubject");
            var replaySubject = new ReplaySubject<string>();
            replaySubject.OnNext("A");
            using (var subscripton = OutputToConsole(replaySubject))
            {
                replaySubject.OnNext("B");
                replaySubject.OnNext("C");
                replaySubject.OnNext("D");
                replaySubject.OnCompleted();
            }
            Console.WriteLine("Buffered replaysubject");
            var bufferedsubject = new ReplaySubject<string>(2);
            bufferedsubject.OnNext("A");
            bufferedsubject.OnNext("B");
            bufferedsubject.OnNext("C");
            using (var subscription = OutputToConsole(bufferedsubject))
            {
                bufferedsubject.OnNext("D");
                bufferedsubject.OnCompleted();
            }
            Console.WriteLine("Time window replaysubject");
            var timeSubject = new ReplaySubject<string>(TimeSpan.FromMilliseconds(200));
            timeSubject.OnNext("A");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            timeSubject.OnNext("B");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            timeSubject.OnNext("C");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            using (var subscription = OutputToConsole(timeSubject))
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(300));
                timeSubject.OnNext("D");
                timeSubject.OnCompleted();
            }
            Console.WriteLine("Asyncsubject");
            var asyncsubject = new AsyncSubject<string>();
            asyncsubject.OnNext("A");
            using (var subscription = OutputToConsole(asyncsubject))
            {
                asyncsubject.OnNext("B");
                asyncsubject.OnNext("C");
                asyncsubject.OnNext("D");
                asyncsubject.OnCompleted();
            }
            Console.WriteLine("behaviorsubject");
            var behabiorsubject = new BehaviorSubject<string>("Default");
            using (var subscription = OutputToConsole(behabiorsubject))
            {
                behabiorsubject.OnNext("B");
                behabiorsubject.OnNext("C");
                behabiorsubject.OnNext("D");
                behabiorsubject.OnCompleted();
            }
        
            Console.Read();
        }
    }

    class demo
    {

        IDisposable OutputToConsole<test>(IObservable<test> sequence) 
        {

            return sequence.Subscribe(obj => { Console.WriteLine(obj); }, ex => Console.WriteLine("error:{0}", ex.Message), () => Console.WriteLine("Completed"));
        }
    }

    class Customeobserver : IObserver<test>
    {
        public test t;
        public Customeobserver(test y)
        {
            t = y;
        }
        public void OnCompleted()
        {
            Console.WriteLine(t.success);
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

 


        public void OnNext(test value)
        {
            throw new NotImplementedException();
        }
    }
    class test
    {
        public int age { get; set; }

        public string success { get; set; }
        public void dosomething()
        {
            Console.WriteLine(age);
        }
    }
}
