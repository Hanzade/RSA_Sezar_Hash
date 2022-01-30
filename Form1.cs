using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace rsa
{
    public partial class Form1 : Form
    {
            int birinciAsal = 0, ikinciAsal = 0;
            int n = 0;
            int phi = 0;
            int eDegeri = 0;
            double dDegeri = 0;
            string text;
            string text3;
            string text2;
            string hash;
            string desifreHash;
            string veri="";
            string sifre = "";

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        static bool AsalMi(int sayi)
        {
            //Girilen değerlerin asallığının kontrol edildiği blok
            for (int i=2; i<sayi; i++)
            {
                if (sayi % i == 0)
                    return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!AsalMi(Convert.ToInt32(textBox1.Text)) || !AsalMi(Convert.ToInt32(textBox2.Text)))
            {
                MessageBox.Show("Lütfen asal sayılar giriniz!");            //eğer girilen değer asal değilse bu uyarıyı gösterecek
            }
            else
            {           //girilen değer asalsa 
                  //iki asal sayı seçilir
                birinciAsal = Convert.ToInt32(textBox1.Text); //kullanıcı iki asal sayı girer
                ikinciAsal = Convert.ToInt32(textBox2.Text);
                n = birinciAsal * ikinciAsal;
                phi = (birinciAsal - 1) * (ikinciAsal - 1);
                textBox3.Text = n.ToString();
                textBox4.Text = phi.ToString();
                 //e sayısı phi sayısı ile aralarında asal olmalıdır
                 //static bir klas tanımlanmalı ve aralarında asallık durumları kontrol edilmelidir
                for (int i = 2; i < phi; i++)
                {
                    if (OBEB(phi, i) == 1)          //phi sayısına kadar aralarında asallık durumunu sağlayacak e değerleri //obeb metodu çağırıldı
                    {
                        listBox1.Items.Add(i);
                    }
                }
            }

        }
        private void button2_Click(object sender, EventArgs e)          //rsa sifreleme kısmı
        {
            text = textBox6.Text.ToString();
            text3 = RSA(text, n, eDegeri);          //rsa ile şifrelendi
            text2 = RSA(text3, n, (int)dDegeri);    //rsa ile deşifrelendi

            textBox7.Text = text3.ToString();
            textBox8.Text = text2.ToString();
        }
        private void button3_Click(object sender, EventArgs e)          //hash kısmı
        {
            hash = Hash(text);
            hash = RSA(hash, n, (int)dDegeri);              //rsa 
            textBox9.Text = hash.ToString();
            desifreHash = RSA(hash, n, eDegeri);
            textBox10.Text = desifreHash.ToString();

        }

       
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)          //e seçilme kısmı
        {
            label10.Text = listBox1.SelectedItem.ToString();
            eDegeri = Convert.ToInt32(label10.Text);
            dDegeri = DBulma(phi, eDegeri);
            textBox5.Text = dDegeri.ToString();
            label9.Text = n.ToString();
            label12.Text = n.ToString();
            label13.Text = dDegeri.ToString();
        }
        static int OBEB(int x, int y)             //e ile phinin aralarında asallık kontrolü
        {
            int min = Math.Min(x, y);
            int obeb = 1;
            for (int i = 2; i <= min; i++)
            {
                if (x % i == 0 && y % i == 0)
                {
                    obeb = i;
                }
            }
            return obeb;
        }
        static double DBulma(int phi, int eDegeri)                                                                            //d değeri bulma
        {
            int k = 0;
            double d;
            for (int i = 0; i <= k; i++)
            {
                k++;
                d = (double)(1 + i * phi) / (double)eDegeri;
                double hesapla = d - ((int)d);
                if (hesapla == 0)
                {
                    if (1 < d && d < phi)
                        return d;
                }
            }
            return 0;
        }
        static int ModAlma(int a, double b, int n)    // == a^^b(mod(n))                    //üs alma fonksiyonu
        {
            int _a = a % n;      //_a değeri a ile n nin bölümünden kalan değer
            double _b = b;
            if (b == 0)            //eğer sayının üssü (b) sıfır ise sonuç 1 yazdır
            {
                return 1;
            }
            while (_b > 1)        //eğer sayının üssü 1den büyük ise
            {
                _a *= a;        //kendisi ile çarp
                _a %= n;        //n değerine bölündüğünde kalanını bul
                _b--;           //üssü 1 azalt
            }
            return _a;          // _a değerini döndür

        }
       
        static string RSA(string metin2, int m, double d)     //c= şifreli mesaj == j^d(mod(n))
        {
            char[] chars2 = metin2.ToCharArray();               //chars2 adında yeni char dizisi tanımlandı
            StringBuilder builder2 = new StringBuilder();
            for (int j = 0; j < chars2.Length; j++)             //tüm diziyi dolaşacak for döngüsü tanımlandı
            {
                builder2.Append(Convert.ToChar(ModAlma(chars2[j], d, m)));
            }
            return builder2.ToString();                     //sonuç olarak builder2 deki değerler string olarak yazdırılacak
        }
        static string Hash(string metin)                //hash 
        {
            string mesajtxt = metin;

            MD5 md5 = new MD5CryptoServiceProvider();

            string hashSignature = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(mesajtxt)));

            return hashSignature;
        }
        private void button4_Click(object sender, EventArgs e)              //sezar şifreleme
        {
            //textbox temizleme
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            
            veri = textBox11.Text;
            char[] karakterler = veri.ToCharArray();
            foreach (char harf in karakterler)
            {
                textBox12.Text += Convert.ToChar(harf + 5).ToString();
            }

            string hashGonderici = Hash(textBox11.Text);
            hashGonderici = RSA(hashGonderici, n, dDegeri);      //hashle d ile şifrelenir
            textBox14.Text = hashGonderici;

           



        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string sifrelihash = textBox14.Text;
            string cozumluhash = RSA(sifrelihash, n, eDegeri);     //hash i e değeri ile deşifreler
            string hash = Hash(textBox13.Text);

            if (hash==cozumluhash)       //rsa ile şifrelenip gçnderilen hash değeri ile alıcının aldığı metnin hash değerini karşılaştırdı
            {
                MessageBox.Show("Veri bütünlüğü korunmuştur");
            }
            else
            {
                MessageBox.Show("Veri bütünlüğü korunamamıştır");

            }


        }

        private void button5_Click(object sender, EventArgs e)                  //sezar deşifreleme
        {
            sifre = textBox12.Text;
            char[] karakterler2 = sifre.ToCharArray();
            foreach (char harf2 in karakterler2)
            {
                textBox13.Text += Convert.ToChar(harf2 - 5).ToString();
            }
        }
    }
}
