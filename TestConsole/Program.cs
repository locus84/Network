using Locus.Network;
using System;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                var socket = new NetSocket();
                socket.Start(55111);
                var socket2 = new NetSocket();
                socket2.Start(55111);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}