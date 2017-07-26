using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 使用PLINQ
{
    class Class5
    {
        static void Main1(string[] args)
        {
            IEnumerable<int> numbers = Enumerable.Range(-5, 10);
            var query = from number in numbers select 100 / number;
            try
            {
                foreach (var n in query)
                {
                    Console.WriteLine(n);
                }
            }
            catch (DivideByZeroException x)
            {
                Console.WriteLine(x.Message);
            }
            Console.WriteLine("--------------------");
            Console.WriteLine("sequential linq query processing");
            Console.WriteLine();
          
            try
            {
                var parallelquery = from number in numbers.AsParallel() select 100 / number;
                parallelquery.ForAll((i) => { Console.WriteLine(i); });
            }
            catch (AggregateException e)
            {
                e.Flatten().Handle(ex => {
                    if (ex is DivideByZeroException)
                    {
                        Console.WriteLine(ex.Message);
                        return true;
                    }
                    return false;
                });
            }
            Console.WriteLine("----------");
            Console.WriteLine("parallel linq query processing and results merging");
            Console.ReadLine();
        }
      
        
    }
}
