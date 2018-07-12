using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace myLib
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        int Twice(int value);

        [OperationContract]
        int[] GenerownieGrafu(int i);
        [OperationContract]
        void AktualizujFeromon(int[] sciazka, double dlugoscTrasy);

        [OperationContract]
        double[] AktualizujFeromonNew(int[] TablicaSciezekPolaczone, double[] tablicaDlugosciSciezek, int liczbaMrowek, int iloscWatkow);

        [OperationContract]
        int[] ZwrocGraf();
        [OperationContract]
        double[] ZwrocFeromon();
        [OperationContract]
        int ZwrocLiczbeIteracji();
        [OperationContract]
        int ZwrocMiastoStartowe();
        [OperationContract]
        int ZwrocLiczbeMrowek();

        [OperationContract]
        double ZwrocAlfa();
        [OperationContract]
        double ZwrocBeta();

        [OperationContract]
        void startRysowanie();

        [OperationContract]
        int startObliczen();

        [OperationContract]
        void PrzeslijDaneDoZapisu(bool czyOptymalna ,int iteracja, double czas, double dlugoscTrasy, string sciezka,int liczbaMiast, int liczbaIteracji, int liczbaMrowek,double alfaparm, double betaParm, string Ip);


        [OperationContract]
        void PrzeslijStatystyke(int iteracjaZnalezienia, double czasZnalezienia);




        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "myLib.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
