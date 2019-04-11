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
		int[,] popini  = new int[10,20]{
				{0,0,0,0,0,1,0,1,1,0,0,0,1,1,1,0,1,1,1,0},
				{1,0,0,0,0,0,1,1,0,0,1,0,1,0,1,1,1,1,1,0},
				{0,1,0,1,0,0,0,0,0,1,0,1,0,1,1,0,1,1,1,1},
				{0,1,1,1,1,1,0,0,0,1,0,1,1,1,0,0,0,1,0,0},
				{1,0,1,1,1,1,0,0,0,0,0,1,1,1,0,1,0,1,0,1},
				{0,1,0,0,0,1,0,0,1,0,1,0,1,0,0,1,0,1,1,1},
				{1,1,0,1,1,0,1,0,1,1,0,0,0,0,0,1,0,1,1,1},
				{0,1,0,1,1,0,0,0,0,0,1,1,0,1,1,0,0,0,0,0},
				{1,0,0,1,1,1,0,0,1,0,0,1,1,0,1,1,1,0,1,1},
				{0,0,1,1,0,0,1,1,1,0,1,0,1,0,1,1,0,0,1,1}
		 };
		double x;
		double y;

        double fitness;

        string lineString;

        DataRow dr;


        public Form1()
		{
			InitializeComponent();
            DataColumn binx = new DataColumn("binx");
            DataColumn biny = new DataColumn("biny");
            DataColumn decx = new DataColumn("decx");
            decx.DataType = typeof(double);
            DataColumn decy = new DataColumn("decy");
            decy.DataType = typeof(double);
            DataColumn resultx = new DataColumn("resultx");
            DataColumn resulty = new DataColumn("resulty");
            DataColumn fitnessx = new DataColumn("fitnessx");
            DataColumn fitnessy = new DataColumn("fitnessy");
            DataColumn sum = new DataColumn("sum");
            dt.Columns.Add(binx);
            dt.Columns.Add(biny);
            dt.Columns.Add(decx);
            dt.Columns.Add(decy);
            dt.Columns.Add(resultx);
            dt.Columns.Add(resulty);
            dt.Columns.Add(fitnessx);
            dt.Columns.Add(fitnessy);
            dt.Columns.Add(sum);


        }

        private void Button1_Click(object sender, EventArgs e)
		{
            
            Rodar();
           Selecao();

        }

        private void Selecao()
        {
            throw new NotImplementedException();
        }

        private void bintodec(string line)
		{
			
			
			
            
			string xString = line.Substring(0,10);
			string yString = line.Substring(10, 10);
            dr["binx"] = xString;
            dr["biny"] = yString;
            x = Convert.ToInt32(xString,2);
			y = Convert.ToInt32(yString, 2);

			x = (x * 0.00978) -5;
			y = (y * 0.00978) - 5;

			x = Math.Round(x, 2);
			y = Math.Round(y, 2);

            dr["decx"] = x;
            dr["decy"] = y;



        }

        private void Rodar()
        {
            for (int i = 0; i < 10; i++)
            {
                
                var line = popini.Cast<int>().Skip(i * 20).Take(20).ToArray(); // pega cada linha da matriz
                string lineString = string.Join("",line); // array to string

                dr = dt.NewRow();
                
             
                bintodec(lineString);
                
                
                Console.WriteLine("Fitnes {0}: {1}", i + 1, fitness);

            var result1 = 20 + Math.Pow(x, 2) + Math.Pow(y, 2) - 10 * (Math.Cos(2 * Math.PI * x) + Math.Cos(2 * Math.PI * y));
            fitness = 1 / result1;
            }
        }
    }
}
