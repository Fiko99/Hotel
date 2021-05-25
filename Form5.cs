using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Hotel
{
    
    public partial class Form5 : Form
    {
        string imie, nazwisko, pesel, pokoj, cena_za_pobyt,id_osoby; DateTime poczatek, koniec; DataTable T_Global;double id_rezerwacji;
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
        void QueryDoDaty()
        {
            MySqlConnection con = new MySqlConnection("server=localhost;port=3307;user id=root;password=banany1234;database=mydb;");
            MySqlCommand cmd;
            MySqlDataAdapter da;
            DataTable dt;
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT poczatek, koniec FROM rezerwacje WHERE nr_pokoju = @nr ";
            cmd.Parameters.AddWithValue("@nr", pokoj);
            da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            dt = new DataTable();
            da.Fill(dt);
            T_Global = dt;
            con.Close();
        }
        public void Insert1()
        {
            int wyjscie = 0;
            QuerySql("SELECT id_osoby, pesel FROM dane_osobowe ORDER BY 1");
            for(int i = 0; i<T_Global.Rows.Count ;i++)
            {
                if (pesel == T_Global.Rows[i].ItemArray[1].ToString())
                {
                    wyjscie = 1; id_osoby = T_Global.Rows[1].ItemArray[i].ToString();
                }
            }
            if (wyjscie == 0)
            {
                QuerySql("SELECT id_osoby FROM dane_osobowe ORDER BY 1 DESC Limit 1");
                double zmienna = double.Parse(T_Global.Rows[0].ItemArray[0].ToString()) + 1;
                id_osoby = zmienna.ToString();
                MySqlConnection con = new MySqlConnection("server=localhost;port=3307;user id=root;password=banany1234;database=mydb;");
                MySqlCommand cmd;
                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO dane_osobowe VALUES(@id, @pesel, @imie, @nazwisko)";
                cmd.Parameters.AddWithValue("@id", id_osoby);
                cmd.Parameters.AddWithValue("@pesel", pesel);
                cmd.Parameters.AddWithValue("@imie", imie);
                cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                con.Close();
            }

        }
        public void Insert2()
        {
            QuerySql("SELECT nr_rezerwacji FROM rezerwacje ORDER BY 1 DESC Limit 1");
            id_rezerwacji = double.Parse(T_Global.Rows[0].ItemArray[0].ToString()) + 1;
            MySqlConnection con = new MySqlConnection("server=localhost;port=3307;user id=root;password=banany1234;database=mydb;");
            MySqlCommand cmd;
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO platnosci VALUES (@id, @cena, @poczatek, 'NIE')";
            cmd.Parameters.AddWithValue("@id", id_rezerwacji);
            cmd.Parameters.AddWithValue("@cena", cena_za_pobyt);
            cmd.Parameters.AddWithValue("@poczatek", poczatek);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            con.Close();
        }
        public void Insert3()
        {
            
            MySqlConnection con = new MySqlConnection("server=localhost;port=3307;user id=root;password=banany1234;database=mydb;");
            MySqlCommand cmd;
            
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO rezerwacje VALUES (@id1, @id2, @nr, @poczatek, @koniec)";
            cmd.Parameters.AddWithValue("@id1", id_rezerwacji);
            cmd.Parameters.AddWithValue("@id2", id_osoby);
            cmd.Parameters.AddWithValue("@nr", pokoj);
            cmd.Parameters.AddWithValue("@poczatek", poczatek);
            cmd.Parameters.AddWithValue("@koniec", koniec);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            con.Close();
        }
        public void Sprawdzam()
        {
            pesel = textBox3.Text;
            if (pesel.Length != 11) throw new Exception("Zbyt krótki pesel");
            imie = ZmianaLitery(textBox1.Text);//imie
            nazwisko = ZmianaLitery(textBox2.Text);//nazwisko
            poczatek = dateTimePicker1.Value.Date;//data poczatkowa
            koniec = dateTimePicker2.Value.Date;//data koncowa
            if (poczatek.CompareTo(koniec) >= 0) throw new Exception("Początkowa data pobytu musi być wcześniejsza od daty końcowej");
            pokoj = comboBox1.Text;
            int przecinek = pokoj.IndexOf(",", 0, pokoj.Length);
            pokoj = pokoj.Substring(0, przecinek);//ustalanie pokoju
            cena_za_pobyt = PoliczCene(WartoscPokoju(comboBox1.Text));//wyliczanie ceny
            CzyWystepujeData();
        }
        void CzyWystepujeData()
        {
            QueryDoDaty();
            for (int i = 0; i < T_Global.Rows.Count; i++)
            {
                DateTime poczatekZAJETY = Convert.ToDateTime(T_Global.Rows[i].ItemArray[0]);
                DateTime koniecZAJETY = Convert.ToDateTime(T_Global.Rows[i].ItemArray[1]);
                if(0 < poczatek.CompareTo(poczatekZAJETY) && 0 < koniec.CompareTo(koniecZAJETY)) {  }
                else if(0 > poczatek.CompareTo(poczatekZAJETY) && 0 > koniec.CompareTo(koniecZAJETY)) {  }
                else throw new Exception("Pokój w tym terminie jest już zajęty"); 
            }
        }
        public string PoliczCene(string cena)
        {
            double pomocna = double.Parse(koniec.Subtract(poczatek).ToString("dd"));
            return (pomocna * double.Parse(cena)).ToString();
        }
        public string ZmianaLitery(string text)
        {
            string pomocna = text.Remove(1);
            return pomocna.ToUpper() + text.Substring(1);
        }
        public string WartoscPokoju(string text)
        {
            string pomocna = text.Remove(0, 4);
            int przecinek = pomocna.IndexOf(",", 0, pomocna.Length);
            return pomocna.Substring(przecinek + 1);
        }
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
                cmd.CommandText = "Select * FROM rodzaje";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString("nr_pokoju") + ", "
                        + reader.GetString("rodzaj") + ", "
                        + reader.GetString("cena_za_pokój")
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

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dateTimePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dateTimePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public Form5()
        {
            InitializeComponent();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            
            if (char.IsDigit(e.KeyChar))
            {
                if ((sender as TextBox).Text.Count(Char.IsDigit) >= 11)
                e.Handled = true;
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

       private void Form5_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
            else
            {
                try
                {
                    Sprawdzam();
                    Insert1();
                    Insert2();
                    Insert3();
                    MessageBox.Show("Termin został zarezerwowany");
                    comboBox1.SelectedIndex = -1;
                    comboBox1.Text = "";
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }
    }
}
