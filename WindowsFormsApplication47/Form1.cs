using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;


namespace WindowsFormsApplication47
{
    public partial class Form1 : Form
    {
        Bitmap newBitmap;



        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //open file kullanarak bilgisayardan resim seçiyoruz ve pictureBox'a resmi ekliyoruz.
            openFileDialog1.Filter = "resim dosyaları |" + "*.bmp;*.jpg;*.gif;*.wmf;*.tif;*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                newBitmap = new Bitmap(openFileDialog1.FileName);
            }


        }

        //kullanici buton2'ye tikladiginda resim griye döner.
        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(pictureBox1.Image);
            Bitmap gri = griyap(img);
            pictureBox2.Image = gri;

        }
        //resmi gri yapan fonksiyon
        public Bitmap griyap(Bitmap newBitmap)
        {
            //resmi piksel piksel dolasmak icin ic ice iki for kullandik.
            for (int i = 0; i < newBitmap.Height - 1; i++)
            {
                for (int j = 0; j < newBitmap.Width - 1; j++)
                {
                    //resmi griye ceviren algoritma
                    int deger = (newBitmap.GetPixel(j, i).R + newBitmap.GetPixel(j, i).G + newBitmap.GetPixel(j, i).B) / 3;
                    Color renk;
                    renk = Color.FromArgb(deger, deger, deger);
                    newBitmap.SetPixel(j, i, renk);

                }
            }
            return newBitmap;

        }
        //kullanici buton 3'e bsatiginda resme GaussianBlur filtresi uygulanmis olarak görür
        private void button3_Click(object sender, EventArgs e)
        {
            //gaussianblur algoritması
            //resmi dolaşmak için iç içe for kullanıyoruz
            for (int x = 1; x < newBitmap.Width; x++)
            {
                for (int y = 1; y < newBitmap.Height; y++)
                {
                    try
                    {
                        Color prevX = newBitmap.GetPixel(x - 1, y);
                        Color nextX = newBitmap.GetPixel(x + 1, y);
                        Color prevY = newBitmap.GetPixel(x, y - 1);
                        Color nextY = newBitmap.GetPixel(x, y + 1);

                        int avgR = (int)((prevX.R + nextX.R + prevY.R + nextY.R) / 4);
                        int avgG = (int)((prevX.G + nextX.G + prevY.G + nextY.G) / 4);
                        int avgB = (int)((prevX.B + nextX.B + prevY.B + nextY.B) / 4);

                        newBitmap.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                        pictureBox3.Image = newBitmap;

                    }
                    catch (Exception) { }

                }
            }

        }
        //kullanıcı buton4'e tıklarsa canny algoritması uygulanır.
        private void button4_Click(object sender, EventArgs e)
        {

            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int x, y;
            //resim pixel pixel taranıyor.
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
            //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    Color Renk;
                    int P1,P2,P3,P4,P5,P6,P7,P8,P9;
                    Renk = GirisResmi.GetPixel(x - 1, y - 1);
                    P1 = Renk.R;
                    Renk = GirisResmi.GetPixel(x, y - 1);
                    P2 = Renk.R;
                    Renk = GirisResmi.GetPixel(x + 1, y - 1);
                    P3 = Renk.R;
                    Renk = GirisResmi.GetPixel(x - 1, y);
                    P4 = Renk.R;
                    Renk = GirisResmi.GetPixel(x, y);
                    P5 = Renk.R;
                    Renk = GirisResmi.GetPixel(x + 1, y);
                    P6 = Renk.R;
                    Renk = GirisResmi.GetPixel(x - 1, y + 1);
                    P7 = Renk.R;
                    Renk = GirisResmi.GetPixel(x, y + 1);
                    P8 = Renk.R;
                    Renk = GirisResmi.GetPixel(x + 1, y + 1);
                    P9 = Renk.R;
                    //Hesaplamayı yapan Sobel Temsili matrisi ve formülü.
                    int RenkDegeri = Math.Abs((P1 + 2 * P2 + P3) - (P7 + 2 * P8 + P9)) + Math.Abs((P3 + 2 *
                   P6 + P9) - (P1 + 2 * P4 + P7));
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                    if (RenkDegeri > 255) RenkDegeri = 255;
                    CikisResmi.SetPixel(x, y, Color.FromArgb(RenkDegeri, RenkDegeri, RenkDegeri));
                }
            }
            //işlenen resim picturbox4'te görüntüleniyor.
            pictureBox4.Image = CikisResmi;


        }
    }
}