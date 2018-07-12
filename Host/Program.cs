using myLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace wcHost1
{
    class Program
    {
        public static int wielkoscGrafu = 0;
        public static int liczbaMrowek = 0;
        public static int liczbaMiast = 0;
        public static int liczbaIteracji = 0;
        public static int miastoStartowe = 0;
        public static int rysowanieStart = 0;
        public static int[] graf = { 1 };

        public static void FormularzDanych()  //metoda wywolujaca formularz do zmiany dancyh
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Start());
        }

        public static void RysowanieStart() // metoda wywolujaca formularz rysowania
        {
            Service1.doSomething();
        }

        public static void GenerujNowyGraf(int wielkoscGrafu)  // generowanie nowego grafu z formularza
        {
            Console.WriteLine("Wielkosc grafu: {0}", wielkoscGrafu);
            Service1 instancja_klasy = new Service1();
            graf = instancja_klasy.GenerownieGrafu(wielkoscGrafu);           
            Console.WriteLine("Wygenerowane punkty grafu: ");
            for (int a = 0; a < wielkoscGrafu; a++)
            {
                Console.WriteLine("{0}, {1}", graf[a], graf[wielkoscGrafu + a]);
            }
        }

        public static string GetMyIP()  //zczytanie IP do utworzenia URI
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("brak  IPv4 w systemie!");
        }

        static void Main(string[] args)
        {

            string HostName = Dns.GetHostName();
            Console.WriteLine("Nazwa komputera= " + HostName);
            Thread thr = new Thread(FormularzDanych);
            thr.Start();
            // Tworzenie polaczenia
            string MyIp = GetMyIP();
            string uri = "http://" + MyIp + ":8001/myLib";            

            ServiceHost svcHost = new ServiceHost(typeof(Service1), new Uri(uri));
            try
            {
                // Sprawdznie dzialajacych servisow
                ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                // jezeli brak to dodaj nowy
                if (smb == null)
                    smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                svcHost.Description.Behaviors.Add(smb);

                // dodawanie endpoint'a
                svcHost.AddServiceEndpoint(
                  ServiceMetadataBehavior.MexContractName,
                  MetadataExchangeBindings.CreateMexHttpBinding(),
                  "mex"
                );

                // Ustawnienie wlasciwosci polaczenia
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                basicHttpBinding.CloseTimeout = new TimeSpan(00, 05, 00);
                basicHttpBinding.OpenTimeout = new TimeSpan(00, 05, 00);
                basicHttpBinding.ReceiveTimeout = new TimeSpan(00, 05, 00);
                basicHttpBinding.SendTimeout = new TimeSpan(00, 05, 00);
                basicHttpBinding.TextEncoding = System.Text.Encoding.UTF8;
                basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;
                basicHttpBinding.MaxBufferSize = int.MaxValue;

                svcHost.AddServiceEndpoint(typeof(IService1), basicHttpBinding, "");
                // otwarcie hosta na polaczenia
                svcHost.Open();
                               
                Console.WriteLine("Serwis dziala...");
               
                // program czeka az zostanie wygenerowany graf
                while (wielkoscGrafu < 3)
                {

                }
               
                Console.WriteLine("Wielkosc grafu: {0}", wielkoscGrafu);
                Service1 instancja_klasy = new Service1();
                Console.WriteLine(Service1.alfa);
                Console.WriteLine(Service1.beta);
                Console.WriteLine(Service1.LiczbaNowychMrowek);

                // program czeka na wywolanie rysowania
                while (rysowanieStart < 5)
                {

                }
                
                RysowanieStart();
                
                // zamkniecie polaczenia
                svcHost.Close();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine("Problem z polaczeniem. " + commProblem.Message);
                Console.Read();
            }

        }
    }
}
