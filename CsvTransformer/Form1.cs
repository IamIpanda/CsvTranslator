using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CsvTransformer.csv;
using CsvTransformer.matlab;

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
            var stream = new FileStream("hello.mat", FileMode.Create);
            var writer = new MatlabWriter(stream);
            writer.WriteHead();
            var Struct = new MatlabStructuredMatrix
            {
                Name = new MatlabStructName("FuckMatlab"),
                Size = new MatlabSize(3, 4),
                Value = new MatlabDoubleMatrix(new List<double> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 20})
            };
            writer.Write(Struct);
            writer.Close();
        }

        private void bt_check_all_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clb_files.Items.Count; i++)
                clb_files.SetItemChecked(i, true);
            SetStartButtonState();
        }

        private void bt_uncheck_all_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clb_files.Items.Count; i++)
                clb_files.SetItemChecked(i, false);
            SetStartButtonState();
        }

        private void tb_csv_dir_TextChanged(object sender, EventArgs e)
        {
            SetCheckedListBox();
            SetStartButtonState();
        }

        protected void SetCheckedListBox()
        {
            var pattern = tb_file_filter.Text;
            if (pattern == "") pattern = "*.csv";
            clb_files.Items.Clear();
            var dir = new DirectoryInfo(tb_csv_dir.Text);
            if (!dir.Exists) return;
            if (cb_reg.Checked)
            {
                var reg = new Regex(pattern);
                foreach (var fileInfo in dir.GetFiles("*"))
                    if (reg.Match(fileInfo.Name).Success)
                        clb_files.Items.Add(fileInfo.Name);
            }
            else
                foreach (var fileInfo in dir.GetFiles(pattern))
                        clb_files.Items.Add(fileInfo.Name);
        }

        protected void SetStartButtonState()
        {
            bt_start.Enabled = clb_files.SelectedItems.Count != 0;
        }

        private void clb_files_MouseUp(object sender, MouseEventArgs e)
        {
            SetStartButtonState();
        }

        protected CsvReader reader;

        private void bt_start_Click(object sender, EventArgs e)
        {
            if (cb_merge_file.Checked)
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                timer.Enabled = true;
                process_together_deck(sfd.FileName);
                timer.Enabled = false;
            }
            else
            {
                if (fbd.ShowDialog() != DialogResult.OK) return;
                timer.Enabled = true;
                process_seperate_deck(fbd.SelectedPath);
                timer.Enabled = false;
            }
            MessageBox.Show("处理完成。");
            Application.Exit();
        }

        private List<string> get_file_list()
        {
            return clb_files.CheckedItems.OfType<string>()
                .Select(filename => Path.Combine(tb_csv_dir.Text, filename))
                .ToList();
        }

        public int all_progrss;

        private void process_together_deck(string filename)
        {
            reader = new CsvReader();
            var time_table = new List<List<double>>();
            var value_table = new List<List<double>>();
            var file_list = get_file_list();
            pb_all.Maximum = file_list.Count;
            all_progrss = 0;
            foreach (var file_path in file_list)
            {
                var stream = new FileStream(file_path, FileMode.Open);
                var data = reader.ReadStream(stream);
                if (cb_keep_time.Checked)
                {
                    time_table.Add(data[0]);
                    value_table.Add(data[1]);
                }
                else
                    value_table.Add(data[0]);
                all_progrss += 1;
            }
            var file_stream = new FileStream(filename, FileMode.Create);
            var matlab = new MatlabWriter(file_stream);
            matlab.WriteHead();
            var construct = new MatlabStructuredMatrix
            {
                Name = new MatlabStructName(tb_data_name.Text),
                Size = new MatlabSize(value_table[0].Count, value_table.Count),
                Value = new MatlabDoubleMatrix(value_table)
            };
            matlab.Write(construct);
            if (!cb_keep_time.Checked) return;
            construct = new MatlabStructuredMatrix
            {
                Name = new MatlabStructName("time_table"),
                Size = new MatlabSize(time_table.Count, time_table[0].Count),
                Value = new MatlabDoubleMatrix(time_table)
            };
            matlab.Write(construct);
        }
        
        private void process_seperate_deck(string dirname)
        {
            reader = new CsvReader();
            var file_list = get_file_list();
            pb_all.Maximum = file_list.Count;
            foreach (var file_path in file_list)
            {
                var name = Path.GetFileNameWithoutExtension(file_path);
                var matlab_name = Path.Combine(dirname, name);
                var stream = new FileStream(file_path, FileMode.Open);
                var value_table = reader.ReadStream(stream);
                stream.Close();
                stream = new FileStream(matlab_name, FileMode.Create);
                var matlab_writer = new MatlabWriter(stream);
                matlab_writer.WriteHead();
                var value_construct = genertate_construct(tb_data_name.Text, cb_keep_time.Checked ? );
            }
        }

        private MatlabStructuredMatrix genertate_construct(string name, List<List<double>> value)
        {
            return new MatlabStructuredMatrix
            {
                Name = new MatlabStructName("name"),
                Size = new MatlabSize(value.Count, value[0].Count),
                Value = new MatlabDoubleMatrix(value)
            };
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (reader != null)
                lb_data.Text = $"Reading {reader.Process}";
            pb_all.Value = all_progrss;
        }
    }
}
