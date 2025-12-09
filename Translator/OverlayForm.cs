using System;
using System.Drawing;
using System.Windows.Forms;

namespace Translator
{
    public partial class OverlayForm : Form
    {
        // Değişkenler
        private Point baslangicNoktasi;
        private bool cizimYapiyorMu = false;

        public Action<Bitmap, Rectangle> ResimSecildi;

        public OverlayForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        // Mouse Tuşuna Basınca (Çizim Başlar)
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cizimYapiyorMu = true;
                baslangicNoktasi = e.Location;
            }
            base.OnMouseDown(e);
        }

        // Mouse Hareket Edince (Kare çizilir)
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (cizimYapiyorMu)
            {
                // Ekranı yenile ki çizilen kare görünsün
                this.Invalidate();
            }
            base.OnMouseMove(e);
        }

        // Mouse Tuşunu Bırakınca (Çizim Biter ve Resim Alınır)
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (cizimYapiyorMu)
            {
                cizimYapiyorMu = false;

                // Seçilen alanın koordinatlarını hesapla
                int x = Math.Min(baslangicNoktasi.X, e.X);
                int y = Math.Min(baslangicNoktasi.Y, e.Y);
                int genislik = Math.Abs(baslangicNoktasi.X - e.X);
                int yukseklik = Math.Abs(baslangicNoktasi.Y - e.Y);

                // Eğer çok küçük bir tıklama ise işlem yapma
                if (genislik > 10 && yukseklik > 10)
                {
                    // Ekran görüntüsünü al
                    Bitmap ekranGoruntusu = new Bitmap(genislik, yukseklik);
                    using (Graphics g = Graphics.FromImage(ekranGoruntusu))
                    {
                        // CopyFromScreen, ekranın o anki halini kopyalar
                        // Opacity yüzünden ekran kararık görünüyor ama
                        // bu komut ekranın orijinal (parlak) halini alır.
                        g.CopyFromScreen(x, y, 0, 0, new Size(genislik, yukseklik));
                    }

                    // Ana Forma resmi gönder ve bu pencereyi kapat
                    ResimSecildi?.Invoke(ekranGoruntusu, new Rectangle(x, y, genislik, yukseklik));
                    this.Close();
                }
            }
            base.OnMouseUp(e);
        }

        // Çizim olayını görselleştirmek için (Kırmızı çerçeve)
        protected override void OnPaint(PaintEventArgs e)
        {
            if (cizimYapiyorMu)
            {
                // Mouse'un şu anki konumu
                Point mevcutKonum = this.PointToClient(Cursor.Position);

                // Dikdörtgeni hesapla
                int x = Math.Min(baslangicNoktasi.X, mevcutKonum.X);
                int y = Math.Min(baslangicNoktasi.Y, mevcutKonum.Y);
                int w = Math.Abs(baslangicNoktasi.X - mevcutKonum.X);
                int h = Math.Abs(baslangicNoktasi.Y - mevcutKonum.Y);

                // Kırmızı kalemle çiz
                using (Pen kalem = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(kalem, x, y, w, h);
                }
            }
            base.OnPaint(e);
        }

        // ESC tuşuna basarsa iptal etsin
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            base.OnKeyDown(e);
        }
    }
}