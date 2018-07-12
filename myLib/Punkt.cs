using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLib
{
    class Punkt
    {
        static Random random = new Random();
        private int x, y;

        public Punkt()
        {
            int x = random.Next(30, 1400);
            int y = random.Next(50, 800);
            this.x = x;
            this.y = y;

        }

        public Punkt zwrocPunkt()
        {
            Punkt punkt = new Punkt();
            return punkt;
        }

        public int zwrocX()
        {
            return x;
        }
        public int zwrocY()
        {
            return y;
        }



    }
}
