using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ЛАБА2._1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace ЛАБА2._1
{
    public partial class Form1 : Form
    {

        private List<double> values = new List<double>();

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 2;
          


            numericUpDown1.Minimum = -1000;
            numericUpDown1.Maximum = 1000;
            numericUpDown2.Minimum = -1000;
            numericUpDown2.Maximum = 1000;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 100;


            button2.Enabled = false;

            numericUpDown1.Validating += NumericUpDown_Validating;
            numericUpDown2.Validating += NumericUpDown_Validating;
        }


        private void NumericUpDown_Validating(object sender, CancelEventArgs e)
        {
            if (values == null || values.Count == 0) return;

            double min = (double)numericUpDown1.Value;
            double max = (double)numericUpDown2.Value;

            // Проверяем, есть ли точки в выбранном диапазоне
            bool hasPointsInRange = values.Any(v => v >= min && v <= max);

            if (!hasPointsInRange)
            {
                MessageBox.Show($"В диапазоне от {min} до {max} нет точек данных!\n" +
                               $"Доступный диапазон: от {values.Min():F2} до {values.Max():F2}",
                               "Предупреждение",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);

                // Можно сбросить значения к допустимым
                numericUpDown1.Value = (decimal)values.Min();
                numericUpDown2.Value = (decimal)values.Max();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстовые файлы (*.txt)|*.txt" };
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

             string filename = openFileDialog1.FileName;
            ReadFile(filename);

            button2.Enabled = values.Count > 0;
            if (values.Count > 0)
            {
                MessageBox.Show($"Загружено {values.Count} точек.\nИнтервал фильтрации: от {numericUpDown1.Value} до {numericUpDown2.Value}");
                DrawChart();
            }
        }


        public void ReadFile(string file)
        {
            values.Clear();

            try
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (double.TryParse(line.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                    {
                        values.Add(val);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения файла:" + ex.Message);
            }
        }

        private void DrawChart()
        {
            chart1.Series.Clear();

            double min = (double)numericUpDown1.Value;
            double max = (double)numericUpDown2.Value;

            var filteredPoints = values.Where(v => v >= min && v <= max).ToList();

            Series series = new Series("Координаты");
            switch (comboBox1.SelectedIndex)
            {
                case 0: series.ChartType = SeriesChartType.Point; break;
                case 1: series.ChartType = SeriesChartType.Line; break;
                case 2: series.ChartType = SeriesChartType.Spline; break;
                default: series.ChartType = SeriesChartType.Spline; break;
            }
            series.Color = Color.Blue;
            series.BorderWidth = 2;
            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 6;

            for (int i = 0; i < filteredPoints.Count; i++)
                series.Points.AddXY(i + 1, filteredPoints[i]);

            chart1.Series.Add(series);

            chart1.Titles.Clear();
            chart1.Titles.Add($"Отображено {filteredPoints.Count} из {values.Count}");

            chart1.ChartAreas[0].AxisX.Title = "Номер точки";
            chart1.ChartAreas[0].AxisY.Title = "Значение";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (values.Count == 0) {
                MessageBox.Show("Нет данных для построения графика.");
                return;

            }

            DrawChart();

           
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (values.Count > 0)
            {
                DrawChart();

            }
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            /*    button1.Location = new Point(splitContainer1.Panel1.Width / 2 - 60,
                splitContainer1.Panel1.Height / 20 - 8);
                label1.Location = new Point(splitContainer1.Panel1.Width / 2 - 67,
                splitContainer1.Panel1.Height / 10 + 9);
                checkedListBox1.Location = new Point(splitContainer1.Panel1.Width / 2 - 57,
                 splitContainer1.Panel1.Height / 5 - 8);
                checkedListBox1.Height = splitContainer1.Panel1.Height / 2;
                button2.Location = new Point(splitContainer1.Panel1.Width / 2 - 62,
                 splitContainer1.Panel1.Height - splitContainer1.Panel1.Height / 4 - 6);
                label2.Location = new Point(splitContainer1.Panel1.Width / 2 - 60,
                 splitContainer1.Panel1.Height - splitContainer1.Panel1.Height / 5 + 13);
                comboBox1.Location = new Point(splitContainer1.Panel1.Width / 2 - 60,
                 splitContainer1.Panel1.Height - splitContainer1.Panel1.Height / 5 + 40);

                label3.Location = new Point(splitContainer1.Panel1.Width / 2 - 80, splitContainer1.Panel1.Height / 2 + 10);
                numericUpDown1.Location = new Point(splitContainer1.Panel1.Width / 2 - 80, splitContainer1.Panel1.Height / 2 + 40);
                label4.Location = new Point(splitContainer1.Panel1.Width / 2 + 10, splitContainer1.Panel1.Height / 2 + 10);
                numericUpDown2.Location = new Point(splitContainer1.Panel1.Width / 2 + 10, splitContainer1.Panel1.Height / 2 + 40);

           */
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           
            if (values.Count > 0 && button2.Enabled)
            {
                DrawChart();
            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
           
            if (values.Count > 0 && button2.Enabled)
            {
                DrawChart();

            }

        }

       

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (values.Count > 0 && button2.Enabled)
            {
                DrawChart();

            }
        }
    }
}
















