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
    public partial class Form4 : Form
    {
        DataTable T_Global;
        private void LoadData()
        {
            try
            {
                MySqlConnection con = new MySqlConnection("server=localhost;port=3307;user id=root;password=banany1234;database=mydb;");
                MySqlCommand cmd;
                MySqlDataReader reader;
                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select R.nr_rezerwacji, R.nr_pokoju,P.cena FROM rezerwacje R INNER JOIN platnosci P ON R.nr_rezerwacji=P.id_rezerwacji WHERE P.zaplacone = 'NIE' ORDER BY 1";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString("nr_rezerwacji") + ", "
                        + reader.GetString("nr_pokoju") + ", "
                        + reader.GetString("cena")
                    );
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void UpDate(int nrPokoju)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;port=3307;user id=root;password=banany1234;database=mydb;");
            MySqlCommand cmd;
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            string query = "UPDATE platnosci SET zaplacone = 'TAK' WHERE id_rezerwacji = @nr";
            cmd.Parameters.AddWithValue("@nr", nrPokoju);
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            con.Close();
        }
        void QuerySql(string query)
        {
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
        public Form4()
        {
            InitializeComponent();
            LoadData();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            string text = comboBox1.Text;
            int przecinek = text.IndexOf(",",0, text.Length);
            int liczba = int.Parse(text.Remove(przecinek));
            UpDate(liczba);
            comboBox1.Items.Clear();//tutaj clear
            comboBox1.SelectedIndex = -1;
            comboBox1.Text="";
            if(comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Pobyt został zapłacony.");
            }
            LoadData();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }


}
