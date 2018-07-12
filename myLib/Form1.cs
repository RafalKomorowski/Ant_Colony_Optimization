using muLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myLib
{
    public partial class Form1 : Form
    {
        Point[] pts = { new Point(10, 10), new Point(105, 105), new Point(200, 300), new Point(4, 380), new Point(100, 50), new Point(380, 125), new Point(300, 10), new Point(40, 22), new Point(120, 570), new Point(320, 188), new Point(120, 357) };
        public static List<Point> listaPunktow = new List<Point>();
        public static Point[] ptsTest;
        private int LiczbaMrowek = 0;


        public static List<int> listaPrzezroczystosci = new List<int>();
        public static int liczbaMiast;
        //static bool zegar = false;
        // static int nrListy = 0;
        static int zmiennaDonumerowaniaList = 0;

        public static List<List<List<int>>> listaListIteracji = new List<List<List<int>>>();
        public static List<List<int>>[] tabelatak = new List<List<int>>[2];

        public static int[,,] tablicaSciezekAll = new int[4, 8, 6];
        static Pen semiTransPen ;


        public Form1(DaneDoRysowania dane, int liczbaMrowek)
        {
            LiczbaMrowek = liczbaMrowek;
            zmiennaDonumerowaniaList = 0;

            if (LiczbaMrowek < 8)
            {
                semiTransPen = new Pen(Color.FromArgb(60, 255, 0, 0), 3);
            }
            else
            if(LiczbaMrowek>7)
            {
                semiTransPen = new Pen(Color.FromArgb(2, 255, 0, 0), 3);
            }

            DoubleBuffered = true;
            InitializeComponent();

            listaListIteracji = dane.pobierzListeIteracji();

            timer1.Enabled = true;

            timer1.Interval = 2;
            timer1.Tick += new EventHandler(test);



        }

        //int iks = 0;
        // int igrek = 0;
       // static int tmp = 0;

        Color semiRed1 = Color.FromArgb(40, Color.Red);

        private void test(object sender, EventArgs e)
        {
            this.Invalidate();

            if (zmiennaDonumerowaniaList < listaListIteracji.Count - 1)
            {
               
                zmiennaDonumerowaniaList += 1;

            }
           

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Console.WriteLine("Lista przez: " + listaSciezek.Count);
            Graphics g = e.Graphics;

          

           // tmp = 0;

            //foreach (List<int> sublist in listaList)
            {
                Console.WriteLine("ZmiennaDo zmiany: " + zmiennaDonumerowaniaList);
                Console.WriteLine("listaList[zmiennaDonumerowaniaList].Count : " + listaListIteracji.ElementAt(zmiennaDonumerowaniaList).Count);

                for (int j = 0; j < listaListIteracji[zmiennaDonumerowaniaList].Count; j++)
                {
                    for (int i = 0; i < listaListIteracji[zmiennaDonumerowaniaList][j].Count - 1; i++)
                        g.DrawLine(semiTransPen, ptsTest[listaListIteracji[zmiennaDonumerowaniaList][j][i]], ptsTest[listaListIteracji[zmiennaDonumerowaniaList][j][i + 1]]);

                }



            }

            Font font = new Font("Arial", 16);
            SolidBrush s = new SolidBrush(Color.White);
            int tmpk = 0;
            foreach (Point element in ptsTest) //ptsTest
            {
                g.FillRectangle(Brushes.Black, element.X - 9, element.Y - 9, 18, 18);
                String numer = tmpk.ToString();
                g.DrawString(numer, font, s, element.X - 10, element.Y - 11);
                tmpk += 1;

            }
            // g.FillRectangles(Brushes.Black, rec);

            base.OnPaint(e);
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            Invalidate();
            zmiennaDonumerowaniaList = 0;
        }





        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("test");
        }

        public String testowas()
        {
            String s = "gdgdgdg";
            return s;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_2(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Invalidate();
            Invalidate();
            zmiennaDonumerowaniaList = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zmiennaDonumerowaniaList = listaListIteracji.Count -1;
        }
    }
}
