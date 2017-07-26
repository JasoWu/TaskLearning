using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 使用并发集合
{
    class ConcurrentDictionaryDome
    {
        const string Item = "Dictionary item";
        public static string CurrentItem;
        /// <summary>
        /// ConcurrentDictionary线程安全的字典集合，对读操作无许使用锁，但是写操作需要锁。该并发字典使用多个锁，在字典桶之上实现了一个细粒度的锁模型
        /// 使用参数concurrencyLevel可以在构造函数中定义锁的数量；由于并发字典使用锁，所以一些操作需要获取该字典中的所有锁；如果没有必要请避免使用
        /// 一下操作：Count，IsEmpty，Keys，Values，CopyTo，ToArray；
        /// </summary>
        /// <param name="args"></param>
        static void Main1(string[] args)
        {
            var concurrentDictionary = new ConcurrentDictionary<int, string>();
            var dictionary = new Dictionary<int, string>();
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                lock (dictionary)
                {
                    dictionary[i] = Item;
                }
            }
            sw.Stop();
            Console.WriteLine("writeing to dictionary with a lock :{0}", sw.Elapsed);
            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                concurrentDictionary[i] = Item;
            }
            sw.Stop();
            Console.WriteLine("writing to a concurrent dictionary:{0}", sw.Elapsed);
            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                lock (dictionary)
                {
                    CurrentItem = dictionary[i];
                }
            }
            sw.Stop();
            Console.WriteLine("reading from dictionary with a lock:{0}", sw.Elapsed);
            sw.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                CurrentItem = concurrentDictionary[i];
            }
            sw.Stop();
            Console.WriteLine("reading  from a concurrent dictionary:{0}", sw.Elapsed);
            Console.Read();
        }
    }
}
