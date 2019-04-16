﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EpIA
{
	public partial class Form1 : Form
	{
        DataTable chartdt = new DataTable();
        DataTable dt = new DataTable();
        DataRow dr;
        double x;
		double y;

        double fitness;
        double probabilidadeMutacao = 5;
        double mediafitness;

        List<String> selecionadaosx;
        List<String> selecionadaosy;
	    List<String> filhos1;
        List<String> filhos2;
        private int countMutacao = 0;
        private double melhor;

        public Form1()
		{

			InitializeComponent();
            DataColumn binx = new DataColumn("binx");//TODO: Mudar de dt para Lista
            DataColumn biny = new DataColumn("biny");
            DataColumn fitness = new DataColumn("fitness");
            dt.Columns.Add(binx);
            dt.Columns.Add(biny);
            dt.Columns.Add(fitness);

            DataColumn generation = new DataColumn("generation");//TODO: Mudar de dt para Lista
            DataColumn max = new DataColumn("max");
            DataColumn fitness2 = new DataColumn("fitness");
            chartdt.Columns.Add(generation);
            chartdt.Columns.Add(max);
            chartdt.Columns.Add(fitness2);


            #region População inicial
            dr = dt.NewRow();
            dr["binx"] = "0000010110";
            dr["biny"] = "0011101110";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "1000001100";
            dr["biny"] = "1010111110";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "0101000001";
            dr["biny"] = "0101101111";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "0111110001";
            dr["biny"] = "0111000100";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "1011110000";
            dr["biny"] = "0111010101";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "0100010010";
            dr["biny"] = "1010010111";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "1101101011";
            dr["biny"] = "0000010111";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "0101100000";
            dr["biny"] = "1101100000";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "1001110010";
            dr["biny"] = "0110111011";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["binx"] = "0011001110";
            dr["biny"] = "1010110011";
            dt.Rows.Add(dr);

            #endregion

        }

        private void Button1_Click(object sender, EventArgs e)
		{
            int Geracoes = 1;
            for (int i = 0; i < Geracoes; i++)
            {
                DataRow row = chartdt.NewRow();
                row["generation"] = i;
                if (i == 0) // Le da matriz apenas na primeira vez
                {
                    for (int j = 0; j < 10; j++)
                    {
                        dr = dt.Rows[j];
                        string xString = (string)dr["binx"];
                        string yString = (string)dr["biny"];
                        bintodec(xString, yString);
                        Rodar_e_fitness();
                        
                    }
                }
                
                SelecaoTorneio(); // Seleciona da table e coloca nos selecionados
                CrossOver(); // Faz crossover dos selecionados
                Mutacao(); // Muta os selecionados
                MediaFitness();
                row["fitness"] = mediafitness;
                Melhor();
                row["max"] = melhor;
                chartdt.Rows.Add(row);
                chart1.DataSource = chartdt;
                chart1.Series["Series1"].XValueMember = "genaration";
                chart1.Series["Series1"].YValueMembers = "fitness";
                chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            }
        }

        private void Melhor()
        {
            melhor = dt.AsEnumerable().Select(r => r.Field<double>("Fitness")).Max();
        }

        private void MediaFitness()
        {
            double soma = dt.AsEnumerable().Select(r => r.Field<double>("Fitness")).Sum();
            mediafitness = soma / 10;
        }

        private void Mutacao()
        {

            for (int i = 0; i < selecionadaosx.Count; i++)
            {
                Random rand = new Random();
                if (rand.Next(0,10000) < probabilidadeMutacao )
                {
                    countMutacao++;
                    var cromossomoarray = selecionadaosx[i].ToCharArray();
                    int sorteado = rand.Next(0, 4);

                    if (selecionadaosx[i][sorteado] == '1')
                    {
                        cromossomoarray[sorteado] = '0';
                    }
                    else
                    {
                        cromossomoarray[sorteado] = '1';
                    }

                    selecionadaosx.RemoveAt(i);
                    selecionadaosx.Add(cromossomoarray.ToString());
                }
            }
            for (int i = 0; i < selecionadaosy.Count; i++)
            {
                Random rand = new Random();
                if (rand.Next(0, 100) < probabilidadeMutacao)
                {
                    var cromossomoarray = selecionadaosy[i].ToCharArray();
                    int sorteado = rand.Next(0, 4);

                    if (selecionadaosy[i][sorteado] == '1')
                    {
                        cromossomoarray[sorteado] = '0';
                    }
                    else
                    {
                        cromossomoarray[sorteado] = '1';
                    }

                    selecionadaosy.RemoveAt(i);
                    selecionadaosy.Add(cromossomoarray.ToString());
                }
            }
        }

        private void CrossOver()
        {
            filhos1 = new List<String>();
            filhos2 = new List<String>();

            int chanceCruzamento = 70;
            var rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                int cruza = rand.Next(1, 100);

                if (cruza < chanceCruzamento)
                {
                    int pontoDivisao = rand.Next(1, 19);

                    string parte_x1 = selecionadaosx[i].Substring(0, pontoDivisao);
                    string parte_y1 = selecionadaosx[i].Substring(pontoDivisao, 20 - pontoDivisao);

                    string parte_x2 = selecionadaosy[i].Substring(0, pontoDivisao);
                    string parte_y2 = selecionadaosy[i].Substring(pontoDivisao, 20 - pontoDivisao);

                    filhos1[i] = parte_x1 + parte_y2;
                    filhos2[i] = parte_x2 + parte_y1;
                }
		else
                {
                    filhos1[i] = selecionadaosx[i];
                    filhos2[i] = selecionadaosy[i];
                }
            }
        }

        private void SelecaoTorneio()
        {
            selecionadaosx = new List<String>();
            selecionadaosy = new List<String>();
            while (selecionadaosx.Count < 5 || selecionadaosy.Count < 5)
            {
                var rand = new Random();
                var sorteadosx = new List<String>();
                var sorteadosy = new List<String>();
                for (int i = 0; i < 3; i++)
                {
                    int sorteadox = rand.Next(0, 9);
                    int sorteadoy = rand.Next(0, 9);
                    sorteadosx.Add((string)dt.Rows[sorteadox]["fitness"]);
                    sorteadosy.Add((string)dt.Rows[sorteadoy]["fitness"]);
                }
                if (!selecionadaosx.Contains(sorteadosx.Max()) && selecionadaosx.Count < 5)
                {
                    selecionadaosx.Add(sorteadosx.Max());
                }

                if (!selecionadaosy.Contains(sorteadosy.Max()) && selecionadaosy.Count < 5)
                {
                    selecionadaosy.Add(sorteadosy.Max());
                }

            } 

           
        }

        

        private void bintodec(string xString, string yString)
		{
			
            x = Convert.ToInt32(xString,2);
			y = Convert.ToInt32(yString, 2);

			x = (x * 0.00978) -5;
			y = (y * 0.00978) - 5;

			x = Math.Round(x, 2);
			y = Math.Round(y, 2);
            



        }

        private void Rodar_e_fitness()
        {
            
                var result = 20 + Math.Pow(x, 2) + Math.Pow(y, 2) - 10 * (Math.Cos(2 * Math.PI * x) + Math.Cos(2 * Math.PI * y));
                fitness = 1 / result;
                dr["fitness"] = fitness;
                
        }
    }
}
