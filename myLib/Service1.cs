using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using muLib;
using System.ServiceModel.Channels;
using System.IO;

namespace myLib
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        private static int[] tablicaGrafuTest = { 1 };
        private static double[,] tabFeromonu;
        public static double[] tablicaFeromonuSend;
        public static double beta = 0.9;
        public static double alfa = 0.1;
        public static int liczbaIteracji = 0;
        public static int miastoStartowe = 0;
        public static int liczbaMrowek = 5;
        public static int LiczbaNowychMrowek = 0;
        public static List<List<int>> listaSciezek = new List<List<int>>();
        public static DaneDoRysowania daneDoRysowania = new DaneDoRysowania();
        public static int CzyZaczacObliczenia = 0;
        private static List<int> FindIteration = new List<int>();
        private static List<double> FindTimes = new List<double>();
        static string filePath;
       // static string delimeter = ";";
        public static int zmiennaDoSomething = 0;


        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }


        // metoda testowa nie uzywana
        public int Twice(int value)
        {
            int v = value * 2;
            return v;
        }


        // generowanie grafu
        public int[] GenerownieGrafu(int i)
        {
            int wielkoscGrafu = i * 2; //poniewaz kazdy punkt ma 2 warotci x i y
            int iloscMiast = i;
            Form1.ptsTest = new System.Drawing.Point[i]; //inicjalizacja tabliy punktow w formularzu do rysowania
            int[] graf = new int[wielkoscGrafu];

            Random random = new Random();
            for (int a = 0; a < i; a++)
            {
                Punkt punkt = new Punkt();
                int wspX = punkt.zwrocX();
                int wspY = punkt.zwrocY();

                graf[a] = wspX;
                graf[i + a] = wspY;
                Form1.ptsTest[a] = new System.Drawing.Point(wspX, wspY); //dodanie punktu do tablicy rysowania
                Form1.liczbaMiast = iloscMiast;


            }
            // feromon dla startu obliczen zostaje zainicjalizowany mala, rowna wartoscia 
            tablicaGrafuTest = graf;
            tabFeromonu = new double[iloscMiast, iloscMiast];
            tablicaFeromonuSend = new double[iloscMiast * iloscMiast];
            for (int a = 0; a < iloscMiast * iloscMiast; a++)
            {
                tablicaFeromonuSend[a] = 0.0001;
            }

            int tmp = 0;
            // zmiana z formatu do przesylu na format tablicy n*n elementow
            for (int c = 0; c < iloscMiast; c++)
            {
                for (int j = 0; j < iloscMiast; j++)
                {
                    tabFeromonu[c, j] = tablicaFeromonuSend[tmp];
                    tmp += 1;
                }

            }
            return graf;
        }

        // inicjacja feromonu dla nowych danych
        public static void ZainicjujFeromon(int iloscMiast)
        {
            for (int a = 0; a < iloscMiast * iloscMiast; a++)
            {
                tablicaFeromonuSend[a] = 0.0001;
            }

            int tmp = 0;
            for (int c = 0; c < iloscMiast; c++)
            {
                for (int j = 0; j < iloscMiast; j++)
                {
                    tabFeromonu[c, j] = tablicaFeromonuSend[tmp];
                    tmp += 1;
                }

            }
        }





        public double[] AktualizujFeromonNew(int[] TablicaSciezekPolaczone, double[] tablicaDlugosciSciezek, int liczbaMrowek, int iloscWatkow)
        {
            //wywolanie nowego watku do akutlizacji feromonu
            Thread myNewThread = new Thread(() => AktualizacjaWatki(TablicaSciezekPolaczone, tablicaDlugosciSciezek, liczbaMrowek, iloscWatkow));
            myNewThread.Start();

            // czekam na zakonczenie aktualizacji feromonu 
            while (myNewThread.IsAlive == true)
            {
                Thread.Sleep(10);
            }
            double[] feromonZaktualizowany = ZwrocFeromonStatic();
            return feromonZaktualizowany;
        }


        private static void AktualizacjaWatki(int[] TablicaSciezekPolaczone, double[] tablicaDlugosciSciezek, int liczbaMrowek, int iloscWatkow)
        {
            int iloscMiast = tablicaGrafuTest.Length / 2;
            int poprzednik = miastoStartowe;
            List<List<int>> listaListSciezek = new List<List<int>>();
            List<List<int>> listaListSciezekDoRysowania = new List<List<int>>();

            // realizacja parowania feromonu
            for (int c = 0; c < iloscMiast; c++)
            {
                for (int j = 0; j < iloscMiast; j++)
                {
                    tabFeromonu[c, j] = tabFeromonu[c, j] * alfa;
                }
            }
            for (int i = 0; i < liczbaMrowek * iloscWatkow; i++)
            {
                List<int> lista = new List<int>();
                for (int j = 0; j < iloscMiast; j++)   //utworzenie listy list sciezek dla rysowania 
                {
                    if (i * iloscMiast + j < TablicaSciezekPolaczone.Length)
                        lista.Add(TablicaSciezekPolaczone[i * iloscMiast + j]);
                }            
                listaListSciezek.Add(lista);
                daneDoRysowania.dodajListe(listaListSciezek);
            }
            daneDoRysowania.dodajListe(listaListSciezek);
            int tmp = 0;
            int numerNajkrotszejSciezki = 0;
            double tmpMin = double.MaxValue;  
            int indexMinimum = 0;
            foreach (double element in tablicaDlugosciSciezek) //szukanie najkrotszej sciezki wsrod sciezek iteracji
            {
                if (element < tmpMin)
                {
                    tmpMin = element;
                    numerNajkrotszejSciezki = indexMinimum;
                }
                indexMinimum += 1;
            }            
                foreach (int element in listaListSciezek[numerNajkrotszejSciezki]) // aktualizacja feromonu 
                {
                    int x = listaListSciezek[numerNajkrotszejSciezki].Count; //pobranie dlugosci najkrotszej trasy
                    if (listaListSciezek[numerNajkrotszejSciezki].IndexOf(element) > 0 && listaListSciezek[numerNajkrotszejSciezki].IndexOf(element) < x)
                    {
                        tabFeromonu[poprzednik, element] = tabFeromonu[poprzednik, element] + (beta / tmpMin); //aktualizacja wg wzoru elementow sciezki
                        poprzednik = element;
                    }
                }            

            tmp = 0;
            for (int i = 0; i < iloscMiast; i++) // zmiana na format do przesylu danych 
            {
                for (int j = 0; j < iloscMiast; j++)
                {
                    tablicaFeromonuSend[tmp] = tabFeromonu[i, j];
                    tmp += 1;
                }

            }
        }






        public int[] ZwrocGraf()
        {
            return tablicaGrafuTest;
        }


        public static double[] ZwrocFeromonStatic()
        {
            return tablicaFeromonuSend;
        }

        public double[] ZwrocFeromon()
        {

            return tablicaFeromonuSend;
        }

        public int ZwrocLiczbeIteracji()
        {
            return liczbaIteracji;
        }

        public int ZwrocMiastoStartowe()
        {
            return miastoStartowe;
        }

        public static void ustalLiczbeMrowek(int i)
        {
            liczbaMrowek = i;
        }

        public int ZwrocLiczbeMrowek() // przeslanie liczby mrowek oraz pobranie IP wolajacego clienta
        {
            OperationContext oOperationContext = OperationContext.Current;
            MessageProperties oMessageProperties = oOperationContext.IncomingMessageProperties;
            RemoteEndpointMessageProperty oRemoteEndpointMessageProperty = (RemoteEndpointMessageProperty)oMessageProperties[RemoteEndpointMessageProperty.Name];

            string szAddress = oRemoteEndpointMessageProperty.Address;
            Console.WriteLine("Nowy klient podlaczony: " + szAddress);


            return LiczbaNowychMrowek;
        }

        public void startRysowanie()
        {

        }



        public static void doSomething() // start formularza do rysowania
        {
            Application.Run(new Form1(daneDoRysowania, liczbaMrowek));
        }



        public double ZwrocAlfa()
        {
            return alfa;
        }

        public double ZwrocBeta()
        {
            return beta;
        }

        public int startObliczen()
        {
            return CzyZaczacObliczenia;
        }

        public void PrzeslijStatystyke(int iteracjaZnalezienia, double czasZnalezienia) // wersja robocza 
        {

            FindIteration.Add(iteracjaZnalezienia);
            FindTimes.Add(czasZnalezienia);


        }

        public void PrzeslijDaneDoZapisu(bool czyOptymalna, int iteracja, double czas, double dlugoscTrasy, string sciezka, 
            int liczbaMiast, int liczbaIteracji, int liczbaMrowek, double alfaparm, double betaParm, string Ip)
        {
            // client przesyla dane algorytmu widoczne w parametrach
            // zapis danych do CSV

            string startupPath = Application.StartupPath;
            filePath = startupPath + "\\DaneAlgorytmu.csv";
            string dane = czyOptymalna.ToString() + ";" + iteracja.ToString() + ";" + czas.ToString() + ";" + dlugoscTrasy.ToString() + ";" + 
                sciezka.ToString() + ";" + liczbaMiast.ToString() + ";" + liczbaIteracji.ToString() + ";" + liczbaMrowek.ToString() + ";" + alfaparm.ToString() + 
                ";" + betaParm.ToString() + ";" + Ip + Environment.NewLine;
            File.AppendAllText(filePath, dane);


        }

        public void AktualizujFeromon(int[] sciazka, double dlugoscTrasy)
        {
            throw new NotImplementedException();
        }
    }
}
