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
using System.Globalization;
using System.Net.NetworkInformation;

namespace McProject
{
    public partial class Form1 : Form
    {
        public Funcs f = new Funcs();
        public Form1()
        {

            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;    
            button4.Enabled = false;
            button5.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show(f.ZeroCaloriesList[0]);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            MessageBox.Show(f.CountMcChicken);
            MessageBox.Show(f.McChickenGramms);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            MessageBox.Show(f.SugarsCalories[0][1]);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            MessageBox.Show(f.TransItems[0][1]);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            MessageBox.Show(f.relation);

        }





        private void button8_Click(object sender, EventArgs e)
        {
            //file piacker
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Настраиваем свойства диалога (опционально)
            openFileDialog.Title = "Выберите файл";
            openFileDialog.Filter = "Файл с данными (*.csv)|*.csv";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            // Показываем диалог и проверяем, что пользователь выбрал файл
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к выбранному файлу
                string filePath = openFileDialog.FileName;

                try {
                    f.GetData(filePath);
                    f.GetZeroCalories();
                    f.GetMcChickenValues();
                    f.GetSugarsCalories();
                    f.GetTransFatsItems();
                    f.GetMidleTransInBeefPork();
                    button8.Text = "Выбранный файл: " + filePath;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("Неподходящий файл, проверьте данные в файле!\n" + "Выбранный файл: " + filePath, "Ошибка", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    {
                        button8_Click(sender, e);
                    }
                    
                    
                }

                
                
            }
        }
    }
    public class Funcs
    {

        public string[][] data;
        public List<string> ZeroCaloriesList;
        public string CountMcChicken;
        public string McChickenGramms;

        public List<string[]> SugarsCalories;
        public List<string[]> TransItems;

        public string relation;
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



        public void GetData(string path) {
            string[] f = File.ReadAllLines(path);
            string[][] f_splitted = new string[f.Length - 1][];
            string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
            for (int i = 1; i < f.Length; i++)
            {

                f_splitted[i - 1] = Regex.Split(f[i], pattern);
            }
            data = f_splitted;
        }

        

        public void GetZeroCalories() {
            List<string> zeros = new List<string>();
            
            foreach (string[] d in data)
            {

                if (int.Parse(d[(int)fields.Calories]) == 0)
                {
                    zeros.Add(d[(int)fields.Item]);
                }
            }
            ZeroCaloriesList = zeros;
        }

        public void GetMcChickenValues() {
            string pattern = @"\((\d+)\s*[^\)]+\)";
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i][(int)fields.Item] == "McChicken")
                {
                    double cnt = Math.Ceiling(100.0 / int.Parse(data[i][(int)fields.IronDaily]));

                    CountMcChicken = cnt.ToString();
                    Match match = Regex.Match(data[i][(int)fields.ServingSize], pattern);
                    int need = int.Parse(match.Groups[1].Value);
                    McChickenGramms = (need * cnt).ToString();
                    break;
                }
            }
        }

        public void GetSugarsCalories() {
            List<string[]> sugars_calories = new List<string[]>();
            foreach (string[] d in data)
            {
                int sugars = int.Parse(d[(int)fields.Sugars]);
                int calories = int.Parse(d[(int)fields.Calories]);
                if (sugars >= 1 && sugars <= 10)
                {
                    if (calories >= 15 && calories <= 220)
                    {
                        string category = d[(int)fields.Category];
                        string item = d[(int)fields.Item];
                        string cal = d[(int)fields.Calories];
                        string sug = d[(int)fields.Sugars];
                        string size = d[(int)fields.ServingSize];
                        string[] add_fields = new string[] { category, item, cal, sug, size };
                        sugars_calories.Add(add_fields);
                    }
                }
            }
            SugarsCalories = sugars_calories;
        }

        public void GetTransFatsItems() {
            List<string[]> trans_items = new List<string[]>();
            foreach (string[] d in data)
            {
                double trans = double.Parse(d[(int)fields.TransFat], CultureInfo.InvariantCulture);
                if (trans > 0)
                {
                    string category = d[(int)fields.Category];
                    string item = d[(int)fields.Item];
                    string tr = d[(int)fields.TransFat];
                    string size = d[(int)fields.ServingSize];
                    string[] add_fields = new string[] { category, item, tr, size };
                    trans_items.Add(add_fields);
                }
            }
            TransItems = trans_items;
        }

        public void GetMidleTransInBeefPork() {
            double sum10 = 0;
            double sum3 = 0;
            string pattern = @"\((\d+)\s*[^\)]+\)";
            foreach (string[] d in data) {
                if (d[(int)fields.Category] == "Beef & Pork") 
                {
                    sum10 += double.Parse(d[(int)fields.TransFat], CultureInfo.InvariantCulture);
                    Match match = Regex.Match(d[(int)fields.ServingSize], pattern);
                    double s3 = double.Parse(match.Groups[1].Value);
                    sum3 += s3;
                }
            }
            double rel = sum10 / sum3;
            rel = Math.Round(rel, 4);
            relation = rel.ToString(CultureInfo.InvariantCulture);
        }


    }

    
}
