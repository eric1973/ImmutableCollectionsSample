using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImmutableQueue
{
    class MyImmutableQueueClass
    {
        public MyImmutableQueueClass()
        {
            this.MyImmutableQueue = ImmutableQueue<int>.Empty;

            this.MyImmutableQueue = this.MyImmutableQueue.Enqueue(10);
            this.MyImmutableQueue = this.MyImmutableQueue.Enqueue(50);
        }

        public IImmutableQueue<int> MyImmutableQueue { get; private set; }

        public async Task<IImmutableQueue<int>> GetUpdatedQueueAsync()
        {
            await Task.Delay(1000);
            return this.MyImmutableQueue;
        }

        public void Enqueue(int value)
        {
            this.MyImmutableQueue = this.MyImmutableQueue.Enqueue(value);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            // NEW FW 4.5 Microsoft.Bcl.Immutable Collection. 
            // Is a light weight alternative to use of ConcurrentQueue (like above).
            // if the Queue doesn't CHANGE very often this is good usage. 
            // If it is a high frequent producer/consumer queue the ConcurrentQueue
            // is good usage. What is the criteria to use this or that?
            // If the UPDATE in the Main Thread (this example) would be very often,
            // the ConcurrentQueue would be best to establish that all Threads get 
            // nearly the same snapshot of data (if required).
            // If the UPDTE in the Main Thread occures less often the new Immutable
            // Base Class Library would be worth to try.
            // ImmutableXXX: When only one Thrad updates the ImmutableXXX but several
            // other read it.
            MyImmutableQueueClass myImmutable = new MyImmutableQueueClass();

            ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();
            // Producer Thread A
            var producer = Task.Run(async () =>
            {
                int i = 1;
                while (i < 100)
                {
                    myImmutable.Enqueue(50 + i);
                    await Task.Delay(2000);
                    i++;
                }
            });

            tasks.Add(producer);

            // Consumer Thread B
            var consumer1 = Task.Run(async () =>
            {
                int i  = 1;
                while (true)
                {
                    Console.WriteLine("Run {0}", i++);
                    var immutableLocalQueue = await myImmutable.GetUpdatedQueueAsync();
                    Console.WriteLine("Thread Pool Thread: ImmutableQueue.Count : {0}", immutableLocalQueue.Count());

                    foreach (var item in immutableLocalQueue)
                    {
                        Console.WriteLine("print immutableQueue item: " + item);

                    }
                }
            });

            tasks.Add(consumer1);

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("finished.");
            Console.ReadLine();
        }




    }
}
