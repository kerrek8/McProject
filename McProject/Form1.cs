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
            button9.Enabled = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label7.Enabled = false;
            label8.Enabled = false;
            label9.Enabled = false;
            label10.Enabled = false;
            label11.Enabled = false;
            label12.Enabled = false;
            label13.Enabled = false;
            numericUpDown1.Enabled = false;
            listBox1.Enabled = false;
            listBox2.Enabled = false;
            listBox3.Enabled = false;
            listBox4.Enabled = false;
            listBox5.Enabled = false;
            listBox6.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled  = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            int cal = (int)numericUpDown1.Value;
            dataGridView1.Columns.Add("column1", $"Название блюда с калорийностью = {cal}");
            dataGridView1.Columns["column1"].Width = 300;
            f.GetZeroCalories(cal);
            if (f.ZeroCaloriesList.Count == 0) {
                MessageBox.Show($"Блюд, с калорийностью = {cal}, не найдено", "McProject", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (string item in f.ZeroCaloriesList) { 
                dataGridView1.Rows.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // TODO: переделать к чертам

            string selectedItem1 = listBox2.SelectedItem as string; 
            string selectedItem2 = listBox3.SelectedItem as string;
 
            f.GetMcChickenValues(selectedItem1, selectedItem2);
            MessageBox.Show($"Чтобы получить суточную норму {selectedItem2} нужно купить {f.CountMcChicken} {selectedItem1}, или {f.McChickenGramms} грамм!", "McProject", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            string selectedItem1 = listBox4.SelectedItem as string;
            double before1 = double.Parse(textBox1.Text);
            double after1 = double.Parse(textBox2.Text);
            double before2 = double.Parse(textBox3.Text);
            double after2 = double.Parse(textBox4.Text);
            f.GetSugarsCalories(selectedItem1, before1, after1, before2, after2);
            dataGridView1.Columns.Add("column1", "Категория");
            dataGridView1.Columns.Add("column2", "Блюдо");
            dataGridView1.Columns.Add("column3", "Калории");
            //TODO: русик
            dataGridView1.Columns.Add("column4", $"{selectedItem1}");
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
            string selectedItem = listBox5.SelectedItem as string;
            f.GetTransFatsItems(selectedItem);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("column1", "Категория");
            dataGridView1.Columns.Add("column2", "Блюдо");
            // TODO: русифицировать
            dataGridView1.Columns.Add("column3", selectedItem);
            dataGridView1.Columns.Add("column4", "Размер порции");
            dataGridView1.Columns["column1"].Width = 120;
            dataGridView1.Columns["column2"].Width = 300;
            dataGridView1.Columns["column3"].Width = 80;
            dataGridView1.Columns["column4"].Width = 100;
            
            foreach (string[] item in f.TransItems)
            {
                dataGridView1.Rows.Add(item[0], item[1], item[2], item[3]);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string selectedItem = listBox6.SelectedItem as string;
            f.GetMidleTransInBeefPork(selectedItem);
            // русик
            MessageBox.Show($"Средняя доля транс-жиров в категории {selectedItem} составляет {f.relation}%", "McProject", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    f.DieticalFoodForEachCategory();
                    f.HighProteinProductsCount();
                    foreach (string u in f.uniccategories) {
                        listBox1.Items.Add(u);
                        listBox6.Items.Add(u);
                    }
                    foreach (string nd in f.notdailycategories) {
                        listBox5.Items.Add(nd);
                        listBox4.Items.Add(nd);
                    }
                    

                    button8.Text = "Выбранный файл: " + filePath;
                    button1.Enabled = true;
                    button6.Enabled = true; 
                    button7.Enabled = true;
                    button9.Enabled = true;
                    label1.Enabled = true;
                    label2.Enabled = true;
                    label3.Enabled = true;
                    label4.Enabled = true;
                    label5.Enabled = true;
                    label6.Enabled = true;
                    label7.Enabled = true;
                    label8.Enabled = true;
                    label9.Enabled = true;
                    label10.Enabled = true;
                    label11.Enabled = true;
                    label12.Enabled = true;
                    label13.Enabled = true;
                    numericUpDown1.Enabled = true;
                    listBox1.Enabled = true;
                    listBox4.Enabled = true;
                    listBox5.Enabled = true;
                    listBox6.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
                    
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
            Form3 form = new Form3(f.Highproteinproducts);
            form.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            foreach (string c in f.cat) {
                dataGridView1.Columns.Add(c,c);
            }
            for (int i = 0; i<f.data.Length; i++) {
                dataGridView1.Rows.Add(f.data[i]);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = listBox1.SelectedItem as string;
            if (selectedItem != null)
            {
                listBox2.Enabled = true;
                listBox3.Enabled = true;
                listBox2.Items.Clear();
                foreach (string[] d in f.data) {
                    if (d[0] == selectedItem) {
                        listBox2.Items.Add(d[1]);
                    }
                }

                foreach (string d in f.dailycategories)
                {
                    listBox3.Items.Add(d);
                }
            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = listBox5.SelectedItem as string;
            //TODO: русифицировать
            button4.Text = $"Получить список блюд с содержанием(>0): {selectedItem}";
            button4.Enabled = true;
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = listBox6.SelectedItem as string;
            //TODO: русифицировать
            button5.Text = $"Получить среднюю долю транс-жиров в категории: {selectedItem}";
            button5.Enabled = true;
        }



        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem2 = listBox2.SelectedItem as string;
            string selectedItem3 = listBox3.SelectedItem as string;
            if (selectedItem2!=null && selectedItem3!=null) {
                // TODO: возможно вторую подстоновку русифицировать
                button2.Text = $"Сколько порций {selectedItem2}\nнужно купить чтобы получить суточную норму {selectedItem3}";
                button2.Enabled = true;
                
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string selectedItem = listBox4.SelectedItem as string;
            try
            {
                if (textBox1.Text == "" || textBox4.Text == "" || textBox3.Text == "" || textBox2.Text == "")
                {
                    return;
                }
                double before1 = double.Parse(textBox1.Text);
                double after1 = double.Parse(textBox2.Text);
                double before2 = double.Parse(textBox3.Text);
                double after2 = double.Parse(textBox4.Text);
                if (selectedItem != null)
                {
                    //русифицировать
                    button3.Text = $"Получить список блюд с содержанием {selectedItem} от {before1} до {after1} и калорийностью от {before2} до {after2}";
                    button3.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Неправильно задан диапозон, провертезначения", "McProject", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button3.Enabled = false;
            }
        }
    }
    public class Funcs
    {
        public List<string> dailycategories;
        public List<string> notdailycategories;

        public List<string> uniccategories;
        public string[] cat;
        public string[][] data;
        public List<string> ZeroCaloriesList;
        public string CountMcChicken;
        public string McChickenGramms;

        public List<string[]> SugarsCalories;
        public List<string[]> TransItems;

        public string relation;

        public List<double> CaloriesPercentsCategoryes;
        public List<string> Categorues;

        public Dictionary<string, int> Highproteinproducts;

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
            string[] c = new string[0];
            c = f[0].Split(',');
            List<string> daily = new List<string>();
            List<string> notdaily = new List<string>();
            for(int i = 3; i<c.Length; i++) {
                if (c[i].Contains("(% Daily Value)"))
                {
                    daily.Add(c[i].Substring(0, c[i].IndexOf('(')));
                }
                else {
                    notdaily.Add(c[i]);
                }
            }
            notdailycategories = notdaily;
            dailycategories = daily;
            cat = c;


            List<string> unic = new List<string>();
            for (int i = 1; i < f.Length; i++)
            {
                
                f_splitted[i - 1] = Regex.Split(f[i], pattern);
                if (!unic.Contains(f_splitted[i-1][(int)fields.Category])) {
                    unic.Add(f_splitted[i - 1][(int)fields.Category]);
                }
            }
            uniccategories = unic;
            data = f_splitted;
        }

        

        public void GetZeroCalories(int c) {
            List<string> zeros = new List<string>();
            
            foreach (string[] d in data)
            {

                if (int.Parse(d[(int)fields.Calories]) == c)
                {
                    zeros.Add(d[(int)fields.Item]);
                }
            }
            ZeroCaloriesList = zeros;
        }

        public void GetMcChickenValues(string item, string far) {
            // TODO: переделать
            string pattern = @"\((\d+)\s*[^\)]+\)";
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i][(int)fields.Item] == item)
                {
                    int n = Array.IndexOf(cat, $"{far}(% Daily Value)");
                    double cnt = Math.Ceiling(100.0 / int.Parse(data[i][n]));

                    CountMcChicken = cnt.ToString();
                    Match match = Regex.Match(data[i][(int)fields.ServingSize], pattern);
                    int need = int.Parse(match.Groups[1].Value);
                    McChickenGramms = (need * cnt).ToString();
                    break;
                }
            }
        }

        public void GetSugarsCalories(string si, double b1, double a1, double b2, double a2) {
            List<string[]> sugars_calories = new List<string[]>();
            int n = Array.IndexOf(cat, si);
            foreach (string[] d in data)
            {
                double sugars = double.Parse(d[n], CultureInfo.InvariantCulture);
                int calories = int.Parse(d[(int)fields.Calories]);
                if (sugars >= b1 && sugars <= a1)
                {
                    if (calories >= b2 && calories <= a2)
                    {
                        string category = d[(int)fields.Category];
                        string item = d[(int)fields.Item];
                        string cal = d[(int)fields.Calories];
                        string sug = d[n];
                        string size = d[(int)fields.ServingSize];
                        string[] add_fields = new string[] { category, item, cal, sug, size };
                        sugars_calories.Add(add_fields);
                    }
                }
            }
            SugarsCalories = sugars_calories;
        }

        public void GetTransFatsItems(string si) {
            List<string[]> trans_items = new List<string[]>();
            int n = Array.IndexOf(cat, si);
            foreach (string[] d in data)
            {
                double trans = double.Parse(d[n], CultureInfo.InvariantCulture);
                if (trans > 0)
                {
                    string category = d[(int)fields.Category];
                    string item = d[(int)fields.Item];
                    string tr = d[n];
                    string size = d[(int)fields.ServingSize];
                    string[] add_fields = new string[] { category, item, tr, size };
                    trans_items.Add(add_fields);
                }
            }
            TransItems = trans_items;
        }

        public void GetMidleTransInBeefPork(string c) {
            double sum10 = 0;
            double sum3 = 0;
            string pattern = @"\((\d+)\s*[^\)]+\)";
            foreach (string[] d in data) {
                if (d[(int)fields.Category] == c) 
                {
                    sum10 += double.Parse(d[(int)fields.TransFat], CultureInfo.InvariantCulture);
                    // TODO: обработка товаров где нет грамм
                    Match match = Regex.Match(d[(int)fields.ServingSize], pattern);
                    double s3 = double.Parse(match.Groups[1].Value);
                    sum3 += s3;
                }
            }
            double rel = sum10 / sum3;
            rel = Math.Round(rel, 4);
            rel = rel * 100;
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

        public void HighProteinProductsCount()
        {
            Dictionary<string, int> highproteinproducts = new Dictionary<string, int>() {
                {"allproducts", 0},
                {"highprotein", 0},
                {"midprotein", 0}
            };
            
            foreach (string[] d in data) {
                highproteinproducts["allproducts"]++;


                if ((double.Parse(d[(int)fields.Protein]) * 20) >= double.Parse(d[(int)fields.Calories])) {
                    highproteinproducts["highprotein"]++;
                } else if ((double.Parse(d[(int)fields.Protein])*50) > double.Parse(d[(int)fields.Calories])) {
                    highproteinproducts["midprotein"]++;
                }
            }
            Highproteinproducts = highproteinproducts;
        }
    }

    
}
