using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace McProject
{
    public partial class Form3 : Form
    {
        public Form3(Dictionary<string, int> proteinproducts)
        {
            InitializeComponent();
            InitializeChart(proteinproducts);
            
        }
        private void InitializeChart(Dictionary<string, int> dict) {
            chart1.Dock = DockStyle.Fill;
            chart1.Series.Clear();
            chart1.Titles.Add("Распределение продуктов по содержанию белка: ");

            Series series = chart1.Series.Add("Высокобелковые продукты");
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;
            var lowprotein = dict["allproducts"] - dict["highprotein"] - dict["midprotein"];
            series.Points.AddXY("Низкобелковые продукты", lowprotein);
            series.Points.AddXY("Высокобелковые продукты", dict["highprotein"]);
            series.Points.AddXY("Среднебелковые продукты", dict["midprotein"]);
            
        }
    }
}
