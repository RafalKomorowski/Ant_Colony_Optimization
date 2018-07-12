using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class DaneNastPunktu

    {
        private List<int> miastaNieOdwiedzone = new List<int>();
        private List<int> miastaOdwiedzone = new List<int>();
        private int x = 0;

        public void dodajDoNieodwiedzonych(int wybraneMiasto)
        {
            miastaNieOdwiedzone.Add(wybraneMiasto);
        }
        public void dodajDoOdwiedzonych(int wybraneMiasto)
        {
            miastaOdwiedzone.Add(wybraneMiasto);
        }

        public void usunZNieodwiedzonych(int wybrana)
        {
            miastaNieOdwiedzone.Remove(wybrana);
        }

        public void miastoWybrane(int wybrane)
        {
            this.x = wybrane;
        }

        public int zwrocMiastoWybrane()
        {
            return x;
        }

    }
}
