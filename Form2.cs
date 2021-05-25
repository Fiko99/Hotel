using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;


namespace Hotel
{

    public partial class Form2 : Form
    {
        DataTable T_Global, T_Global2; bool dostep;int zmiana1=0; int zmiana2=0; int zmiana3=0;
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
        void QuerySql2(string query)
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
            T_Global2 = dt;
            con.Close();
        }
        void Wyswietl_T()
        {
            listView1.Columns.Add("Numer pokoju", 100);
            listView1.Columns.Add("Dostępność", 100);
            listView1.Columns.Add("Ilość osób", 100);
            listView1.View = View.Details;

            for (int i = 0; i < T_Global.Rows.Count; i++)
            {
                string text = "Dostępny do użytku";
                if (T_Global.Rows[i].ItemArray[1].ToString() == "1")
                {
                    text = "Wyłączony z użytku";
                }
                listView1.Items.Add(T_Global.Rows[i].ItemArray[0].ToString());
                listView1.Items[i].SubItems.Add(text);
                listView1.Items[i].SubItems.Add(T_Global.Rows[i].ItemArray[2].ToString());
            }
        }
        void UpDate(int[] PokojeAll, int[] Dostepnosc)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;port=3307;user id=root;password=banany1234;database=mydb;");
            string query;
            for (int i = 0; i < PokojeAll.Length; i++)
            {
                MySqlCommand cmd;
                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                query = "UPDATE dostepnosc SET dostepnosc = @dos WHERE nr_pokoju = @nr";
                cmd.Parameters.AddWithValue("@dos", Dostepnosc[i]);
                cmd.Parameters.AddWithValue("@nr", PokojeAll[i]);
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                con.Close();
            }
            
        }
        void sprawdzenie(DateTime poczatek, DateTime koniec, DateTime teraz)
        {
            dostep = false; //zakłożono ze zawsze jest dostępny
            if (poczatek.CompareTo(teraz) <= 0 && koniec.CompareTo(teraz) >= 0)
            {
                dostep = true;    
            }
        }
        void OdpowiedniaData(int[] Dostepnosc, int[] PokojeAll)
        {
            DateTime localDate = DateTime.Now;
            string cultureName = "de-DE";
            var culture = new CultureInfo(cultureName);
            DateTime Teraz = localDate;
            DateTime poczatek = localDate, koniec = localDate; //ponieważ nie moze być pusta
            for (int i = 0; i < T_Global2.Rows.Count; i++) //dla poszczególnego pokoju z rezerwacji
            {
                for(int j = 0; j<T_Global.Rows.Count;j++) //szukanie pokoju po całej tablicy pokoi
                {
                    if (int.Parse(T_Global2.Rows[i].ItemArray[2].ToString()) == PokojeAll[j])
                    {
                        poczatek = Convert.ToDateTime(T_Global2.Rows[i].ItemArray[0]);
                        koniec = Convert.ToDateTime(T_Global2.Rows[i].ItemArray[1]);
                        sprawdzenie(poczatek, koniec, Teraz);
                        if (dostep == true) Dostepnosc[j] = 1;//jezeli 0 to pokój wolny
                    }
                }
            }
        }
        private void LoadData()
        {
            try
            {
                QuerySql("Select * FROM dostepnosc");
                int[] PokojeAll = new int[T_Global.Rows.Count];
                for (int i = 0; i < T_Global.Rows.Count; i++)
                {
                    PokojeAll[i] = int.Parse(T_Global.Rows[i].ItemArray[0].ToString());
                }
                int[] Dostepnosc = new int[T_Global.Rows.Count];
                QuerySql2("Select poczatek, koniec ,nr_pokoju FROM rezerwacje");
                //sprawdzanie poszczególnych pokoi
                OdpowiedniaData(Dostepnosc, PokojeAll);
                //wypełniacz
                for (int i = 0; i < T_Global.Rows.Count; i++)
                {
                    if (Dostepnosc[i] != 1) Dostepnosc[i] = 0;
                }
                //1 = dostepny, 0 = niedostępny
                UpDate(PokojeAll, Dostepnosc);
                QuerySql("Select D.nr_pokoju, D.dostepnosc , P.ilosc_osob FROM dostepnosc D INNER JOIN pokoje P ON P.nr_pokoju=D.nr_pokoju");
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public Form2()
        {
            InitializeComponent();
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            try
            {
                QuerySql("Select * FROM dostepnosc");
                int[] PokojeAll = new int[T_Global.Rows.Count];
                for (int i = 0; i < T_Global.Rows.Count; i++)
                {
                    PokojeAll[i] = int.Parse(T_Global.Rows[i].ItemArray[0].ToString());
                }
                int[] Dostepnosc = new int[T_Global.Rows.Count];
                QuerySql2("Select poczatek, koniec ,nr_pokoju FROM rezerwacje");
                //sprawdzanie poszczególnych pokoi
                OdpowiedniaData(Dostepnosc, PokojeAll);
                //wypełniacz
                for (int i = 0; i < T_Global.Rows.Count; i++)
                {
                    if (Dostepnosc[i] != 1) Dostepnosc[i] = 0;
                }
                //1 = dostepny, 0 = niedostępny
                UpDate(PokojeAll, Dostepnosc);
                if (zmiana1 == 0) { QuerySql("Select D.nr_pokoju, D.dostepnosc , P.ilosc_osob FROM dostepnosc D INNER JOIN pokoje P ON P.nr_pokoju=D.nr_pokoju ORDER BY 1 ASC"); zmiana1 = 1; }
                else if (zmiana1 == 1) { QuerySql("Select D.nr_pokoju, D.dostepnosc , P.ilosc_osob FROM dostepnosc D INNER JOIN pokoje P ON P.nr_pokoju=D.nr_pokoju ORDER BY 1 DESC"); zmiana1 = 0; }
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            try
            {
                QuerySql("Select * FROM dostepnosc");
                int[] PokojeAll = new int[T_Global.Rows.Count];
                for (int i = 0; i < T_Global.Rows.Count; i++)
                {
                    PokojeAll[i] = int.Parse(T_Global.Rows[i].ItemArray[0].ToString());
                }
                int[] Dostepnosc = new int[T_Global.Rows.Count];
                QuerySql2("Select poczatek, koniec ,nr_pokoju FROM rezerwacje");
                //sprawdzanie poszczególnych pokoi
                OdpowiedniaData(Dostepnosc, PokojeAll);
                //wypełniacz
                for (int i = 0; i < T_Global.Rows.Count; i++)
                {
                    if (Dostepnosc[i] != 1) Dostepnosc[i] = 0;
                }
                //1 = dostepny, 0 = niedostępny
                UpDate(PokojeAll, Dostepnosc);
                if (zmiana2 == 0) { QuerySql("Select D.nr_pokoju, D.dostepnosc , P.ilosc_osob FROM dostepnosc D INNER JOIN pokoje P ON P.nr_pokoju=D.nr_pokoju ORDER BY 2 ASC"); zmiana2 = 1; }
                else if (zmiana2 == 1) { QuerySql("Select D.nr_pokoju, D.dostepnosc , P.ilosc_osob FROM dostepnosc D INNER JOIN pokoje P ON P.nr_pokoju=D.nr_pokoju ORDER BY 2 DESC"); zmiana2 = 0; }
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            try
            {
                QuerySql("Select * FROM dostepnosc");
                int[] PokojeAll = new int[T_Global.Rows.Count];
                for (int i = 0; i < T_Global.Rows.Count; i++)
                {
                    PokojeAll[i] = int.Parse(T_Global.Rows[i].ItemArray[0].ToString());
                }
                int[] Dostepnosc = new int[T_Global.Rows.Count];
                QuerySql2("Select poczatek, koniec ,nr_pokoju FROM rezerwacje");
                //sprawdzanie poszczególnych pokoi
                OdpowiedniaData(Dostepnosc, PokojeAll);
                //wypełniacz
                for (int i = 0; i < T_Global.Rows.Count; i++)
                {
                    if (Dostepnosc[i] != 1) Dostepnosc[i] = 0;
                }
                //1 = dostepny, 0 = niedostępny
                UpDate(PokojeAll, Dostepnosc);
                if (zmiana3 == 0) { QuerySql("Select D.nr_pokoju, D.dostepnosc , P.ilosc_osob FROM dostepnosc D INNER JOIN pokoje P ON P.nr_pokoju=D.nr_pokoju ORDER BY 3 ASC"); zmiana3 = 1; }
                else if (zmiana3 == 1) { QuerySql("Select D.nr_pokoju, D.dostepnosc , P.ilosc_osob FROM dostepnosc D INNER JOIN pokoje P ON P.nr_pokoju=D.nr_pokoju ORDER BY 3 DESC");zmiana3 = 0; }
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(textBox1.Text.ToString()) > 999) throw new Exception("Zły numer pokoju");
                listView1.Clear();
                QuerySql("Select D.nr_pokoju, D.dostepnosc , P.ilosc_osob FROM dostepnosc D INNER JOIN pokoje P ON P.nr_pokoju=D.nr_pokoju WHERE D.nr_pokoju LIKE '%" + textBox1.Text + "%'");
                Wyswietl_T();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
