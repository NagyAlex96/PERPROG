using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _01_Ora_Loverseny
{
    internal class Program
    {
        static Lo hourseClass = new Lo();
        static void Main(string[] args)
        {
            Example2();


            Console.ReadKey();
        }

        static void Example1()
        {

            for (int i = 0; i < 10; i++)
            {
                hourseClass.Step();
                Console.WriteLine(hourseClass);
            }
        }

        static void Example2()
        {

            var horses = Enumerable.Range(1, 10)
                      .Select(i => new Lo())
                      .ToList();

            //lontrunning saját önálló szál
            var task = horses.Select(h => new Task(() => hourseClass.Step(), TaskCreationOptions.LongRunning)).ToList();

            task.Add(new Task(() => 
            {
                //while (!horses.All(l => l.hasFinished))
                while(horses.Any(l => !l.hasFinished))
                {
                    //Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    foreach (var item in horses)
                    {
                        Console.WriteLine(item);
                        Thread.Sleep(40);
                    }
                }
            }));

            task.ForEach(t => t.Start());
        }
    }
}
