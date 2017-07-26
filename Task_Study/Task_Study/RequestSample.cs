using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Study
{
    class RequestSample
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main Thread start");
            var taks=MethodC();
            taks.Wait();//这个Wait方法会真阻塞主线程。
            Console.Read();
        }
        public static async Task<int> MethodC()
        {
            Console.WriteLine("运行MethodC方法的线程ID:"+Thread.CurrentThread.ManagedThreadId);
            var task = MethodB();
            Console.WriteLine("。。。。。。。。。。。。。");
            ///当执行到await时，task已经完成了，此时不会发生线程切换
            var result = await task;
            Console.WriteLine("运行MethodC方法的线程ID:" + Thread.CurrentThread.ManagedThreadId);
            return  result;
        }
        public static async Task<int> MethodB()
        {
            Task<int> r = Task.Run(() =>
            {
                Console.WriteLine("线程方法 r 被执行");
                for (var i = 0; i < 1000000000; i++)
                {
                    //模拟其他操作
                }
                return 2 * 3;
            });
            //await 留下一个悬挂点，只有task执行完毕才能执行后面的代码
            var result = await r;
            Console.WriteLine("线程 r 执行完毕才能继续执行,返回结果{0}",result);
            Console.WriteLine("运行MethodB方法的线程ID:" + Thread.CurrentThread.ManagedThreadId);
            return result;
        }
        private static void NewMethod(out int workthreadnumber, out int iothreadnumber)
        {

            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);

            Console.WriteLine("CurrentThreadId is {0}\n CurrentThread is background :{1}\n WorkerThreadNumber is:{2}\n IOThreadNumbers is: {3}\n",

         Thread.CurrentThread.ManagedThreadId,
         Thread.CurrentThread.IsBackground.ToString(),
         workthreadnumber.ToString(),
         iothreadnumber.ToString());
        }

        public static async Task<string> RequestAsync()
        {
            int workthreadnumber, iothreadnumber;
            NewMethod(out workthreadnumber, out iothreadnumber);
            string uri = "http://www.cnblogs.com/";
            HttpClient client = new HttpClient();
            string body = await client.GetStringAsync(uri);

            NewMethod(out workthreadnumber, out iothreadnumber);
            return body;
        }
        // 回调方法
        private static void ProcessWebResponse(IAsyncResult result)
        {

            PrintMessage("Asynchronous Method start");

            WebRequest webrequest = (WebRequest)result.AsyncState;
            using (WebResponse webresponse = webrequest.EndGetResponse(result))
            {
                Console.WriteLine("Content Length is : " + webresponse.ContentLength);
            }
        }

        // 打印线程池信息
        private static void PrintMessage(String data)
        {
            int workthreadnumber;
            int iothreadnumber;

            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);

            Console.WriteLine("{0}\n CurrentThreadId is {1}\n CurrentThread is background :{2}\n WorkerThreadNumber is:{3}\n IOThreadNumbers is: {4}\n",
              data,
              Thread.CurrentThread.ManagedThreadId,
              Thread.CurrentThread.IsBackground.ToString(),
              workthreadnumber.ToString(),
              iothreadnumber.ToString());
        }


        public async  Task<string> Asv2()
        {
            var task =await  AssignValue2().ConfigureAwait(false);
            return task;
        }
        public async Task<string> AssignValue2()
        {
            await Task.Delay(500).ConfigureAwait(false);
            return  await Task.Run(() => { return "nihao"; });
        }
    }
}
