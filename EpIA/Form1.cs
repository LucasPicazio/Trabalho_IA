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
		int[,] popini  = new int[10,20]{
				{0,0,0,0,0,1,0,1,1,0,0,0,1,1,1,0,1,1,1,0},				{1,0,0,0,0,0,1,1,0,0,1,0,1,0,1,1,1,1,1,0},				{0,1,0,1,0,0,0,0,0,1,0,1,0,1,1,0,1,1,1,1},				{0,1,1,1,1,1,0,0,0,1,0,1,1,1,0,0,0,1,0,0},				{1,0,1,1,1,1,0,0,0,0,0,1,1,1,0,1,0,1,0,1},				{0,1,0,0,0,1,0,0,1,0,1,0,1,0,0,1,0,1,1,1},				{1,1,0,1,1,0,1,0,1,1,0,0,0,0,0,1,0,1,1,1},				{0,1,0,1,1,0,0,0,0,0,1,1,0,1,1,0,0,0,0,0},				{1,0,0,1,1,1,0,0,1,0,0,1,1,0,1,1,1,0,1,1},				{0,0,1,1,0,0,1,1,1,0,1,0,1,0,1,1,0,0,1,1}
		 };		double x;		double y;
		public Form1()
		{
			InitializeComponent();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			bintodec();
		}

		private void bintodec()
		{
			
			var line = popini.Cast<int>().Take(20).ToArray();
			string lineString = string.Join("",line);
			string xString = lineString.Substring(0,10);
			string yString = lineString.Substring(10, 10);
			x = Convert.ToInt32(xString,2);
			y = Convert.ToInt32(yString, 2);

			x = (x * 0.00978) -5;
			y = (y * 0.00978) - 5;

			x = Math.Round(x, 2);
			y = Math.Round(y, 2);

		}
	}
}
