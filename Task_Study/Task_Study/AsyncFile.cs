using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Study
{
    class AsyncFile
    {
        static void Main44(string[] args)
        {

            PrintMessage("Main Thread start");

            // 初始化FileStream对象
            FileStream filestream = new FileStream("test.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 100, true);

            //打印文件流打开的方式
            Console.WriteLine("filestream is {0} opened Asynchronously", filestream.IsAsync ? "" : "not");

            byte[] writebytes = new byte[100000];
            string writemessage = "An operation Use asynchronous method to write message.......................";
            writebytes = Encoding.Unicode.GetBytes(writemessage);
            Console.WriteLine("message size is： {0} byte\n", writebytes.Length);
            // 调用异步写入方法比信息写入到文件中
            filestream.BeginWrite(writebytes, 0, writebytes.Length, new AsyncCallback(EndWriteCallback), filestream);
            Console.WriteLine("----------------------------------------------------------------------------");

            filestream.Flush();
            Console.Read();

        }

        // 当把数据写入文件完成后调用此方法来结束异步写操作
        private static void EndWriteCallback(IAsyncResult asyncResult)
        {

            PrintMessage("Asynchronous Method start");

            FileStream filestream = asyncResult.AsyncState as FileStream;

            // 结束异步写入数据
            filestream.EndWrite(asyncResult);
            filestream.Close();
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
    }
}
