using SM.Messaging.Interfaces;
using System;

// Don't make .Console part of the namespace because then the static Console will get confused.

namespace SM.MessagingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IMessage message = null;

            Console.WriteLine("Hello World!");
        }
    }
}
