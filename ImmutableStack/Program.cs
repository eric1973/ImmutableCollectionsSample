using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmutableStack
{
    class Program
    {
        static void Main(string[] args)
        {

            var stack = ImmutableStack<int>.Empty;

            stack = stack.Push(13);
            stack = stack.Push(7);

            // Displays "7" followed by "13" (LIFO: Last In -> First Out)
            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }

            int lastItem;
            var oldStack = stack; // Oldstack is enumerated in Thread A.
            stack = stack.Pop(out lastItem); // stack is used in Thread B.
            
            // Both Threads operate on different instances! So a safe enumeration
            // is possible. Below Assert fails if uncommented.
            //Debug.Assert(Object.ReferenceEquals(oldStack, stack));
            
            
            // Displays "7"
            Console.WriteLine(lastItem);


            var smallStack = ImmutableStack<int>.Empty;
            smallStack = smallStack.Push(13);

            var bigStack = smallStack.Push(7);

            // Displays "7" followed by "13" (LIFO: Last In -> First Out)
            foreach (var item in bigStack)
            {
                Console.WriteLine("bigstack: " + item);
            }

            // Displays "13" (LIFO: Last In -> First Out)
            foreach (var item in smallStack)
            {
                Console.WriteLine("smallStack: " + item);
            }

            

            Console.ReadLine();
        }
    }
}
