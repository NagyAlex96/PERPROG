using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Ora_Loverseny
{
    internal class Zsoke
    {
        int height, weight;

        public Zsoke(int height, int weight)
        {
            this.height = height;
            this.weight = weight;
        }

        public double PaceMultiplier 
        { 
            get
            {
                return (height / 170) * (weight / 60);
            }
        }
    }
}
