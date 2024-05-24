using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace McProject
{
    public partial class Form1 : Form
    {
        enum fields : int
        { 
            Category,
            Item,
            ServingSize,
            Calories,
            CaloriesFromFat,
            TotalFat,
            TotalFatDaily,
            SaturatedFat,
            SaturatedFatDaily,
            TransFat,
            Cholesterol,
            CholesterolDaily,
            Sodium,
            SodiumDaily,
            Carbohydrates,
            CarbohydratesDaily,
            DietaryFiber,
            DietaryFiberDaily,
            Sugars,
            Protein,
            VitaminADaily,
            VitaminCDaily,
            CalciumDaily,
            IronDaily
        }

        public string[][] data;

        public Form1()
        {
           
            InitializeComponent();
            string[] f = File.ReadAllLines("E:\\data_mc.csv");
            string[][] f_splitted = new string[f.Length-1][];
            string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
            for (int i = 1; i < f.Length; i++) {

                f_splitted[i-1] = Regex.Split(f[i], pattern);
            }
            data = f_splitted;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> zeros = new List<string>();

            foreach (string[] d in data) {

                if (int.Parse(d[(int)fields.Calories]) == 0) {
                    zeros.Add(d[(int)fields.Item]);
                }
            }


            //TODO: надо сделать красивый вывод на экран
            MessageBox.Show(zeros[0]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string pattern = @"\((\d+)\s*[^\)]+\)";
            for (int i = 0; i<data.Length; i++) {
                if (data[i][(int)fields.Item] == "McChicken") {
                    double cnt = Math.Ceiling(100.0 / int.Parse(data[i][(int)fields.IronDaily]));


                    // TODO: покрасивше вывод сделать
                    MessageBox.Show(cnt.ToString());
                    Match match = Regex.Match(data[i][(int)fields.ServingSize], pattern);
                    var need = match.Groups[1].Value;
                    MessageBox.Show((int.Parse(need)*cnt).ToString());
                    break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
