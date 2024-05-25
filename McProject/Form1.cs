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
using System.Diagnostics.Eventing.Reader;

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
            button6.Enabled = false;
            button7.Enabled = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("column1", "Название блюда с нулевой калорийностью");
            dataGridView1.Columns["column1"].Width = 300;
            foreach (string item in f.ZeroCaloriesList) { 
                dataGridView1.Rows.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Чтобы получить суточную норму железа нужно купить {f.CountMcChicken} макчикенов, или {f.McChickenGramms} грамм!", "McChickens", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("column1", "Категория");
            dataGridView1.Columns.Add("column2", "Блюдо");
            dataGridView1.Columns.Add("column3", "Калории");
            dataGridView1.Columns.Add("column4", "Сахар");
            dataGridView1.Columns.Add("column5", "Размер порции");
            dataGridView1.Columns["column1"].Width = 100;
            dataGridView1.Columns["column2"].Width = 300;
            dataGridView1.Columns["column3"].Width = 80;
            dataGridView1.Columns["column4"].Width = 50;
            dataGridView1.Columns["column5"].Width = 100;
            foreach (string[] item in f.SugarsCalories) {
                dataGridView1.Rows.Add(item[0], item[1], item[2], item[3], item[4]);
            }
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
                    f.DieticalFoodForEachCategory();
                    button8.Text = "Выбранный файл: " + filePath;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true; 
                    button7.Enabled = true;
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

        private void button6_Click(object sender, EventArgs e)
        {
            // график столбчатый по калорийности (диетическая еда в каждой категории)
           
            

            Form2 form = new Form2(f.CaloriesPercentsCategoryes, f.Categorues);
            form.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // кружок (высокобелковая пища по всем категориям)
            Form3 form = new Form3();
            form.ShowDialog();
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

        public List<double> CaloriesPercentsCategoryes;
        public List<string> Categorues;

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

        public void DieticalFoodForEachCategory() {

            List<double> percents = new List<double>();
            List<string> categories = new List<string>(); 
            string temp_category = data[0][(int)fields.Category];
            double allproductsincategory = 0;
            double dieticalproductsincategory = 0;


            for (int i = 0; i < data.Length; i++)
            {
                string category = data[i][(int)fields.Category];

                if (category != temp_category) {
                    
                    categories.Add(temp_category);
                    temp_category = category;
                    double gg = (dieticalproductsincategory / allproductsincategory) * 100;
                    percents.Add(Math.Round(gg));
                    
                    allproductsincategory = 0;
                    dieticalproductsincategory = 0;
                } 

                allproductsincategory++;


                double p = (double.Parse(data[i][(int)fields.CaloriesFromFat]) + 1) / (double.Parse(data[i][(int)fields.Calories])+1);
                if (p<0.4) {
                    dieticalproductsincategory++;
                }

                if (i == data.Length-1) { 
                    percents.Add((dieticalproductsincategory / allproductsincategory) * 100);
                    categories.Add(category);
                }
            }

            CaloriesPercentsCategoryes = percents;
            Categorues = categories;
        }
    }

    
}
