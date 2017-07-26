using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Study
{
    class Dome6
    {
        static void Main11(string[] args)
        {

            Task parent = new Task(() =>
            {
                var cts = new CancellationTokenSource();
                var tf = new TaskFactory<Int32>(cts.Token, TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                //创建并启动3个子任务
                var childTasks = new[]
                {
                    tf.StartNew(() => Sum(cts.Token, 10000)),

                    tf.StartNew(() => Sum(cts.Token, 20000)),

                    tf.StartNew(() => Sum(cts.Token, Int32.MaxValue))  // 这个会抛异常

                };
                // 任何子任务抛出异常就取消其余子任务
                for (Int32 task = 0; task < childTasks.Length; task++)
                {
                    childTasks[task].ContinueWith(t => cts.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
                }
                // 所有子任务完成后，从未出错/未取消的任务获取返回的最大值
                // 然后将最大值传给另一个任务来显示最大结果
                tf.ContinueWhenAll(childTasks,
                   completedTasks => completedTasks.Where(t => !t.IsFaulted && !t.IsCanceled).Max(t => t.Result),
                   CancellationToken.None).ContinueWith(t => Console.WriteLine("The maxinum is: " + t.Result),
                      TaskContinuationOptions.ExecuteSynchronously).Wait(); // Wait用于测试
            });
            // 子任务完成后，也显示任何未处理的异常
            parent.ContinueWith(p =>
            {
                // 用StringBuilder输出所有
                StringBuilder sb = new StringBuilder("The following exception(s) occurred:" + Environment.NewLine);
                foreach (var e in p.Exception.Flatten().InnerExceptions)
                {
                    sb.AppendLine("   " + e.GetType().ToString());
                }
                Console.WriteLine(sb.ToString());
            }, TaskContinuationOptions.OnlyOnFaulted);
            // 启动父任务
            parent.Start();
            try
            {
                parent.Wait(); //显示结果
            }
            catch (AggregateException)
            {

            }
        }

        private static Int32 Sum(CancellationToken ct, int n)
        {
            int sum = 0;
            for (; n > 0; n--)
            {
                ct.ThrowIfCancellationRequested();
                checked { sum += n; }
            }
            return sum;
        }
    }
}
