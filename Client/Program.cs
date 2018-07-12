using System;
using System.ServiceModel;
using Client.ServiceReference1;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using System.Windows.Forms;
using System.IO;

public class Program
{

    static int nrMrowki = 0;
    private const int V = 0;
    static int[] tablicagrafu = new int[1];
    static int liczbaIteracji = 0;
    static int miastoStartowe = 0;
    public static double alfa, beta;
    private static int nrRunningThreads = 0;
    private static StopWatch stoper = new StopWatch();
    private static int IteracjaZnalezieniaSciezki = 0;
   // private static int nrOgolnyTest = 0;
    private static int[] sciezkaOptymalna;
    private static double dlugoscSciezkiOptymalnej = 0;

    static List<DaneIteracji> listaDanych = new List<DaneIteracji>();

    static DaneIteracji jednaIteracja(int liczbaMrowek, int[,] graf, double[,] TablicaFeromonu, int miastoStartowe, int nrWatku, Ant mrowka)
    {
        // pojedyncza iteracja jednej mrowki
        DaneIteracji daneIteracji = new DaneIteracji();

        for (int i = 0; i < liczbaMrowek; i++)
        {
            Ant instancja = mrowka;
            DaneZwracanePrzezMrowke daneMrowki = instancja.RozpocznijNastepnaIteracje(graf, TablicaFeromonu, miastoStartowe, i); //rozpoczecie obliczen
            List<int> trasaMrowki = daneMrowki.getSciezka();
            double dlugoscTrasy = daneMrowki.getDlugoscSciezki();
            daneIteracji.dodajSciezke(trasaMrowki, dlugoscTrasy);
            //Console.WriteLine(nrOgolnyTest+" W: " + nrWatku + ", M: " + nrMrowki + ": " + "D: " + Math.Round(dlugoscTrasy, 3));
            //nrOgolnyTest += 1;
            nrMrowki += 1;
        }
        nrRunningThreads -= 1;
        listaDanych.Add(daneIteracji);
        return daneIteracji;
    }

