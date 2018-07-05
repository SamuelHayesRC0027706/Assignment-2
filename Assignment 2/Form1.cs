using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;


namespace Assignment_2
{
    public partial class Form1 : Form
    {

        class row
        {
            public double time;
            public double altitude;
            public double velocity;
            public double acceleration;

        }

        List<row> table = new List<row>();

        public Form1()
        {
            InitializeComponent();
            chart1.Series.Clear();
        }

        private void calculateVelocity()
        //the calculation to workout velocity
        {
            for (int i = 1; i < table.Count; i++)
            {
                double dQ = table[i].altitude - table[i - 1].altitude;
                double dt = table[i].time - table[i - 1].time;
                table[i].velocity = dQ / dt;
            }
        }

        private void calculateAcceleration()
        //the calculation on how to workout acceleration
        {
            for (int i = 2; i < table.Count; i++)
            {
                double dI = table[i].velocity - table[i - 1].velocity;
                double dt = table[i].time - table[i - 1].time;
                table[i].acceleration = dI / dt;
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        //this is to get the open file working on the menu strip, this allows the files to be opened.
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "csv Files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            table.Add(new row());
                            string[] r = sr.ReadLine().Split(',');
                            table.Last().time = double.Parse(r[0]);
                            table.Last().altitude = double.Parse(r[1]);
                        }

                    }
                    calculateVelocity();
                    calculateAcceleration();

                }
                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "failed to open");

                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "is not in the required format");

                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "is not in the required format");

                }
                catch (DivideByZeroException)
                {
                    MessageBox.Show(openFileDialog1.FileName + "has rows that have the same time");
                }
            }
        }

        private void velocityToolStripMenuItem_Click(object sender, EventArgs e)
        //this code works out the velocity and displays it as a graph
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series = new Series
            {
                Name = "Velocity",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            chart1.Series.Add(series);
            foreach (row r in table.Skip(1))
            {
                series.Points.AddXY(r.time, r.velocity);
            }
            chart1.ChartAreas[0].AxisX.Title = "time /s";
            chart1.ChartAreas[0].AxisY.Title = "velocity /m/s";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void altitudeToolStripMenuItem_Click(object sender, EventArgs e)
        //this works out the altidude and displays it in a graph
        {
            {
                chart1.Series.Clear();
                chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
                Series series = new Series
                {
                    Name = "Altitude",
                    Color = Color.Blue,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Spline,
                    BorderWidth = 2
                };
                chart1.Series.Add(series);
                foreach (row r in table.Skip(1))
                {
                    series.Points.AddXY(r.time, r.altitude);
                }
                chart1.ChartAreas[0].AxisX.Title = "time /s";
                chart1.ChartAreas[0].AxisY.Title = "altitude /m";
                chart1.ChartAreas[0].RecalculateAxesScale();
            }
        }

        private void rateOfChangeOfVelocityToolStripMenuItem_Click(object sender, EventArgs e) 
        //this calculates the Rate of chnage of Velocity and displays it in a graph.

        {
            {
                chart1.Series.Clear();
                chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
                Series series = new Series
                {
                    Name = "Rate of Change of Velocity",
                    Color = Color.Blue,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Spline,
                    BorderWidth = 2
                };
                chart1.Series.Add(series);
                foreach (row r in table.Skip(1))
                {
                    series.Points.AddXY(r.time, r.acceleration);
                }
                chart1.ChartAreas[0].AxisX.Title = "time /s";
                chart1.ChartAreas[0].AxisY.Title = "Rate of Change of Velocity /m/s^2";
                chart1.ChartAreas[0].RecalculateAxesScale();
            }
        }

        private void saveCSVToolStripMenuItem_Click(object sender, EventArgs e)
        //this is how to save the CSV files
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "csv Files|*.csv";
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        sw.WriteLine("Time /s, Altitude /m, Velocity /m/s, Rate of change of Velocity m/s");
                        foreach (row r in table)
                        {
                            sw.WriteLine(r.time + "," + r.altitude + "," + "," + r.velocity + "," + r.acceleration);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName + " failed to save");
                }
            }
        }

        private void savePNGToolStripMenuItem_Click(object sender, EventArgs e)
        //this is the code on how to save png files
        {
            {
                saveFileDialog1.FileName = "";
                saveFileDialog1.Filter = "png Files|*.png";
                DialogResult results = saveFileDialog1.ShowDialog();
                if (results == DialogResult.OK)
                {
                    try
                    {
                        chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
                    }
                    catch
                    {
                        MessageBox.Show(saveFileDialog1.FileName + " failed to save");
                    }
                }
            }
        }
    }
}
