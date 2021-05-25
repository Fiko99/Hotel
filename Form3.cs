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
    
    public partial class Form3 : Form
    {
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
            listView1.Clear();
            listView1.Columns.Add("Numer rezerwacji", 100);
            listView1.Columns.Add("id osoby", 100);
            listView1.Columns.Add("Numer pokoju", 100);
            listView1.Columns.Add("Początek rezerwacji", 100);
            listView1.Columns.Add("Koniec rezerwacji", 100);
            listView1.View = View.Details;

            for (int i = 0; i < T_Global.Rows.Count; i++)
            {
                listView1.Items.Add(T_Global.Rows[i].ItemArray[0].ToString());
                listView1.Items[i].SubItems.Add(T_Global.Rows[i].ItemArray[1].ToString());
                listView1.Items[i].SubItems.Add(T_Global.Rows[i].ItemArray[2].ToString());
                listView1.Items[i].SubItems.Add(T_Global.Rows[i].ItemArray[3].ToString());
                listView1.Items[i].SubItems.Add(T_Global.Rows[i].ItemArray[4].ToString());
            }
        }
        private void LoadData()
        {
            try
            {
                QuerySql("Select * FROM rezerwacje");
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public Form3()
        {
            InitializeComponent();
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)//sortowanie nr rezerwacji
        {
            listView1.Clear();
            if (zmiana1 == 0){QuerySql("Select * FROM rezerwacje ORDER BY 1 ASC"); zmiana1 = 1; }
            else if (zmiana1 == 1) { QuerySql("Select * FROM rezerwacje ORDER BY 1 DESC"); zmiana1 = 0; }
            Wyswietl_T();
        }

        private void button2_Click(object sender, EventArgs e)//sortowanie nr id
        {
            listView1.Clear();
            if (zmiana2 == 0) { QuerySql("Select * FROM rezerwacje ORDER BY 2 ASC"); zmiana2 = 1; }
            else if (zmiana2 == 1) { QuerySql("Select * FROM rezerwacje ORDER BY 2 DESC"); zmiana2 = 0; }
            Wyswietl_T();
        }

        private void button3_Click(object sender, EventArgs e)//sortowanie nr pokoju
        {
            listView1.Clear();
            if (zmiana3 == 0) { QuerySql("Select * FROM rezerwacje ORDER BY 3 ASC"); zmiana3 = 1; }
            else if (zmiana3 == 1) { QuerySql("Select * FROM rezerwacje ORDER BY 3 DESC"); zmiana3 = 0; }
            Wyswietl_T();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string text1 = textBox1.Text;
            string text2 =comboBox1.Text;
            switch (text2)
            {
                case "Nr_Rezerwacji":
                    QuerySql("Select * FROM rezerwacje WHERE nr_rezerwacji LIKE '%" + textBox1.Text + "%'ORDER BY 1");
                    Wyswietl_T();
                    break;
                case "ID_Osoby":
                    QuerySql("Select * FROM rezerwacje WHERE id_osoby LIKE '%" + textBox1.Text + "%'ORDER BY 2");
                    Wyswietl_T();
                    break;
                case "Nr_Pokoju":
                    if (text1.Length > 3) { MessageBox.Show("Nr Pokoju może mieć maksymalnie 3 znaki"); break; }
                    QuerySql("Select * FROM rezerwacje WHERE nr_pokoju LIKE '%" + textBox1.Text + "%'ORDER BY 3");
                    Wyswietl_T();
                    break;
                default:
                    MessageBox.Show("Wybierz kolumnę do wyszukania");
                    break;
            }
                
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            LoadData();
            listView1.Refresh();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
