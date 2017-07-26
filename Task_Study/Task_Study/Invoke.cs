using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Study
{
    public delegate string delegateTest(string str);
    class Invoke
    {
        static void Main9(string[] args)
        {
            var task=tests();
            task.Wait();
            Console.WriteLine("这是主线程执行的");
            NewMethod();
            delegateTest test = new delegateTest(GetIntance);
            IAsyncResult result = test.BeginInvoke("这是异步执行的", AsyncAdd, (object)"我是回调函数");
            if (!result.IsCompleted)
            {
                ///处理其他工作。
            }
            test.EndInvoke(result);

            Console.WriteLine("这是同步执行的");
            Console.ReadLine();
        }

        private static void NewMethod()
        {
            int workthreadnumber;
            int iothreadnumber;

            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);
            Console.WriteLine(" CurrentThreadId is {0}\n CurrentThread is background :{1}\n WorkerThreadNumber is:{2}\n IOThreadNumbers is: {3}\n",
              Thread.CurrentThread.ManagedThreadId,
              Thread.CurrentThread.IsBackground.ToString(),
              workthreadnumber.ToString(),
              iothreadnumber.ToString());
        }

        public static async Task tests()
        {

            var s= say();
            await s;
            Console.WriteLine("这是.......执行的");
            NewMethod();
        }
        public static async Task say()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine("这是say执行的");
            NewMethod();
        }
        private static  string GetIntance(string str)
        {
            
            Console.WriteLine("这是GetIntance执行的");
            NewMethod();
            return "111";
        }
        public static void AsyncAdd(IAsyncResult ar)
        {
            Console.WriteLine("这是回调函数执行的");
            NewMethod();
                AsyncResult acresult = (AsyncResult)ar;
            string data = acresult.AsyncState.ToString();
            System.Console.WriteLine("传递的数据{0}", data);
        }


        async Task<byte[]> AccessTheWebAsync()
        {
            var httpClient = new HttpClient();
            var getmsg = httpClient.GetAsync("http://www.asp.net");
            ///这里可以执行其他操作
            DoSomething();
            ///而这里需要getmsg的结果，所以需要await其完成。
            var response = await getmsg;
            return await response.Content.ReadAsByteArrayAsync();
        }
        public void DoSomething()
        {
        }

        
    }
}
