using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace 使用PLINQ
{
    class Program
    {
        static void Mains(string[] args)
        {
            try
            {
                var eorrs = new ConcurrentQueue<Exception>();
                try
                {
                    throw new InvalidOperationException("并行异常");
                }
                catch (Exception e)
                {
                    eorrs.Enqueue(e);
                }
                if (eorrs.Count > 0)
                {
                    throw new AggregateException(eorrs);
                }
            }
            catch (AggregateException err)
            {

                foreach (var item in err.InnerExceptions)
                {
                    Console.WriteLine("异常类型:{0};来自{1}；异常内容:{2}",item.InnerException.GetType(),
                        item.InnerException.Source,item.InnerException.Message);
                }
            }
        }
    }
}
