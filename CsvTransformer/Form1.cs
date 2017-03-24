using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CsvTransformer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var stream = new System.IO.FileStream("hello.mat", System.IO.FileMode.Create);
            var writer = new matlab.MatlabWriter(stream);
            writer.WriteHead();
            var Struct = new matlab.MatlabStructuredMatrix()
            {
                Name = new matlab.MatlabStructName() { Value = "aaaa" },
                Size = new matlab.MatlabSize() { Width = 1, Height = 5 },
                Value = new matlab.MatlabDoubleMatrix() { Value = new List<double>() { 1.23456789,2,3,4,5 } }
            };
            writer.Write(Struct);
            writer.Close();
        }
    }
}
