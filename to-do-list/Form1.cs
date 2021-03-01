using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

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
            CreateTable();

            table = new DataTable();
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Description", typeof(String));

            dataGridView1.DataSource = table;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.Columns["Description"].Visible = false;
            dataGridView1.Columns["Title"].Width = 172;

            LoadFromDB();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            textTitle.Clear();
            textDescription.Clear();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            table.Rows.Add(textTitle.Text, textDescription.Text);

            UpdateDB(textTitle.Text, textDescription.Text);

            textTitle.Clear();
            textDescription.Clear();
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            int index;

            if (dataGridView1.CurrentCell != null)
            {
                index = dataGridView1.CurrentCell.RowIndex;
            }
            else
            {
                index = -1;
            }

            if (index > -1)
            {
                textTitle.Text = table.Rows[index].ItemArray[0].ToString();
                textDescription.Text = table.Rows[index].ItemArray[1].ToString();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            DeleteFromDB(index);
            table.Rows[index].Delete();
            LoadFromDB();

            textTitle.Clear();
            textDescription.Clear();
        }

        private void CreateTable()
        {
            string cs = @"Data Source=todo.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS todo
                                (title TEXT,
                                description TEXT)";
            cmd.ExecuteNonQuery();
            
            con.Close();
        }

        private void LoadFromDB()
        {
            string cs = @"Data Source=todo.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM todo";
            using(SQLiteDataReader reader = cmd.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                con.Close();
                table = dt;
                dataGridView1.DataSource = table;
            }

        }

        private void UpdateDB(string title, string description)
        {
            string cs = @"Data Source=todo.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = @"INSERT INTO todo (title, description)
                                VALUES ('" + title + "', '" + description + "');";
            
            cmd.ExecuteNonQuery();

            con.Close();
        }

        private void DeleteFromDB(int index)
        {
            string cs = @"Data Source=todo.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            string title = table.Rows[index].ItemArray[0].ToString();
            string description = table.Rows[index].ItemArray[1].ToString();

            cmd.CommandText = @"DELETE FROM todo
                                WHERE title='" + title + "' AND description= '" + description + "';";

            cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}