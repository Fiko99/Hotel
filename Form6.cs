using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Hotel
{
    public partial class Form6 : Form
    {
        bool CzyLiczba(string text)
        {
            bool exit = true;
            for (int i = 0; i < text.Length; i++) { if (false == Char.IsNumber(text, i)){ exit = false;break; } }
            return exit;
        }
        bool CzyZnak(string text)
        {
            bool exit = true;
            for (int i = 0; i < text.Length; i++) { if (true == Char.IsNumber(text, i)) { exit = false; break; } }
            return exit;
        }
        DataTable T_Global; int zmiana1 = 0; int zmiana2 = 0; int zmiana3 = 0;
        void QuerySql(string query)
        {
            listView1.Clear();
            MySqlConnection con = new MySqlConnection("server=localhost;port=3307;user id=root;password=banany1234;database=mydb;");
            MySqlCommand cmd;
            MySqlDataAdapter da;
            DataTable dt;
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = query;
            da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            dt = new DataTable();
            da.Fill(dt);
            T_Global = dt;
            con.Close();
        }
        void Wyswietl_T()
        {
            listView1.Columns.Add("ID osoby", 100);
            listView1.Columns.Add("PESEL", 100);
            listView1.Columns.Add("Imię", 100);
            listView1.Columns.Add("Nazwisko", 100);
            listView1.View = View.Details;

            for (int i = 0; i < T_Global.Rows.Count; i++)
            {
                listView1.Items.Add(T_Global.Rows[i].ItemArray[0].ToString());
                listView1.Items[i].SubItems.Add(T_Global.Rows[i].ItemArray[1].ToString());
                listView1.Items[i].SubItems.Add(T_Global.Rows[i].ItemArray[2].ToString());
                listView1.Items[i].SubItems.Add(T_Global.Rows[i].ItemArray[3].ToString());
            }
        }

        private void LoadData()
        {
            try
            {
                QuerySql("Select * FROM dane_osobowe");
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public Form6()
        {
            InitializeComponent();
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Clear();
                if (zmiana1 == 0) { QuerySql("Select * FROM dane_osobowe ORDER BY 1 ASC"); zmiana1 = 1; }
                else if (zmiana1 == 1) { QuerySql("Select * FROM dane_osobowe ORDER BY 1 DESC"); zmiana1 = 0; }
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Clear();
                if (zmiana2 == 0) { QuerySql("Select * FROM dane_osobowe ORDER BY 3 ASC"); zmiana2 = 1; }
                else if (zmiana2 == 1) { QuerySql("Select * FROM dane_osobowe ORDER BY 3 DESC"); zmiana2 = 0; }
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Clear();
                if (zmiana3 == 0) { QuerySql("Select * FROM dane_osobowe ORDER BY 4 ASC"); zmiana3 = 1; }
                else if (zmiana3 == 1) { QuerySql("Select * FROM dane_osobowe ORDER BY 4 DESC"); zmiana3 = 0; }
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string text1 = textBox1.Text;
            string text2 = comboBox1.Text;
            switch (text2)
            {
                case "Nr_ID":
                    if (false == CzyLiczba(text1)) { MessageBox.Show("Zawiera litery"); break; }
                    QuerySql("Select * FROM dane_osobowe WHERE id_osoby LIKE '%" + textBox1.Text + "%'ORDER BY 1");
                    Wyswietl_T();
                    break;
                case "Imię":
                    if (false == CzyZnak(text1)) { MessageBox.Show("Zawiera cyfry"); break; }
                    QuerySql("Select * FROM dane_osobowe WHERE imie LIKE '%" + textBox1.Text + "%'ORDER BY 3");
                    Wyswietl_T();
                    break;
                case "Nazwisko":
                    if (false == CzyZnak(text1)) { MessageBox.Show("Zawiera cyfry"); break; }
                    QuerySql("Select * FROM dane_osobowe WHERE nazwisko LIKE '%" + textBox1.Text + "%'ORDER BY 4");
                    Wyswietl_T();
                    break;
                default:
                    MessageBox.Show("Wybierz kolumnę do wyszukania");
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            LoadData();
            listView1.Refresh();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsSeparator(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
