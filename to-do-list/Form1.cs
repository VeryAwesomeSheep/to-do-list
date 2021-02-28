using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite; // TODO: Implement saving tasks to sql

namespace to_do_list
{
    public partial class Form1 : Form
    {
        DataTable table;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            table = new DataTable();
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Description", typeof(String));

            dataGridView1.DataSource = table;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.Columns["Description"].Visible = false;
            dataGridView1.Columns["Title"].Width = 172;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            textTitle.Clear();
            textDescription.Clear();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            table.Rows.Add(textTitle.Text, textDescription.Text);

            textTitle.Clear();
            textDescription.Clear();
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            if (index > -1)
            {
                textTitle.Text = table.Rows[index].ItemArray[0].ToString();
                textDescription.Text = table.Rows[index].ItemArray[1].ToString();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            table.Rows[index].Delete();
            
            textTitle.Clear();
            textDescription.Clear();
        }
    }
}
