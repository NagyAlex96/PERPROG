using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01_Ora_Loverseny
{
    internal class Lo
    {
        string name;
        Zsoke horseMan;
        int luck, distance;
        static int index = 0;
        static Random random = new Random();
        const int goal = 60;

        public bool hasFinished
        {
            get
            {
                return distance == goal;
            }
        }

        int Pace
        {
            get
            {
                return (int)(200 * horseMan.PaceMultiplier * ((this.luck / 100.0 + 0.5)));
            }
        }

        public Lo()
        {
            this.name = "Lo" + (++index);
            this.horseMan = new Zsoke(random.Next(155, 186), random.Next(45, 76));
            this.luck = random.Next(1, 100);
            this.distance = 0;
        }

        public override string ToString()
        {
            //return $"{this.name}: {this.distance}";

            return $"{name} : {"*".PadLeft(distance)}";
        }

        public void Step()
        {
            while (!hasFinished)
            {
                Thread.Sleep(this.Pace);
                distance++;
            }
        }
    }
}
