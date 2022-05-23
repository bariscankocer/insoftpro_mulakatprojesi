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
using System.Xml;

namespace insoftpro_projesi2
{
    public partial class endekshesaplama : Form
    {
        public endekshesaplama()
        {
            InitializeComponent();
        }
        static string conString = "Data Source=BARIS;Integrated Security=SSPI;Initial Catalog=abone_model";
        //connetionString = "Data Source=BARIS;Initial Catalog=DatabaseName;User ID=UserName;Password=Password"
        //                   Provider=SQLNCLI11;Data Source=BARIS;Integrated Security=SSPI;Initial Catalog=abone_model

        SqlConnection baglanti = new SqlConnection(conString);
        DateTime bugun = DateTime.Now;
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

        //ing kullan
        //debug - labellar 

        public void veriekle()
        {
            
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                // Bağlantımızı kontrol ediyoruz, eğer kapalıysa açıyoruz.
                string kayit = "insert into endeks (id,Hesaplama_Tarih,Tarife_Para_Birim_id,Kdv_Oran_Id,Tarife_Birim_Fiyat,Hesaplama_Türü,İlk_Endeks,Son_Endeks,Tuketim,Tarife_Toplam_Fiyat,Tarife_Kdv_Fiyat) values (@id,@hesaplama_tarih,@tarifebirimi,@kdvoranı,@tarifefiyatı,@hesaplamatürü,@ilkendeks,@sonendeks,@tüketim,@tarifetoplamfiyat,@tarifekdvfiyat)";
                // endeks tablomuzun ilgili alanlarına kayıt ekleme işlemini gerçekleştirecek sorgumuz.
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                komut.Parameters.AddWithValue("@id", Convert.ToInt32( label2.Text));
                komut.Parameters.AddWithValue("@hesaplama_tarih",Convert.ToDateTime( bugun));
                string dövizcinsi;
                if (label10.Text == "USD")
                {
                    dövizcinsi ="1".ToString();
                    label17.Text = dövizcinsi.ToString();
                }
                if (label10.Text == "EURO")
                {
                    dövizcinsi = "2".ToString();
                    label17.Text = dövizcinsi.ToString();
                }
                if (label10.Text == "STERLİN")
                {
                    dövizcinsi = "3".ToString();
                    label17.Text = dövizcinsi.ToString();
                }

                label17.Visible = false;
                komut.Parameters.AddWithValue("@tarifebirimi",label17.Text );

                komut.Parameters.AddWithValue("@kdvoranı", label12.Text);
                komut.Parameters.AddWithValue("@tarifefiyatı", (decimal)2);


                if (comboBox1.SelectedItem.ToString()=="Ücret")
                {
                    
                    label18.Text = 2.ToString();

                }
                if (comboBox1.SelectedItem.ToString() == "Tüketim")
                {
                    label18.Text = 1.ToString();
                   

                }
                label18.Visible = false;
                komut.Parameters.AddWithValue("@hesaplamatürü", label18.Text);

                Decimal tüketim = Convert.ToDecimal(textBox2.Text) - Convert.ToDecimal(textBox1.Text);
                Decimal tarifetoplamfiyat = tüketim * decimal.Parse("2");
                komut.Parameters.AddWithValue("@ilkendeks", Convert.ToDecimal( textBox1.Text));
                komut.Parameters.AddWithValue("@sonendeks", Convert.ToDecimal( textBox2.Text));
                komut.Parameters.AddWithValue("@tüketim",tüketim );
                komut.Parameters.AddWithValue("@tarifetoplamfiyat",tarifetoplamfiyat );
                if (label12.Text=="%1")
                {
                    decimal sonuç = tarifetoplamfiyat / 100;
                    sonuç = tarifetoplamfiyat + sonuç;
                    label16.Text = sonuç.ToString();
                }
                if (label12.Text == "%8")
                {
                    decimal sonuç = tarifetoplamfiyat / 100;
                    sonuç = sonuç*8;
                    sonuç = tarifetoplamfiyat + sonuç;
                    label16.Text = sonuç.ToString();
                  
                }

                if (label12.Text == "%18")
                {
                    decimal sonuç = tarifetoplamfiyat / 100;
                    sonuç = sonuç * 18;
                    sonuç = tarifetoplamfiyat + sonuç;
                    label16.Text = sonuç.ToString();

                }
                label16.Visible = false;
                komut.Parameters.AddWithValue("@tarifekdvfiyat", label16.Text);




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
            //textBox1.Clear();
            //textBox3.Clear();
            //textBox4.Clear();
            //comboBox1.SelectedIndex = 0;
            //comboBox2.SelectedIndex = 0;
        }

