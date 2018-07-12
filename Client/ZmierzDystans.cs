using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ZmierzDystans
    {
        public static double zmierzOdleglosc(int X1, int Y1, int X2, int Y2)
        {
            int dystans;
            int y1 = 0, y2 = 0, x1 = 0, x2 = 0;
            double dodane;
            y1 = Y1;
            y2 = Y2;
            x1 = X1;
            x2 = X2;
            dodane = (y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1);
            dystans = (int)Math.Sqrt(dodane);
            return dystans;

        }
    }
}