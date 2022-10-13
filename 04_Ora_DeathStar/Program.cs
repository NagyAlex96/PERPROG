using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _04_Ora_DeathStar
{
    internal class Program
    {
        public const int SPEED = 2;

        static void Main(string[] args)
        {
            var xd = XDocument.Load("https://users.nik.uni-obuda.hu/perprog/labor/deathstar.xml");

            var munkasok = xd.Descendants("munkas").Select(x => new Munkas(
                x.Attribute("nev").Value,
                int.Parse(x.Attribute("allapot").Value),
                int.Parse(x.Attribute("tempo").Value))).ToList();

            var ts = munkasok.Select
                (x => new Task(() =>
                {
                    while (!x.elkeszult)
                    {
                        x.lep();
                    }
                }, TaskCreationOptions.LongRunning)).ToList();

            #region EgyRohamosztagos
            //Rohamosztagos r = new Rohamosztagos();

            //ts.Add(new Task(() =>
            //{
            //    while (true)
            //    {
            //        var m = munkasok.OrderBy(x => x.Allapot).First();
            //        r.Felugyel(m);
            //        Thread.Sleep(3000);
            //        r.FelugyeletVege();
            //    } }, TaskCreationOptions.LongRunning)); 
            #endregion

            //List<Rohamosztagos> rs = new List<Rohamosztagos>()
            //{
            //    new Rohamosztagos(),
            //    new Rohamosztagos(),
            //    new Rohamosztagos()
            //};

            var rs = Enumerable.Range(0, 3).Select(x => new Rohamosztagos()).ToList();

            object valasztasLock = new object();

            ts.AddRange(rs.Select(r => new Task(() =>
            {
                //while (true)
                while (munkasok.Any(x => !x.elkeszult))
                {
                    Munkas m;
                    lock (valasztasLock)
                    {
                        m = munkasok.
                       Where(x => !x.elkeszult && !x.Figyelik)
                       .OrderBy(x => x.Allapot).FirstOrDefault();
                        if (m != null)
                            r.Felugyel(m);
                    }
                    if (m != null)
                    {

                        Thread.Sleep(3000 / SPEED);
                        r.FelugyeletVege();
                    }
                }
            }, TaskCreationOptions.LongRunning)));

            ts.Add(new Task(() =>
            {
                int ms = 50;
                int index = 0;

                //while (true)
                while (munkasok.Any(x=>!x.elkeszult))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"Eltelt idő: {index++ * ms / 1000}");
                    foreach (var item in munkasok)
                        Console.WriteLine(item);

                    foreach (var r in rs)
                        Console.WriteLine(r.ToString().PadRight(40));

                    Thread.Sleep(ms / SPEED);
                }
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Eltelt idő: {index++ * ms / 1000}");
                foreach (var item in munkasok)
                    Console.WriteLine(item);

                foreach (var r in rs)
                    Console.WriteLine(r.ToString().PadRight(40));
            }, TaskCreationOptions.LongRunning));

            ts.ForEach(x => x.Start());



            Console.ReadKey();
        }
    }

    class Munkas
    {
        string nev;
        int allapot, tempo;

        public bool elkeszult
        {
            get
            {
                return this.allapot == 100;
            }
        }
        public int Allapot { get { return allapot; } }
        public string Nev { get { return this.nev; } }
        public bool Figyelik { get; set; }

        public Munkas(string nev, int allapot, int tempo)
        {
            this.nev = nev;
            this.allapot = allapot;
            this.tempo = tempo;
        }

        public void lep()
        {
            allapot++;
            Thread.Sleep((Figyelik ? tempo / 2 : tempo) / Program.SPEED);
        }

        public override string ToString()
        {
            return $"{this.nev} készültség: {allapot}%";
        }


    }

    class Rohamosztagos
    {
        string id;
        static int _idx = 1000;

        public Munkas Felugyelve { get; private set; }

        public Rohamosztagos()
        {
            this.id = $"FN" + _idx++;
        }

        public void Felugyel(Munkas m)
        {
            Felugyelve = m;
            m.Figyelik = true;
            //TODO
        }

        public void FelugyeletVege()
        {
            Felugyelve.Figyelik = false;
            Felugyelve = null;
        }

        public override string ToString()
        {
            return $"{this.id}: {(Felugyelve == null ? "-" : Felugyelve.Nev)}";
        }
    }
}
