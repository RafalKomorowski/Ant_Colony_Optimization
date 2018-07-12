using myLib;
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

namespace wcHost1
{
    public partial class Start : Form
    {
        private int liczbaMiast, miastoStartowe, liczbaIteracji, liczbaMrowek;
        private double alfa, beta;

        public Start()
        {
            InitializeComponent();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            // liczba mrowek
            string value = (sender as TextBox).Text;
            int myNumber = 0;

            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out myNumber);
                if (myNumber > 0)
                {
                    liczbaMrowek = myNumber;
                }
                else
                {
                    MessageBox.Show("Podaj poprawne wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            //liczba miast old
            string value = (sender as TextBox).Text;
            int myNumber = 0;

            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out myNumber);
                if (myNumber > 0)
                {
                    liczbaMiast = myNumber;
                }
                else
                {
                    MessageBox.Show("Podaj poprawne wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.rysowanieStart = 10;
        }




        private void Start_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

     

        private void textBox10_TextChanged_1(object sender, EventArgs e)
        {
            //miasto startowe
            string value = (sender as TextBox).Text;
            int myNumber = 0;

            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out myNumber);
                if (myNumber >= 0 && myNumber<liczbaMiast)
                {
                    miastoStartowe = myNumber;
                }
                else
                {
                    MessageBox.Show("Podaj poprawne wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox4.Text = "0";
                }
            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            //liczba iteracji
            string value = (sender as TextBox).Text;
            int myNumber = 0;


            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out myNumber);
                if (myNumber > 0)
                {
                    liczbaIteracji = myNumber;
                }
                else
                {
                    MessageBox.Show("Podaj poprawne wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //liczba miast
            string value = (sender as TextBox).Text;
            int myNumber = 0;

            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out myNumber);
                if (myNumber > 0)
                {
                    liczbaMiast = myNumber;
                }
                else
                {
                    MessageBox.Show("Podaj poprawne wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                textBox4.Enabled = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Service1.daneDoRysowania = new muLib.DaneDoRysowania();
            Service1.ZainicjujFeromon(this.liczbaMiast);
            Service1.liczbaIteracji = this.liczbaIteracji;
            Service1.LiczbaNowychMrowek = this.liczbaMrowek;
            Program.miastoStartowe = this.miastoStartowe;
            Program.wielkoscGrafu = this.liczbaMiast;
            this.alfa = (double)numericUpDown1.Value;
            Service1.alfa = this.alfa;
            this.beta = (double)numericUpDown2.Value;
            Service1.beta = this.beta;
            Service1.CzyZaczacObliczenia = 2;
            
            Thread.Sleep(1000);
            Service1.CzyZaczacObliczenia = 0;


            button2.Enabled = true;
            
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Service1.ZainicjujFeromon(this.liczbaMiast);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // zmiana wartosci bez zmiany grafu 
            Service1.liczbaIteracji = this.liczbaIteracji;
            Service1.LiczbaNowychMrowek = this.liczbaMrowek;
            Service1.miastoStartowe = this.miastoStartowe;
            this.alfa = (double)numericUpDown1.Value;
            Service1.alfa = this.alfa;
            this.beta = (double)numericUpDown2.Value;
            Service1.beta = this.beta;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // generowanie grafu
            button1.Enabled = true;
            Program.wielkoscGrafu = liczbaMiast;
            Program.GenerujNowyGraf(liczbaMiast);
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //Rysowanie miast 
            
            Thread thr = new Thread(Service1.doSomething);
            thr.Start();
        }

        private void Start_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

            // testowy pod rysowaniem
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // liczba mrowek
            string value = (sender as TextBox).Text;
            int myNumber = 0;

            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out myNumber);
                if (myNumber > 0)
                {
                    liczbaMrowek = myNumber;
                }
                else
                {
                    MessageBox.Show("Podaj poprawne wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //liczba iteracji
            string value = (sender as TextBox).Text;
            int myNumber = 0;


            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out myNumber);
                if (myNumber > 0)
                {
                    liczbaIteracji = myNumber;
                }
                else
                {
                    MessageBox.Show("Podaj poprawne wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            //miasto startowe
            string value = (sender as TextBox).Text;
            int myNumber = 0;

            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out myNumber);
                if (myNumber >= 0)
                {
                    miastoStartowe = myNumber;
                }
                else
                {
                    MessageBox.Show("Podaj poprawne wartości", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.liczbaIteracji = this.liczbaIteracji;
            Program.liczbaMiast = this.liczbaMiast;
            Service1.LiczbaNowychMrowek = this.liczbaMrowek;
            Program.miastoStartowe = this.miastoStartowe;
            Program.wielkoscGrafu = this.liczbaMiast;                   
            this.alfa = (double)numericUpDown1.Value;
            Service1.alfa = this.alfa;
            this.beta = (double)numericUpDown2.Value;
            Service1.beta = this.beta;

            button2.Visible = true;



        }
    }
}
