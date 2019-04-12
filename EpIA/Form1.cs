using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EpIA
{
	public partial class Form1 : Form
	{

        DataTable dt = new DataTable();
        DataRow dr;
        double x;
		double y;

        double fitness;




        public Form1()
		{
			InitializeComponent();
            DataColumn binx = new DataColumn("binx");
            DataColumn biny = new DataColumn("biny");
            DataColumn decx = new DataColumn("decx");
            decx.DataType = typeof(double);
            DataColumn decy = new DataColumn("decy");
            decy.DataType = typeof(double);
            DataColumn result = new DataColumn("result");
            DataColumn fitness = new DataColumn("fitness");
            DataColumn Chance = new DataColumn("Chance");
            dt.Columns.Add(binx);
            dt.Columns.Add(biny);
            dt.Columns.Add(decx);
            dt.Columns.Add(decy);
            dt.Columns.Add(result);
            dt.Columns.Add(fitness);

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
                dataGridView1.DataSource = dt;
                'SelecaoTorneio();

                }
        }

        private void SelecaoTorneio()
        {
            var selecionadaosx = new List<String>();
            var selecionadaosy = new List<String>();
            do
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

                selecionadaosx.Add(sorteadosx.Max());
                selecionadaosy.Add(sorteadosy.Max());

            } while (selecionadaosx.Count < 5 && selecionadaosy.Count < 5 );

            foreach (string selecionado in selecionadaosx)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString() == selecionado)
                    {
                        row.Cells[0].Style.BackColor = Color.Blue;
                    }
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

            dr["decx"] = x;
            dr["decy"] = y;



        }

        private void Rodar_e_fitness()
        {
            
                var result = 20 + Math.Pow(x, 2) + Math.Pow(y, 2) - 10 * (Math.Cos(2 * Math.PI * x) + Math.Cos(2 * Math.PI * y));
                fitness = 1 / result;
                dr["fitness"] = fitness;
                dr["result"] = result;
                
        }
    }
}
