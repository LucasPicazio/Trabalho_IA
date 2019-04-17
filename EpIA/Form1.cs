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

        DataRow[] selecionadaosx;
        DataRow[] selecionadaosy;
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
            DataColumn selecionadox = new DataColumn("selecionadox");
            DataColumn selecionadoy = new DataColumn("selecionadoy");
            fitness.DataType = typeof(double);
            dt.Columns.Add(binx);
            dt.Columns.Add(biny);
            dt.Columns.Add(fitness);
            dt.Columns.Add(selecionadoy);

            DataColumn generation = new DataColumn("generation");//TODO: Mudar de dt para Lista
            DataColumn max = new DataColumn("max");
            DataColumn fitness2 = new DataColumn("fitness");
            fitness2.DataType = typeof(double);
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

                    for (int j = 0; j < 10; j++)
                    {
                        dr = dt.Rows[j];
                        string xString = (string)dr["binx"];
                        string yString = (string)dr["biny"];
                        bintodec(xString, yString);
                        Rodar_e_fitness();
                    }
                
                
                SelecaoTorneio(); // Seleciona da table e coloca nos selecionados
                CrossOver(); // Faz crossover dos selecionados
                Mutacao(); // Muta os selecionados
                MediaFitness();
                row["fitness"] = mediafitness;
                Melhor();
                row["max"] = melhor;
                chartdt.Rows.Add(row);
                ListtoDataTable();
                dataGridView2.DataSource = dt;
            }
            chart1.DataSource = chartdt;
            chart1.Series["Series1"].XValueMember = "generation";
            chart1.Series["Series1"].YValueMembers = "fitness";
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            dataGridView1.DataSource = chartdt;
        }

        private void ListtoDataTable()
        {
            for (int i = 0; i < 4; i++)
            {

            }
        }

        private void Melhor()
        {
            melhor = dt.AsEnumerable().Select(r => r.Field<double>("fitness")).Max();
        }

        private void MediaFitness()
        {
            double soma = dt.AsEnumerable().Select(r => r.Field<double>("fitness")).Sum();
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

            for (int i = 0; i < 2; i++)
            {
                int cruza = rand.Next(1, 100);

                if (cruza < chanceCruzamento)
                {
                    int pontoDivisao = rand.Next(1, 10);

                    //Pega a segunda parte 
                    string parte2_x = selecionadaosx[i].Substring(pontoDivisao, 10 - pontoDivisao);
                    string parte2_x1 = selecionadaosx[i+1].Substring(pontoDivisao, 10 - pontoDivisao);

                    string parte2_y = selecionadaosy[i].Substring(pontoDivisao, 10 - pontoDivisao);
                    string parte2_y1 = selecionadaosy[i+1].Substring(pontoDivisao, 10 - pontoDivisao);

                    // coloca nos lugares
                    selecionadaosx[i] = selecionadaosx[i].Substring(0, pontoDivisao) + parte2_x1;
                    selecionadaosx[i+1] = selecionadaosx[i+1].Substring(0, pontoDivisao) + parte2_x;

                    selecionadaosy[i] = selecionadaosy[i].Substring(0, pontoDivisao) + parte2_y;
                    selecionadaosy[i + 1] = selecionadaosy[i + 1].Substring(0, pontoDivisao) + parte2_y1;
                }

            }
        }

        private void SelecaoTorneio()
        {

            while (selecionadaosx.Count() < 4 || selecionadaosy.Count() < 4)
            {
                selecionadaosx = dt.Select("selecionadox = 1");
                selecionadaosy = dt.Select("selecionadoy = 1");

                var rand = new Random();
                var sorteadosx = new List<String>();
                var sorteadosy = new List<String>();
                for (int i = 0; i < 3; i++)
                {
                    int sorteadox = rand.Next(0, 9);
                    int sorteadoy = rand.Next(0, 9);
                    sorteadosx.Add((string)dt.Rows[sorteadox]["binx"]);
                    sorteadosy.Add((string)dt.Rows[sorteadoy]["biny"]);
                }
                if (!ContainsDataRow(selecionadaosx, sorteadosx.Max()) && selecionadaosx.Count() < 4)
                {
                    dt.Select("binx = '" + sorteadosx.Max() + "'").FirstOrDefault()["selecionado"] = 1;
                }

                if (!ContainsDataRow(selecionadaosy, sorteadosy.Max()) && selecionadaosy.Count() < 4)
                {
                    dt.Select("binx = '" + sorteadosx.Max() + "'").FirstOrDefault()["selecionado"] = 1;
                }

            } 

           
        }

        private bool ContainsDataRow(DataRow[] selecionadaosy, string v)
        {
            throw new NotImplementedException();
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
