using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class DaneIteracji
    {
        /*DaneIteracji(List<int> sciezka, double dlugosc)
        {
            ListaSciezek.Add(sciezka);
            ListaDlugosciSciezek.Add(dlugosc);
        }*/
        private List<List<int>> ListaSciezek = new List<List<int>>();
        private List<double> ListaDlugosciSciezek = new List<double>();

        public void dodajSciezke(List<int> sciezka, double dlugosc)
        {
            ListaSciezek.Add(sciezka);
            ListaDlugosciSciezek.Add(dlugosc);
        }

        public List<List<int>> pobierzListeSciezek()
        {
            return ListaSciezek;
        }

        public List<double> pobierzListeDlugosciSciezek()
        {
            return ListaDlugosciSciezek;
        }
    }
}
