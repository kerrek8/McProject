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
    public partial class Form2 : Form
    {
        public Form2(List<double> list, List<string> categories)
        {
            InitializeComponent();
            InitializeChart(list, categories);
            
        }
        private void InitializeChart(List<double> list, List<string> categories) {
            chart1.Dock = DockStyle.Fill;
            chart1.Series.Clear();
            chart1.Titles.Add("Процент диетических продуктов в каждой категории:");

            Series series = chart1.Series.Add("Проценты");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;

            for (int i = 0; i < list.Count; i++)
            {
                series.Points.AddXY(categories[i], list[i]);

            }
            // Настройка осей
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.Title = "Категории";
            chart1.ChartAreas[0].AxisY.Title = "Проценты";
            ChartArea chartArea = chart1.ChartAreas[0];
            chartArea.AxisY.Maximum = 100;
            // Скрыть сетку
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MinorGrid.Enabled = false;

            // Настройка цвета столбцов
            series.Points[0].Color = System.Drawing.Color.Green;
            series.Points[1].Color = System.Drawing.Color.Blue;
            series.Points[2].Color = System.Drawing.Color.DarkGreen;
            series.Points[3].Color = System.Drawing.Color.Purple;
            series.Points[4].Color = System.Drawing.Color.Pink;
            series.Points[5].Color = System.Drawing.Color.BlueViolet;
            series.Points[6].Color = System.Drawing.Color.LightGreen;
            series.Points[7].Color = System.Drawing.Color.DarkTurquoise;
            series.Points[8].Color = System.Drawing.Color.DeepSkyBlue;
            chart1.Legends[0].Enabled = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
