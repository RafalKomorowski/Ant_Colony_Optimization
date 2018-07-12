using System;
using System.Collections.Generic;

namespace Client
{

    class Ant
    {
        static int[,] TabGraf;
        //static readonly double beta = 0.9;
        //static readonly double alfa = 0.1;
        static double dlugoscTrasy = 0;

        int WybierzNastepnyPkt(int[,] TablicaGrafu, double[,] TablicaFeromonu, int MiastoStartowe, List<int> miastaNieOdwiedzone)
        {
            double[] tablicaSasiadow = new double[(int)Math.Sqrt(TablicaFeromonu.Length)];
            int[,] Graf = TablicaGrafu;
            double najlepszySasiad = 0;
            double najwiekszaWartoscSasiada = 0;
            int iloscMiast = Graf.Length / 2;
            int wybrana = 0;
            int licznik = iloscMiast;
            double wylosowanaLiczba = StaticRandom.Rand(); // statyczny random dla wyeliminowania powtorzen
            double sumaDo1 = 0;

            foreach (int element in miastaNieOdwiedzone) //dla wszystkich miast nieodwiedzonych:
            {
                //Określenie prawdopodobieństwa przejścia w miasta a do miasta b
                double tmp = ZwroPrawdopodobieństwoPrzejścia(Graf, TablicaFeromonu, MiastoStartowe, element, miastaNieOdwiedzone); 
               
                if (tmp > najwiekszaWartoscSasiada) // wybranie nastepnego punktu z sumy do 1. 
                {
                    najlepszySasiad = element;
                    najwiekszaWartoscSasiada = tmp;

                }
                sumaDo1 = sumaDo1 + tmp;
                // Console.WriteLine("Suma: " + Math.Round(sumaDo1, 4));

                //tablica sasiadow będąca tablicą trzymającą sumę prawdopodobieństwa przejścia do nastpnych punktów
                tablicaSasiadow[element] = sumaDo1;
            }           
            while (licznik >= 0) //
            {
                bool czySprawdzac = miastaNieOdwiedzone.Contains(licznik);
                if (czySprawdzac == true) //jezeli miasto nie odwiedzone: 
                {
                    if (tablicaSasiadow[licznik] > wylosowanaLiczba) //wybor punktu z wartosi dodanych do 1
                    {
                        wybrana = licznik;
                    }
                }
                licznik -= 1;
            }            
            dlugoscTrasy = dlugoscTrasy + ZwrocDystans(Graf[MiastoStartowe, 0], Graf[wybrana, 0], Graf[MiastoStartowe, 1], Graf[wybrana, 1]);
            return wybrana;
        }



        // obliczenie prawdopodobienstwa przejscia z miasta X do pozostalych nieodwiedzonych 
        private static double ZwroPrawdopodobieństwoPrzejścia(int[,] Graf, double[,] TablicaFeromonu, int MiastoAktualne, int MiastoBadane, List<int> miastaNieOdwiedzone)
        {
            double Wynik = 0;
            double wynikMianownika = 0;

            // obliczenie miasnownika ze wzoru 
            foreach (int element in miastaNieOdwiedzone)
            {
                double Nil = 1 / ZwrocDystans(Graf[MiastoAktualne, 0], Graf[element, 0], Graf[MiastoAktualne, 1], Graf[element, 1]); //widocznosc
                wynikMianownika = wynikMianownika + ((Math.Pow(TablicaFeromonu[MiastoAktualne, element], Program.alfa)) * Math.Pow(Nil, Program.beta));

            }

            // oliczenie licznika 
            Double Nij = 1 / ZwrocDystans(Graf[MiastoAktualne, 0], Graf[MiastoBadane, 0], Graf[MiastoAktualne, 1], Graf[MiastoBadane, 1]); //widocznosc
            double wynikLicznika = (double)(Math.Pow(TablicaFeromonu[MiastoAktualne, MiastoBadane], Program.alfa) * Math.Pow(Nij, Program.beta));

            Wynik = wynikLicznika / wynikMianownika;
            return Wynik;
        }

        //odleglosc od a do b
        static double ZwrocDystans(double x1, double x2, double y1, double y2)
        {
            double dystans = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
            return dystans;
        }


        public DaneZwracanePrzezMrowke RozpocznijNastepnaIteracje(int[,] graf, double[,] feromon, int MiastoStartowe, int nrIteracji)
        {
            List<int> sciezka = new List<int>();
            int wielkoscTablicyFeromonu = (int)Math.Sqrt(feromon.Length);
            double[,] TabFeromonu = new double[wielkoscTablicyFeromonu, wielkoscTablicyFeromonu];
            int[,] tabGrafuTest = graf;
            TabGraf = tabGrafuTest;
            double[] tablicaSasiadow = new double[wielkoscTablicyFeromonu];
            List<int> miastaOdwiedzone1 = new List<int>();
            List<int> miastaNieOdwiedzone1 = new List<int>();
            TabFeromonu = feromon;
            sciezka.Clear();
            sciezka.Add(MiastoStartowe);
            miastaOdwiedzone1.Add(MiastoStartowe);
            dlugoscTrasy = 0;
            int iloscMiast = graf.Length / 2;


            //dodanie wszystkich miast do listy miast nieodwiedzonych 
            for (int j = 0; j < iloscMiast; j++)
            {
                if (j != MiastoStartowe)
                {
                    miastaNieOdwiedzone1.Add(j);
                }
            }

            int x = WybierzNastepnyPkt(TabGraf, TabFeromonu, MiastoStartowe, miastaNieOdwiedzone1); //szukanie następnego punktu od miasta startowego 
            miastaOdwiedzone1.Add(x); 
            miastaNieOdwiedzone1.Remove(x);
            sciezka.Add(x);

            int i = 1;
            while (i < (TabGraf.Length / 2) - 1) // Szukanie nastepnego miasta dla calego grafu 
            {
                miastaOdwiedzone1.Add(x);         // Dodanie wybranego miasta do listy miast odwiedzonych
                miastaNieOdwiedzone1.Remove(x);   // Usunięcie wybranego miasta z listy miast jeszcze nie odwiedzonych 
                x = WybierzNastepnyPkt(TabGraf, TabFeromonu, x, miastaNieOdwiedzone1);
                sciezka.Add(x); //
                i += 1;

            }

            DaneZwracanePrzezMrowke daneMrowki = new DaneZwracanePrzezMrowke(sciezka, dlugoscTrasy);
            return daneMrowki;
        }
        
        static void main(string[] args)
        {
            
        }
    }
}
