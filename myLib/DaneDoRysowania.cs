using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muLib
{
    public class DaneDoRysowania
    {
        private List<List<List<int>>> listaIteracji = new List<List<List<int>>>();


        public void dodajListe(List<List<int>> listaSciezek1Iteracji)
        {
            listaIteracji.Add(listaSciezek1Iteracji);
        }

        public List<List<List<int>>> pobierzListeIteracji()
        {
            return listaIteracji;
        }

    }
}
