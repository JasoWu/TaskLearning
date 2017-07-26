using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Study
{
    class Dome1
    {
        static void Main2(string[] args)
        {
            Task t = test();
            t.Wait();
            Console.Read();
        }
        static  Task test()
        {
            Func<Task<string>> fun = new Func<Task<string>>(async ()=> {
                var task = Task.Factory.StartNew<string>(()=> {
                    ///费时操作
                    //return "1";
                    throw new Exception("异常");
                });
                Thread.Sleep(100);
                //模拟其他操作               
                ///////////                
                ////做些其他操作，但是不依赖task任务返回的结果             
                ///////////             
                ////执行到await标记时，控制将返回到task方法上，直到task方法返回结果时，              
                ////将在这里恢复控制继续执行后续操作
                await task.ConfigureAwait(continueOnCapturedContext:false);
                ///ConfigureAwait方法参数false指出不能对其同步上下文来运行后续代码
                return task.Result;
            });
            Task<string> t = fun();
            Task t1 = t.ContinueWith(a=> {
                ///如异常时写日志操作
                var msg = a.Exception.GetBaseException().Message;
                Console.WriteLine(msg);
            }, TaskContinuationOptions.OnlyOnFaulted);
            Task t2 = t.ContinueWith(a=> {
                var result = a.Result;
                Console.WriteLine(result);
                ///无异常时继续其他操作
            }, TaskContinuationOptions.NotOnFaulted);
            return Task.WhenAny(t1,t2);///等待任何一个完成时返回
        }
    }
}
