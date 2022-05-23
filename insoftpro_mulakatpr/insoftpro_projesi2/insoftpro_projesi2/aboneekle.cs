using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;




namespace insoftpro_projesi2
{
    public partial class aboneekle : Form
    {
        public aboneekle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        static string conString = "Data Source=BARIS;Integrated Security=SSPI;Initial Catalog=abone_model";
        //connetionString = "Data Source=BARIS;Initial Catalog=DatabaseName;User ID=UserName;Password=Password"
        //                   Provider=SQLNCLI11;Data Source=BARIS;Integrated Security=SSPI;Initial Catalog=abone_model

        SqlConnection baglanti = new SqlConnection (conString);

        public void veriekle()
        {
            //Hataları engellemeye yönelik olarak, tüm veritabanı işlemlerini try-catch blokları arasında yapmaya özen gösterin.
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                // Bağlantımızı kontrol ediyoruz, eğer kapalıysa açıyoruz.
                string kayit = "insert into aboneler(id,Numara,Acilis_Tarih,Adres,Ad_Soyad,Tarife_Para_Birim_id,Kdv_Oran_Id) values (@id,@numara,@açılıştarih,@adres,@adsoyad,@parabirimi,@kdvoranı)";
                // müşteriler tablomuzun ilgili alanlarına kayıt ekleme işlemini gerçekleştirecek sorgumuz.
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                komut.Parameters.AddWithValue("@id", textBox1.Text);
                komut.Parameters.AddWithValue("@numara", textBox2.Text);
                komut.Parameters.AddWithValue("@açılıştarih", monthCalendar1.SelectionStart.ToString("d"));
                komut.Parameters.AddWithValue("@adres", textBox3.Text);
                komut.Parameters.AddWithValue("@adsoyad", textBox4.Text);
                komut.Parameters.AddWithValue("@parabirimi", comboBox1.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@kdvoranı", comboBox2.SelectedItem.ToString());

                //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                komut.ExecuteNonQuery();
                //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                baglanti.Close();
                MessageBox.Show("Müşteri Kayıt İşlemi Gerçekleşti.");
            }
            catch (Exception hata)
            {
                MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
            }
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }
        private void kayitGetir()
        {
            try
            {
                baglanti.Open();
                string kayit = "SELECT * from aboneler";
                //aboneler tablosundaki tüm kayıtları çekecek olan sql sorgusu.
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                SqlDataAdapter da = new SqlDataAdapter(komut);
                //SqlDataAdapter sınıfı verilerin databaseden aktarılması işlemini gerçekleştirir.
                DataTable dt = new DataTable();
                da.Fill(dt);
                //Bir DataTable oluşturarak DataAdapter ile getirilen verileri tablo içerisine dolduruyoruz.
                dataGridView1.DataSource = dt;
                //Formumuzdaki DataGridViewin veri kaynağını oluşturduğumuz tablo olarak gösteriyoruz.
                baglanti.Close();


            }
            catch (Exception hata)
            {

                MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
            }
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 anasayfa = new Form1();
            anasayfa.StartPosition = FormStartPosition.CenterScreen;
            anasayfa.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            veriekle();
            kayitGetir();
        }

        private void aboneekle_Load(object sender, EventArgs e)
        {
            
            kayitGetir();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                string kayit = "SELECT * from aboneler where Numara=@numara";
                //aboneler tablosundaki tüm alanları isim parametresi
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                komut.Parameters.AddWithValue("@numara", textBox5.Text);
                //isim parametremize textbox'dan girilen değeri aktarıyoruz.
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("İşlem Sırasında Hata Oluştu." + hata.Message);
            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            kayitGetir();
        }
    }
}