        public void gecmisdoviz()
        {
            string[] parçala;
            parçala = label4.Text.Split('.');
            string yıldeğeri = parçala[2];
            string aydeğeri = parçala[1];
            string gundeğeri = parçala[0];
            string tarihhepsi = label4.Text.ToString();
        
            
            try
            {
                XmlDocument xmlVerisi = new XmlDocument();
                string tarih = label4.Text.ToString();
                string sondeger = tarih.Split('.').Last();

                string xmlismi = "http://www.tcmb.gov.tr/kurlar/" + yıldeğeri + aydeğeri + "/"+ gundeğeri + aydeğeri + yıldeğeri + ".xml";
                xmlVerisi.Load(xmlismi);

                decimal dolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
                decimal euro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
                decimal sterlin = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "GBP")).InnerText.Replace('.', ','));

                label19.Text = dolar.ToString();
                label20.Text = euro.ToString();
                label21.Text = sterlin.ToString();
            }
        
            catch (XmlException xml)

            {
              
                MessageBox.Show(xml.ToString());
            }

        }
        public void gunlukdoviz()
        {
            string[] parçala;
            parçala = label4.Text.Split('.');
            string yıldeğeri = parçala[2];
            string aydeğeri = parçala[1];
            string gundeğeri = parçala[0];
            string tarihhepsi = label4.Text.ToString();
            try
            {
                XmlDocument xmlVerisi = new XmlDocument();
                string tarih = label4.Text.ToString();
                string sondeger = tarih.Split('.').Last();

                string xmlismi = "http://www.tcmb.gov.tr/kurlar/today.xml";
                xmlVerisi.Load(xmlismi);

                decimal gunlukdolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
                decimal gunlukeuro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
                decimal gunluksterlin = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "GBP")).InnerText.Replace('.', ','));

                label22.Text = gunluksterlin.ToString();
                label23.Text = gunlukeuro.ToString();
                label24.Text = gunlukdolar.ToString();
            }
            catch (XmlException xml)
            {
              
                MessageBox.Show(xml.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 anasayfa = new Form1();
            anasayfa.Show();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            label2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString(); //[0] sütun numarası
            label2.Show();
            label4.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            label4.Show();
            label6.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString(); //[0] sütun numarası
            label6.Show();
            label8.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            label8.Show();
            label10.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString(); //[0] sütun numarası
            label10.Show();
            label12.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            label12.Show();
            
        }

        private void endekshesaplama_Load(object sender, EventArgs e)
        {
            kayitGetir();
            //label19.Visible = false;
            //label20.Visible = false;
            //label21.Visible = false;
            //label22.Visible = false;
            //label23.Visible = false;
            //label24.Visible = false;
            // gecmisdoviz();
            //   gunlukdoviz();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            veriekle();
          //  gecmisdoviz();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            gecmisdoviz();
            gunlukdoviz();
            if (label10.Text == "USD")
            {
                Decimal tüketim = Convert.ToDecimal(textBox2.Text) - Convert.ToDecimal(textBox1.Text);
                decimal geçmişdolar = Convert.ToDecimal(label19.Text);
                decimal anlıkdolar = Convert.ToDecimal(label22.Text);
                decimal sonuç = anlıkdolar - geçmişdolar;
                sonuç = sonuç / geçmişdolar;
                decimal kursonuç = sonuç * 100;
                decimal subedeli = Convert.ToDecimal(tüketim);
                subedeli = subedeli * 2;                           /////////////////////////////// 1 m3 suyun tl cinsinden değeri 2 birim olarak hesaplandı
                subedeli = subedeli / 100;
                decimal kurlusubedeli = subedeli * kursonuç;
                if (label12.Text == "%1")
                {
                    decimal vergisi = kurlusubedeli / 100;
                    decimal birvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%1 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + birvergilisubedeli.ToString());
                }
                if (label12.Text == "%8")
                {
                    decimal vergisi = kurlusubedeli / 100; ;
                    vergisi = vergisi * 8;
                    decimal sekizvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%8 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + sekizvergilisubedeli.ToString());

                }
                if (label12.Text == "%18")
                {
                    decimal vergisi = kurlusubedeli / 100; ;
                    vergisi = vergisi * 18;
                    decimal onsekizvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%18 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + onsekizvergilisubedeli.ToString());

                }
            }
            if (label10.Text == "EURO")
            {

                Decimal tüketim = Convert.ToDecimal(textBox2.Text) - Convert.ToDecimal(textBox1.Text);
                decimal geçmişeuro = Convert.ToDecimal(label20.Text);
                decimal anlıkeuro = Convert.ToDecimal(label23.Text);
                decimal sonuç = anlıkeuro - geçmişeuro;
                sonuç = sonuç / geçmişeuro;
                decimal kursonuç = sonuç * 100;
                decimal subedeli = Convert.ToDecimal(tüketim);
                subedeli = subedeli * 2;                           /////////////////////////////// 1 m3 suyun tl cinsinden değeri 2 birim olarak hesaplandı
                subedeli = subedeli / 100;
                decimal kurlusubedeli = subedeli * kursonuç;
                if (label12.Text == "%1")
                {
                    decimal vergisi = kurlusubedeli / 100;
                    decimal birvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%1 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + birvergilisubedeli.ToString());
                }
                if (label12.Text == "%8")
                {
                    decimal vergisi = kurlusubedeli / 100; ;
                    vergisi = vergisi * 8;
                    decimal sekizvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%8 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + sekizvergilisubedeli.ToString());

                }
                if (label12.Text == "%18")
                {
                    decimal vergisi = kurlusubedeli / 100; ;
                    vergisi = vergisi * 18;
                    decimal onsekizvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%18 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + onsekizvergilisubedeli.ToString());

                }




            }
            if (label10.Text == "İNGİLİZ STERLİNİ")
            {
                Decimal tüketim = Convert.ToDecimal(textBox2.Text) - Convert.ToDecimal(textBox1.Text);

                decimal geçmişsterlin = Convert.ToDecimal(label21.Text);
                decimal anlıksterlin = Convert.ToDecimal(label24.Text);
                decimal sonuç = anlıksterlin - geçmişsterlin;
                sonuç = sonuç / geçmişsterlin;
                decimal kursonuç = sonuç * 100;
                decimal subedeli = Convert.ToDecimal(tüketim);
                subedeli = subedeli * 2;                           /////////////////////////////// 1 m3 suyun tl cinsinden değeri 2 birim olarak hesaplandı
                subedeli = subedeli / 100;
                decimal kurlusubedeli = subedeli * kursonuç;
                if (label12.Text == "%1")
                {
                    decimal vergisi = kurlusubedeli / 100;
                    decimal birvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%1 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + birvergilisubedeli.ToString());
                }
                else if (label12.Text == "%8")
                {
                    decimal vergisi = kurlusubedeli / 100; ;
                    vergisi = vergisi * 8;
                    decimal sekizvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%8 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + sekizvergilisubedeli.ToString());

                }
                else if (label12.Text == "%18")
                {
                    decimal vergisi = kurlusubedeli / 100; ;
                    vergisi = vergisi * 18;
                    decimal onsekizvergilisubedeli = kurlusubedeli + vergisi;
                    MessageBox.Show("%18 vergi ve geçen zamanın kur oranına göre hesaplanmış borcunuz:   " + onsekizvergilisubedeli.ToString());

                }

            }
        }
    }
}
