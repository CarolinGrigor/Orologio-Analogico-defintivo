using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bozza1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(paint);
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void paint(object sender, PaintEventArgs e)
        {
            #region IMPOSTAZIONI
            Brush colore = new SolidBrush(Color.Black);
            int raggio = Math.Min(ClientSize.Width, ClientSize.Height) / 2 - 10;
            int centroX = ClientSize.Width / 2;
            int centroY = ClientSize.Height / 2;
            double angolo;
            float spessore;
            int x1Tacchetta, y1Tacchetta, x2Tacchetta, y2Tacchetta;
            int dimensioneMinima = Math.Min(ClientSize.Width, ClientSize.Height);
            float Scala = (float) dimensioneMinima / 300;
            string numero = "";
            float xNumero, yNumero, dimensioneTesto, raggioInterno;
            DateTime orario;
            int ore, minuti, secondi;
            int diametroCerchio, centroXCerchio, centroYCerchio;
            Font font;
            SizeF dimensioneNumero;
            Pen trattocerchio = new Pen(Color.Black, 4 * Scala);
            Graphics g = e.Graphics;
            #endregion

            #region CERCHIO BASE
            g.DrawEllipse(trattocerchio, centroX - raggio, centroY - raggio, raggio * 2, raggio * 2);
            #endregion

            #region DISEGNO TACCHETTE E NUMERI
            for (int i = 0; i < 12; i++)
            {
                spessore = 2 * Scala;
                if (i % 3 == 0)
                {
                    // ingrandisce lo spessore delle tacchette alle ore 12, 3, 6, 9
                    spessore = 4 * Scala;
                }
                angolo = (i * 30 * Math.PI / 180) - Math.PI / 2;
                // calcolo coordinate punto più esterno della tacchetta
                x1Tacchetta = (int)(centroX + raggio * Math.Cos(angolo));
                y1Tacchetta = (int)(centroY + raggio * Math.Sin(angolo));
                // calcolo coordinate punto più interno della tacchetta
                x2Tacchetta = (int)(centroX + (raggio - 15 * Scala) * Math.Cos(angolo));
                y2Tacchetta = (int)(centroY + (raggio - 15 * Scala) * Math.Sin(angolo));
                g.DrawLine(new Pen(Color.Black, spessore), x1Tacchetta, y1Tacchetta, x2Tacchetta, y2Tacchetta);

                switch (i)
                {
                    case 0:
                        numero = "12";
                        break;
                    case 1:
                        numero = "1";
                        break;
                    case 2:
                        numero = "2";
                        break;
                    case 3:
                        numero = "3";
                        break;
                    case 4:
                        numero = "4";
                        break;
                    case 5:
                        numero = "5";
                        break;
                    case 6:
                        numero = "6";
                        break;
                    case 7:
                        numero = "7";
                        break;
                    case 8:
                        numero = "8";
                        break;
                    case 9:
                        numero = "9";
                        break;
                    case 10:
                        numero = "10";
                        break;
                    case 11:
                        numero = "11";
                        break;
                }

                // regola la posizione interna dei numeri
                raggioInterno = raggio * 0.8f; 
                // calcolo coordinate della posizione del numero
                xNumero = centroX + (float)(raggioInterno * Math.Cos(angolo));
                yNumero = centroY + (float)(raggioInterno * Math.Sin(angolo));
                dimensioneTesto = 20 * Scala;
                font = new Font("Arial", dimensioneTesto);
                // MeasureString -> misura una string in base a un testo dato e il font
                dimensioneNumero = g.MeasureString(numero, font);
                xNumero -= dimensioneNumero.Width / 2;
                yNumero -= dimensioneNumero.Height / 2; 
                g.DrawString(numero, font, colore, xNumero, yNumero);
            }
            #endregion

            #region ACQUISIZIONE ORARIO
            orario = DateTime.Now;
            ore = orario.Hour % 12;
            minuti = orario.Minute;
            secondi = orario.Second;
            #endregion

            #region DISEGNO LANCETTE
            // (ore + minuti / 60) * 30 -> angolo della lancetta delle ore
            /* es: 3:15 -> la lancetta dei minuti sarà su 3, mnetre la 
               lancetta delle ore avrà già compiuto 1/4 dei 30° che
               doveva percorrere */
            DisegnoLancetta(g, centroX, centroY, raggio * 0.5, (ore + minuti / 60) * 30, Pens.Black, 6 * Scala);
            DisegnoLancetta(g, centroX, centroY, raggio * 0.8, minuti * 6, Pens.Black, 4 * Scala);
            DisegnoLancetta(g, centroX, centroY, raggio * 0.9, secondi * 6, Pens.Red, 2 * Scala);
            #endregion

            #region DISEGNO CERCHIO CENTRALE
            diametroCerchio = (int)(10 * Scala);
            centroXCerchio = centroX - diametroCerchio / 2;
            centroYCerchio = centroY - diametroCerchio / 2;
            g.FillEllipse(colore, centroXCerchio, centroYCerchio, diametroCerchio, diametroCerchio);
            #endregion
        }

        private void DisegnoLancetta(Graphics g, int x, int y, double lunghezzaLancetta, double angolo, Pen penna, float spessore)
        {
            #region CALCOLO COORDINATE SECONDO PUNTO
            double radianti = (Math.PI * angolo) / 180;
            // x+ e y- in modo che le lancette siano a contenute all'interno
            int x2 = (int)(x + lunghezzaLancetta * Math.Sin(radianti));
            int y2 = (int)(y - lunghezzaLancetta * Math.Cos(radianti));
            g.DrawLine(new Pen(penna.Color, spessore), x, y, x2, y2);
            #endregion
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}