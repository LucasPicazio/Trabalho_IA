﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EpIA
{
    public partial class Form1 : Form
	{
        DataTable chartdt = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtantigo = new DataTable();
        DataRow dr;
        double x;
		double y;
        int pares;
        double fitness;
        double probabilidadeMutacao = 5;
        double mediafitness;

        DataRow[] selecionadaosx;
        DataRow[] selecionadaosy;

        private int countMutacao = 0;
        private double melhor;

        public Form1()
		{

			InitializeComponent();
            DataColumn binx = new DataColumn("binx");
            DataColumn biny = new DataColumn("biny");
            DataColumn fitness = new DataColumn("fitness");
            DataColumn selecionadox = new DataColumn("selecionadox");
            DataColumn selecionadoy = new DataColumn("selecionadoy");
            fitness.DataType = typeof(double);
            dt.Columns.Add(binx);
            dt.Columns.Add(biny);
            dt.Columns.Add(fitness);
            dt.Columns.Add(selecionadox);
            dt.Columns.Add(selecionadoy);

            DataColumn generation = new DataColumn("generation");
            DataColumn max = new DataColumn("max");
            DataColumn fitness2 = new DataColumn("fitness");
            fitness2.DataType = typeof(double);
            max.DataType = typeof(double);
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
            dr = dt.NewRow();
            dr["binx"] = "1111001111";
            dr["biny"] = "1110110111";
            dt.Rows.Add(dr);

            #endregion

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            chart1.Series["Media"].XValueMember = "generation";
            chart1.Series["Media"].YValueMembers = "fitness";
            chart1.Series["Media"].ChartType = SeriesChartType.Line;

            chart1.Series["Max"].XValueMember = "generation";
            chart1.Series["Max"].YValueMembers = "max";
            chart1.Series["Max"].ChartType = SeriesChartType.Line;
            chart1.Series["Max"].BorderDashStyle = ChartDashStyle.DashDot;
            DefinePares();

            int Geracoes = 1;
            for (int i = 0; i < Geracoes; i++)
            {
                DataRow row = chartdt.NewRow();
                row["generation"] = i;
                dtantigo = dt.Copy();

                

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dr = dt.Rows[j];
                    string xString = (string)dr["binx"];
                    string yString = (string)dr["biny"];
                    bintodec(xString, yString);
                    Rodar_e_fitness();

                }

                
                SelecaoTorneio(); // Seleciona da table e coloca nos selecionados
                // SinglePointCrossOver(); // Faz crossover dos selecionados
                UniformCrossover();
                Mutacao(); // Muta os selecionados
                MediaFitness();
                row["fitness"] = mediafitness;
                Melhor();
                row["max"] = melhor;
                chartdt.Rows.Add(row);
                ListtoDataTable();

                if (dtantigo.Rows.Count != 0)
                    Pintadiferencas();

                dataGridView2.DataSource = dt;
                chart1.DataSource = chartdt;
                dataGridView1.DataSource = chartdt;
            }
        }

        private void Pintadiferencas()
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if(cell.Value != null)
                    {
                    cell.Style.BackColor = System.Drawing.Color.White;
                    if (dtantigo.Rows[cell.RowIndex][cell.ColumnIndex] != dt.Rows[cell.RowIndex][cell.ColumnIndex])
                    {
                        cell.Style.BackColor = System.Drawing.Color.Yellow;
                    }
                    }
                }
            }
        }

        private void DefinePares()
        {
            if (Math.Floor(dt.Rows.Count * 0.4) % 2 == 0)
            {
                pares = (int)(Math.Floor(dt.Rows.Count * 0.4));
            }
            else
            {
                pares = (int)(Math.Ceiling(dt.Rows.Count * 0.4));
            }

        }

        private void TruncaFitness()
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["fitness"] = Math.Truncate((double)dt.Rows[i]["fitness"] * 100000000) /100000000;
                
            }
        }

        private void ListtoDataTable()
        {
            for (int i = 0; i < 4; i++)
            {
                var x = selecionadaosx[i]["fitness"].ToString();
                dr = dt.Select("fitness = " + selecionadaosx[i]["fitness"].ToString().Replace(',', '.')).FirstOrDefault();
                dr["binx"] = selecionadaosx[i]["binx"];
                dr["biny"] = selecionadaosx[i]["biny"];
            }
                
        }
        

        private void Melhor()
        {
            melhor = dt.AsEnumerable().Select(r => r.Field<double>("fitness")).Max();
        }

        private void MediaFitness()
        {
            double soma = dt.AsEnumerable().Select(r => r.Field<double>("fitness")).Sum();
            mediafitness = soma / dt.Rows.Count;
        }

        private void Mutacao()
        {

            for (int i = 0; i < selecionadaosx.Count(); i++)
            {
                Random rand = new Random();
                if (rand.Next(0,1000) < probabilidadeMutacao )
                {
                    countMutacao++;
                    var cromossomoarray = selecionadaosx[i]["binx"].ToString().ToCharArray();
                    int sorteado = rand.Next(0, 4);

                    if (cromossomoarray[sorteado] == '1')
                    {
                        cromossomoarray[sorteado] = '0';
                    }
                    else
                    {
                        cromossomoarray[sorteado] = '1';
                    }

                    selecionadaosx[i]["binx"] = new string(cromossomoarray);
                    }
            }
            for (int i = 0; i < selecionadaosy.Count(); i++)
            {
                Random rand = new Random();
                if (rand.Next(0, 1000) < probabilidadeMutacao)
                {
                    var cromossomoarray = selecionadaosy[i]["biny"].ToString().ToCharArray();
                    int sorteado = rand.Next(0, 4);

                    if (cromossomoarray[sorteado] == '1')
                    {
                        cromossomoarray[sorteado] = '0';
                    }
                    else
                    {
                        cromossomoarray[sorteado] = '1';
                    }

                    selecionadaosy[i]["biny"] = new string(cromossomoarray);
                }
            }
        }

        private void SinglePointCrossOver()
        {

            selecionadaosx = dt.Select("selecionadox = 1");
            selecionadaosy = dt.Select("selecionadoy = 1");

            int chanceCruzamento = 50;
            var rand = new Random();

            for (int i = 0; i < 2; i++)
            {
                int cruza = rand.Next(1, 100);

                if (cruza < chanceCruzamento)
                {
                    int pontoDivisao = rand.Next(1, 10);
                    if (i == 1) i++;
                    //Pega a segunda parte 
                    string parte2_x = selecionadaosx[i]["binx"].ToString().Substring(pontoDivisao, 10 - pontoDivisao);
                    string parte2_x1 = selecionadaosx[i+1]["binx"].ToString().Substring(pontoDivisao, 10 - pontoDivisao);

                    string parte2_y = selecionadaosy[i]["biny"].ToString().Substring(pontoDivisao, 10 - pontoDivisao);
                    string parte2_y1 = selecionadaosy[i+1]["biny"].ToString().Substring(pontoDivisao, 10 - pontoDivisao);

                    // coloca nos lugares
                    selecionadaosx[i]["binx"] = selecionadaosx[i]["binx"].ToString().Substring(0, pontoDivisao) + parte2_x1;
                    selecionadaosx[i+1]["binx"] = selecionadaosx[i+1]["binx"].ToString().Substring(0, pontoDivisao) + parte2_x;

                    selecionadaosy[i]["biny"] = selecionadaosy[i]["biny"].ToString().Substring(0, pontoDivisao) + parte2_y1;
                    selecionadaosy[i + 1]["biny"] = selecionadaosy[i + 1]["biny"].ToString().Substring(0, pontoDivisao) + parte2_y;
                }

            }
        }

        private void UniformCrossover()
        {
            selecionadaosx = dt.Select("selecionadox = 1");
            selecionadaosy = dt.Select("selecionadoy = 1");

            int chanceCruzamento = 50;
            var rand = new Random();

            for (int i = 0; i < pares / 2; i = i+2)
            {

                for (int i2 = 1; i2 < pares / 2; i2 = i2+2)
                {


                    for (int j = 0; j < 5; j++)
                    {
                        if (rand.Next(0, 100) < chanceCruzamento)
                        {
                            var arr = selecionadaosx[i]["binx"].ToString().ToCharArray();
                            arr[j] = selecionadaosx[i + 1]["binx"].ToString().ToCharArray()[j];

                            var arr2 = selecionadaosx[i + 1]["binx"].ToString().ToCharArray();
                            arr2[j] = selecionadaosx[i]["binx"].ToString().ToCharArray()[j];

                            selecionadaosx[i]["binx"] = new string(arr);
                            selecionadaosx[i + 1]["binx"] = new string(arr2);

                            arr = selecionadaosy[i]["biny"].ToString().ToCharArray();
                            arr[j] = selecionadaosy[i + 1]["biny"].ToString().ToCharArray()[j];

                            arr2 = selecionadaosy[i + 1]["biny"].ToString().ToCharArray();
                            arr2[j] = selecionadaosy[i]["biny"].ToString().ToCharArray()[j];

                            selecionadaosy[i]["biny"] = new string(arr);
                            selecionadaosy[i + 1]["biny"] = new string(arr2);

                        }
                    }
                }

            }
        }

        private void SelecaoTorneio()
        {
            selecionadaosx = null;
            selecionadaosy = null;
            LimpaSelecionados();

            
            
            // Seleciona 40% da populacao
            while (selecionadaosx == null || selecionadaosx.Count() < pares)
            {
                selecionadaosx = dt.Select("selecionadox = 1");
                selecionadaosy = dt.Select("selecionadoy = 1");

                var rand = new Random();
                var sorteadosx = new List<double>();
                var sorteadosy = new List<double>();
                for (int i = 0; i < 3; i++)
                {
                    int sorteadox = rand.Next(0, dt.Rows.Count);
                    int sorteadoy = rand.Next(0, dt.Rows.Count);
                    sorteadosx.Add((double)dt.Rows[sorteadox]["fitness"]);
                    sorteadosy.Add((double)dt.Rows[sorteadoy]["fitness"]);
                }
                
                //Permite repetidos
                    var x = string.Concat("fitness = ", sorteadosx.Max().ToString().Replace(',','.'));
                    dt.Select(x).FirstOrDefault()["selecionadox"] = 1; ;
                    dt.Select(x).FirstOrDefault()["selecionadoy"] = 1; ;

                    
                
            } 
        }

        private void LimpaSelecionados()
        {
            foreach (var x in dt.Select("selecionadoy = 1 OR selecionadox = 1"))
            {
                x["selecionadox"] = null;
                x["selecionadoy"] = null;
            }
        }

        private bool ContainsDataRow(bool x, double max)
        {
            if (x)
            {
                foreach (DataRow row in selecionadaosx)if ((double)row["fitness"] == max) return true;    
            }
            else
            {
                foreach (DataRow row in selecionadaosy) if ((double)row["fitness"] == max) return true;
            }
            return false;
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
                dr["fitness"] = Math.Truncate(fitness * 100000000) / 100000000;

        }
    }
}
