using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Study
{
    class Dome
    {
        static void Main1(string[] args)
        {

            Task<Int32[]> parent = new Task<Int32[]>(() =>
            {

                var results = new Int32[3];

                new Task(() => results[0] = Sum(10000), TaskCreationOptions.AttachedToParent).Start();

                new Task(() => results[1] = Sum(20000), TaskCreationOptions.AttachedToParent).Start();

                new Task(() => results[2] = Sum(30000), TaskCreationOptions.AttachedToParent).Start();
                return results;

            });

            var cwt = parent.ContinueWith(parentTask => Array.ForEach(parentTask.Result, Console.WriteLine));
            parent.Start();
            cwt.Wait();

        }

        private static Int32 Sum(Int32 i)
        {
            Int32 sum = 0;
            for (; i > 0; i--)
            {
                checked { sum += i; }
            }
            return sum;

        }

    }

}
