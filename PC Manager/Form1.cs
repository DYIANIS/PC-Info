
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExcelLibrary.SpreadSheet;

namespace PC_Manager
{
    public partial class Form1 : Form
    {

        List<String> tableHead = new List<String>();
        List<String> rawTeble = new List<String>();


        static CommandHandler commandHandler = new CommandHandler();

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Получить данные о ПК");
            tableHead.Add(label1.Text);
            tableHead.Add(label2.Text);
            tableHead.Add(label3.Text);
            tableHead.Add(label4.Text);
            tableHead.Add(label5.Text);
            tableHead.Add(label6.Text);
            tableHead.Add(label7.Text);
            tableHead.Add(label8.Text);
            tableHead.Add(label9.Text);
            tableHead.Add(label10.Text);
            tableHead.Add(label11.Text);
            tableHead.Add(label12.Text);
            tableHead.Add(label13.Text);
            tableHead.Add("Инв. номер");

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "Получить данные о ПК":
                    {
                        string[] data = commandHandler.GetInfo().Split(new string[] { "¥¥¥¥¥/¥¥¥¥¥" }, StringSplitOptions.None);

                        label1.Text = "ОС: " + data[0];
                        label2.Text = "Разрядность ОС: " + data[1];
                        label3.Text = "Объём RAM: " + data[2];
                        label4.Text = "Тип RAM: " + data[3];
                        label5.Text = "Объём HDD: " + data[4];
                        label6.Text = "Процессор: " + data[5];
                        label7.Text = "" + data[6];
                        label8.Text = "Видеокарта: " + data[7];
                        label9.Text = "Материнская плата: " + data[8];
                        label10.Text = "S/N: " + data[9];
                        label11.Text = "BIOS: " + data[10];
                        label12.Text = "Имя компьютера: " + data[11];
                        label13.Text = "Имя пользователя: " + data[12];

                        for (int i = 0; i < data.Length; i++)
                        {
                            rawTeble.Add(data[i]);
                        }

                        rawTeble.Add(textBox1.Text);

                        button2.Enabled = true;
                        break;
                    }

                case "Получить список запущенных процессов":
                    {
                        string process = commandHandler.GetProcess();
                        Form2 form2 = new Form2(process, "Список процессов", 1);
                        form2.Show();
                        break;
                    }

                case "Получить список запущенных служб":
                    {
                        try
                        {
                            string services = commandHandler.GetServices();

                            Form2 form2 = new Form2(services, "Список служб", 0);
                            form2.Show();
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }

                case "Получить список сетевых адаптеров":
                    {
                        string adapters = commandHandler.GetNetwork();

                        Form2 form2 = new Form2(adapters, "Список сетевых адаптеров", 0);
                        form2.Show();

                        break;
                    }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string file = "PC_Info.xls";

            try
            {
                createFile(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка записи в файл! \n" + ex.Message);
                return;
            }

            rawTeble[rawTeble.Count - 1] = textBox1.Text;
            int indexRaw = getIndexRaw(file);

            Workbook workbook = Workbook.Load(file);
            Worksheet worksheet = workbook.Worksheets[0];

            for (int i = 0; i < tableHead.Count; i++)
            {
                worksheet.Cells[i, indexRaw] = new Cell(rawTeble[i]);
            }

            try
            {
                workbook.Save(file);
                MessageBox.Show("Данные сохранены в файл!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void createFile(string file)
        {
            if (isFileExist(file))
            {
                return;
            }

            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("First Sheet");

            for (int i = 0; i < tableHead.Count; i++)
            {
                worksheet.Cells[i, 0] = new Cell(tableHead[i]);
            }

            workbook.Worksheets.Add(worksheet);
            workbook.Save(file);

        }

        private bool isFileExist(string file)
        {
            try
            {
                Workbook workbook = Workbook.Load(file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int getIndexRaw(string file)
        {
            Workbook workbook = null;
            try
            {
                workbook = Workbook.Load(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска нового столбца \n" + ex.Message);
                return 0;
            }

            Worksheet worksheet = workbook.Worksheets[0];

            int raw = 0;

            while (!worksheet.Cells[0, raw].IsEmpty)
            {
                raw++;
            }

            return raw;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("Введите инвентарный номер"))
            {
                textBox1.Text = null;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | textBox1.Text == null)
            {
                textBox1.Text = "Введите инвентарный номер";
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
