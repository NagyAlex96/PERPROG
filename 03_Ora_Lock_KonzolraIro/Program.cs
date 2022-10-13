using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _03_Ora_Lock_KonzolraIro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Iro iro = new Iro(0);
            //iro.Work();

            var ws = Enumerable.Range(0, 4)
                        .Select(i => new Iro(i))
                        .ToList();
            ws.Select(
                x => new Task(() => { x.Work(); }, TaskCreationOptions.LongRunning))
                .ToList()
                .ForEach(x => x.Start());

            new Task(() =>
            {
                while (true)
                {
                    lock (Iro.lockObject)
                    {

                        Console.SetCursorPosition(0, 18);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Összeg: " + Iro.sum);
                        Thread.Sleep(50);
                    }
                }
            }, TaskCreationOptions.LongRunning).Start();

            Console.ReadKey();
        }


    }

    class Iro
    {
        const int SorHossz = 70;

        static ConsoleColor[] szinek = new ConsoleColor[]
        {
            ConsoleColor.Yellow,
            ConsoleColor.Cyan,
            ConsoleColor.Magenta,
            ConsoleColor.Green,
            ConsoleColor.DarkCyan,
        };

        public static int sum = 0;

        int sor, oszlop;
        ConsoleColor szin;

        public Iro(int idx)
        {
            this.sor = idx;
            this.oszlop = 0;
            this.szin = szinek[idx % szinek.Length];
        }

        static Random rnd = new Random();

        int General()
        {
            Thread.Sleep(rnd.Next(10, 201));

            return rnd.Next(0, 10);
        }

        void Kiiras(int szam)
        {
            if (SorVege())
            {
                sor += 1;
                oszlop = 0;
            }
            lock (lockObject)
            {
                Console.SetCursorPosition(oszlop, sor);
                Console.ForegroundColor = this.szin;
                Console.Write(szam);
            }
            oszlop++;
        }

        public static object lockObject = new object();

        public void Work(int db = SorHossz * 2)
        {
            //lock (lockObject)
            //{

            for (int i = 0; i < db; i++)
            {
                //lock (lockObject)
                //{

                int szam = General();
                Interlocked.Add(ref sum, szam);
                //sum += szam;
                Kiiras(szam);
                //}
            }
            //}
        }

        bool SorVege()
        {
            return oszlop >= SorHossz;
        }
    }
}
