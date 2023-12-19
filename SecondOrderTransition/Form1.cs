using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SecondOrderTransition
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random obj = new Random();
            double r, M = 0, Mave = 0, mu = 0, H = 0;
            Graphics gg = CreateGraphics();
            SolidBrush bred = new SolidBrush(Color.AliceBlue);
            SolidBrush bblue = new SolidBrush(Color.Blue);
            
            //Take two dimensional spin system
            //For this purpose take a two dimensional array
            int n_spins = 40;
            int[,] spinsystem = new int[n_spins, n_spins];
            //Show the spin system, having spin down as -1 and spin up as +1.

            for (int i = 0; i < n_spins; i++)
            {
                for (int j = 0; j < n_spins; j++)
                {
                    r = obj.NextDouble();
                    if (r <= 0.5)
                    {
                        spinsystem[i, j] = 1;//spin up
                        gg.FillEllipse(bred, 100 + j * 10, 100 + i * 10, 8, 8);
                    }
                    else//spin down
                    {
                        spinsystem[i, j] = -1;//spin down
                        gg.FillEllipse(bblue, 100 + j * 10, 100 + i * 10, 8, 8);

                    }
                    //Thread.Sleep(2);
                }
            }//spinsystem is initialized

            //Start changing temperature of the spin system and let the system 
            //attain thermal equilibrium at each temperature
            double T = 4.2, J = 1, KB = 1, E1, E2, E_Flip, P_Flip;
            int n_sweeps = 1;
            for (T = 4.2; T > 0.1; T = T - 0.02)
            {
                Mave = 0;
                for (int sweep = 0; sweep < n_sweeps; sweep++)
                //number of sweeps for the system
                //going into thermal equilibrium state at each temperature
                {

                    for (int i = 1; i < n_spins - 1; i++)
                    //check flipping of each spin during
                    //each sweep at every temeprature
                    {
                        for (int j = 1; j < n_spins - 1; j++)
                        {

                            //compute energy of each spin before and after flipping
                            //E1 = CalE(spinsystem, i, j, mu, H);//energy before flipping
                                                               //assume that the spin is flipped
                            E1 = (-spinsystem[i, j] * (spinsystem[i - 1, j] + spinsystem[i + 1, j]
                                + spinsystem[i, j - 1] + spinsystem[i, j + 1] + mu * H));

                            spinsystem[i, j] = spinsystem[i, j] * -1;
                            //energy after flipping
                            // E2 = CalE(spinsystem, i, j, mu, H);
                            E2 = (-spinsystem[i, j] * (spinsystem[i - 1, j] + spinsystem[i + 1, j]
                                + spinsystem[i, j - 1] + spinsystem[i, j + 1] + mu * H));

                            E_Flip = E2 - E1;
                            if (E_Flip <= 0)//the spin will flip
                            {
                                //accept flipping
                                if (spinsystem[i, j] == -1)
                                { 
                                    gg.FillEllipse(bblue, 100 + j * 10, 100 + i * 10, 8, 8); 
                                }
                                else
                                {
                                    gg.FillEllipse(bred, 100 + j * 10, 100 + i * 10, 8, 8);
                                }

                            }
                            else//if E_flip>0
                            {
                                P_Flip = Math.Exp(-E_Flip / (KB * T));
                                r = obj.NextDouble();//random probability
                                if (P_Flip >= r)
                                {
                                    //accept flipping
                                    if (spinsystem[i, j] == -1)
                                    {
                                        gg.FillEllipse(bblue, 100 + j * 10, 100 + i * 10, 8, 8);
                                    }
                                    else
                                    {
                                        gg.FillEllipse(bred, 100 + j * 10, 100 + i * 10, 8, 8);
                                    }
                                }
                                else//if P_Flip<r
                                {
                                    //reject the assumed flipping
                                    spinsystem[i, j] = spinsystem[i, j] * -1;
                                }
                                M = M + spinsystem[i, j];

                            }//else end
                        }//for end
                    }//for end
                    M = M / (n_spins * n_spins - 4 * n_spins + 4);
                    Mave = Mave + M;
                    M = 0;
                }//end of sweeps

                Mave = Mave / n_sweeps;
                for (double s = -1.2; s < 1.2; s = s + 0.1)//MFT Phase Transition
                {
                    //if (Math.Abs(s - Math.Tanh(J * 4 * s / (KB * T))) < 0.01)
                    //{
                    //    gg.FillEllipse(bred, 600 + (float)T * 100, 200 - (float)s * 300, 8, 8);
                    //}
                }
                gg.FillEllipse(bblue, 700 + (float)T * 100, 320 - (float)Mave * 300, 8, 8);


            }//end of temperature loop
        }//end of event handler

        //public double CalE(int[,] spinsystem, int row, int col, double mu, double H)
        //{
        //    return (-spinsystem[row, col] * (spinsystem[row - 1, col] + spinsystem[row + 1, col]
        //        + spinsystem[row, col - 1] + spinsystem[row, col + 1] + mu * H));
        //}
    }//end of class Form1
}//end of namespace