    public static void StartObliczen(Service1Client client)
    {

        int wielkoscGrafu = 0;
        int liczbaMrowek = 0;
        while (tablicagrafu.Length < 3) //program czeka az zostanie wygenerowany graf
        {
            tablicagrafu = client.ZwrocGraf();
            wielkoscGrafu = tablicagrafu.Length;

        }

        alfa = client.ZwrocAlfa();
        beta = client.ZwrocBeta();
       
        wielkoscGrafu = tablicagrafu.Length / 2;
        int[,] graf = new int[wielkoscGrafu, 2];

        for (int i = 0; i < wielkoscGrafu; i++) //zmiana grafu na forme do obliczen
        {
            graf[i, 0] = tablicagrafu[i]; 
            graf[i, 1] = tablicagrafu[wielkoscGrafu + i];
        }

        for (int a = 0; a < wielkoscGrafu; a++) //wypisanie otrzymanego grafu
        {
            Console.WriteLine("{0}, {1}", graf[a, 0], graf[a, 1]); 
        }
               
        while (liczbaMrowek < 1) 
        {
            liczbaMrowek = client.ZwrocLiczbeMrowek();

        }
        //Console.WriteLine("Liczba mrowek: " + liczbaMrowek);
        double[,] TablicaFeromonu = new double[wielkoscGrafu, wielkoscGrafu];



        // Pobranie feromonu od serwera
        double[] tablicaFeromonuOdSerwera = new double[wielkoscGrafu * wielkoscGrafu];
        tablicaFeromonuOdSerwera = client.ZwrocFeromon();
        int tmp = 0;
        for (int i = 0; i < wielkoscGrafu; i++) //zmiana otrzymanego feromonu na forme do obliczen
        {
            for (int j = 0; j < wielkoscGrafu; j++)
            {
                TablicaFeromonu[i, j] = tablicaFeromonuOdSerwera[tmp];
                tmp += 1;
            }

        }
        liczbaIteracji = client.ZwrocLiczbeIteracji();
        miastoStartowe = client.ZwrocMiastoStartowe();

        Console.WriteLine("Liczba miast: " + wielkoscGrafu);
        Console.WriteLine("Liczba mrowek: " + liczbaMrowek);
        Console.WriteLine("Liczba iteracji: " + liczbaIteracji);
        Console.WriteLine("alfa: " + alfa + ", beta: " + beta);


        int numberOfCores = Int32.Parse(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS")); //zczytanie liczby dostepnych watkow procesowa
        Console.WriteLine("Liczba dosteepnych watkow: " + numberOfCores);

        Thread[] ts = new Thread[numberOfCores]; //tablica watkow o zczytanej wielkosci

        double div = (double)liczbaMrowek / numberOfCores;  // obliczenie liczby mrowek na kazdy watek
        int numberOfAntsOnCore = (int)Math.Round(div + 0.05);
        Console.WriteLine("mrowki na rdzen: " + numberOfAntsOnCore);
        int czyStoperDziala = 1;
        long StoperTime = 0;
        stoper.WatchStart();


        for (int i = 0; i < liczbaIteracji; i++) 
        {
            nrMrowki = 0;
            int freeAnts = liczbaMrowek;
            for (int j = 0; j < numberOfCores; j++) // dla kazdego watku:
            {
                int temp = j;
                Ant mrowka = new Ant();

                // rozpoczcie obliczen w zaleznosci od mrowek na dany watek (ostatni watek ma czasami inna ilosc mrowek)
                if (liczbaMrowek == 1)
                {
                    if (j == 0)
                    {
                        ts[j] = new Thread(() => jednaIteracja(liczbaMrowek, graf, TablicaFeromonu, miastoStartowe, j, mrowka));
                    }
                    else
                    {
                        ts[j] = new Thread(() => jednaIteracja(0, graf, TablicaFeromonu, miastoStartowe, j, mrowka));
                    }
                }
                else

                if ((int)freeAnts < numberOfAntsOnCore)
                {

                    ts[j] = new Thread(() => jednaIteracja(freeAnts + numberOfAntsOnCore, graf, TablicaFeromonu, miastoStartowe, j, mrowka));

                }
                else

                if (j == numberOfCores - 1 && freeAnts / numberOfAntsOnCore < 2)
                {
                    ts[j] = new Thread(() => jednaIteracja(freeAnts + numberOfAntsOnCore, graf, TablicaFeromonu, miastoStartowe, j, mrowka));

                }
                else
                {
                    ts[j] = new Thread(() => jednaIteracja(numberOfAntsOnCore, graf, TablicaFeromonu, miastoStartowe, j, mrowka));

                }
                freeAnts = freeAnts - numberOfAntsOnCore;
                nrRunningThreads += 1; // zmienna liczaca ilosc dzialajacych watkow
                ts[j].Start();
                Thread.Sleep(30);
            }

            if (ts[ts.Length - 1].IsAlive == true)
                Thread.Sleep(1500);

            while (nrRunningThreads > 0)
            {
                Thread.Sleep(500); //pauza dopóki wszystkie wątki nie wykonają obliczeń
            }

            List<int> listaPolaczonychSciezek = new List<int>();
            List<double> listaDlugosciSciezek = new List<double>();
            double LacznaDlugoscSciezek = 0;

            foreach (DaneIteracji obiektNaLiscie in listaDanych)
            {

                foreach (List<int> sciezka in obiektNaLiscie.pobierzListeSciezek()) //dodanie sciezek do listy do wyslania 
                {
                    foreach (int element in sciezka)
                    {
                        listaPolaczonychSciezek.Add(element);                       
                    }                   
                }     

                foreach (double element in obiektNaLiscie.pobierzListeDlugosciSciezek()) //dodanie dlugosci sciezek do listy do wyslania
                {
                    listaDlugosciSciezek.Add(element);
                    LacznaDlugoscSciezek += element;
                }

            }

            int[] tablicaSciezek = listaPolaczonychSciezek.ToArray();  // zamiana na tablice
            double[] tablicaDlugosciSciezek = listaDlugosciSciezek.ToArray();


            if (czyStoperDziala == 1 && liczbaMrowek > 1) //jezeli stoper ciagle dziala 
            {

                if ((listaDlugosciSciezek.Any(o => o != listaDlugosciSciezek[0])) == false) //sprawdzenie czy wszystkie mrowki maja rowną trasę - warunek stopu
                {

                    sciezkaOptymalna = new int[wielkoscGrafu];
                    StoperTime = stoper.WatchStop();
                    czyStoperDziala = 0;
                    IteracjaZnalezieniaSciezki = i;
                    dlugoscSciezkiOptymalnej = listaDlugosciSciezek[0];
                    int index = 0;
                    for (int licz = 0; licz < wielkoscGrafu; licz++)
                    {
                        sciezkaOptymalna[index] = tablicaSciezek[licz];
                        index += 1;
                    }
                    Console.WriteLine(tablicaSciezek[tablicaSciezek.Length - wielkoscGrafu] + "  -->  " + tablicaSciezek[tablicaSciezek.Length - 1]); //wypisanie miasta startowego i koncowego

                }

            }

            
            tablicaFeromonuOdSerwera = client.AktualizujFeromonNew(tablicaSciezek, tablicaDlugosciSciezek, liczbaMrowek, numberOfCores); //wolanie o aktualizacje feromonu 
           
            tmp = 0;
            for (int i1 = 0; i1 < wielkoscGrafu; i1++) //zmiana na format do olbiczen
            {
                for (int j = 0; j < wielkoscGrafu; j++)
                {
                    TablicaFeromonu[i1, j] = tablicaFeromonuOdSerwera[tmp];
                    tmp += 1;
                }

            }           
            listaDlugosciSciezek.Clear();
            listaPolaczonychSciezek.Clear();
            listaDanych.Clear();
        }
        
       
        double stoper1 = 0;
        if (czyStoperDziala == 1)
        {
            StoperTime = stoper.WatchStop();

            stoper1 = (double)StoperTime / 1000;
            Console.WriteLine("Optymalna ścieżka nie znaleziona. Czas pracy: " + stoper1 + "s");
            string brakSciezki = "brak optymalnej sciezki";
            client.PrzeslijDaneDoZapisu(false, 0, stoper1, 0, brakSciezki, wielkoscGrafu, liczbaIteracji, liczbaMrowek, alfa, beta, getIP()); //przeslanie danych algorytmu
        }
        else
        {
            Console.Write("Sciezka optymalna: ");
            string sciezkaOptymalnaString = "";
            for (int i = 0; i < wielkoscGrafu; i++)
            {
                Console.Write(sciezkaOptymalna[i] + " ");
                sciezkaOptymalnaString = sciezkaOptymalnaString + " - " + sciezkaOptymalna[i].ToString();
            }
            Console.WriteLine();
            stoper1 = (double)StoperTime / 1000;
            Console.WriteLine("Optymalna sciezka znaleziona w czasie: " + stoper1 + "s, podczas iteracji: " + IteracjaZnalezieniaSciezki+", dl. sciezki: "+Math.Round(dlugoscSciezkiOptymalnej,4));


            client.PrzeslijDaneDoZapisu(true, IteracjaZnalezieniaSciezki, stoper1, dlugoscSciezkiOptymalnej, sciezkaOptymalnaString, wielkoscGrafu, liczbaIteracji, liczbaMrowek, alfa, beta, getIP()); //przeslanie danych algorytmu

        }





    }

    private static string getIP()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        string IpAddress = "";
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                IpAddress = ip.ToString();
            }
        }
        return IpAddress;
    }


    public static void Main()
    {
        Thread.Sleep(1000);
        string appPath = Application.StartupPath; // pobranie sciezki aplikacji do odczytu pliku 

        string pathHostName = appPath + "\\HostMachineName.txt"; // sciezka pliku  z nazwa hosta
        string readText = File.ReadAllText(pathHostName);
        IPAddress[] ipaddress = Dns.GetHostAddresses(readText); //pobranie IP hosta
        String IP = "";
        foreach (IPAddress ip in ipaddress)
        {
            if (IPAddress.Parse(ip.ToString()).AddressFamily == AddressFamily.InterNetwork)
                IP = ip.ToString();
        }

        // Tworzenie polaczenia
        BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        basicHttpBinding.CloseTimeout = new TimeSpan(00, 05, 00);
        basicHttpBinding.OpenTimeout = new TimeSpan(00, 05, 00);
        basicHttpBinding.ReceiveTimeout = new TimeSpan(00, 05, 00);
        basicHttpBinding.SendTimeout = new TimeSpan(00, 05, 00);
        basicHttpBinding.TextEncoding = System.Text.Encoding.UTF8;
        basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;
        basicHttpBinding.MaxBufferSize = int.MaxValue;
        Console.Write("Serwis dziala pod adresem: ");
        String addressOfHost = "http://" + IP + ":8001/myLib";
        Console.WriteLine(addressOfHost);

        EndpointAddress endpoint = new EndpointAddress(new Uri(addressOfHost));

        Service1Client client = new Service1Client(basicHttpBinding, endpoint);
        
        try
        {       

            IteracjaZnalezieniaSciezki = 0;
            int CzyZaczacObliczenia = 0;
            while (CzyZaczacObliczenia == 0) // czekam na wygenerowanie grafu
            {
                CzyZaczacObliczenia = client.startObliczen();
            }
            StartObliczen(client);

            //Zakonczenie polaczenia 
            Console.WriteLine("Press ENTER to exit:");
            Console.ReadLine();
            client.Close();
            Console.WriteLine("Done!");
        }
        catch (TimeoutException timeProblem)
        {
            Console.WriteLine("Timeout exeption. " + timeProblem.Message);
            Console.ReadLine();
            client.Abort();
        }

        catch (FaultException unknownFault)
        {
            Console.WriteLine("Nieznany wyjatek. " + unknownFault.Message);
            Console.ReadLine();
            client.Abort();
        }
        catch (CommunicationException commProblem)
        {
            Console.WriteLine("Problem z komunikacją. " + commProblem.Message + commProblem.StackTrace);
            Console.ReadLine();
            client.Abort();
        }

    }





}