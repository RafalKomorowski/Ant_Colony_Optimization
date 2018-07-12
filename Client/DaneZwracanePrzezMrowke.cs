using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class DaneZwracanePrzezMrowke
    {
        private List<int> sciezka;
        private double dlugoscSciezki;

        public DaneZwracanePrzezMrowke(List<int> sciezka, double dlugoscSciezki)
        {
            this.sciezka = sciezka;
            this.dlugoscSciezki = dlugoscSciezki;
        }

        public List<int> getSciezka()
        {
            return sciezka;
        }

        public double getDlugoscSciezki()
        {
            return dlugoscSciezki;
        }


    }
}