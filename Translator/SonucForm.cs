using System;
using System.Drawing;
using System.Linq; // Harf kontrolü (Any) için gerekli
using System.Windows.Forms;

namespace Translator
{
    public partial class SonucForm : Form
    {
        public SonucForm(string metin, int x, int y, int h)
        {
            InitializeComponent();

            // --- GÖRÜNÜM AYARLARI ---
            this.FormBorderStyle = FormBorderStyle.None; // Çerçeve yok
            this.BackColor = Color.FromArgb(40, 40, 40); // Koyu tema
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true; // Hep en üstte
            this.Padding = new Padding(10);

            // Görev çubuğunda görünmesin (Alt tarafta yer kaplamaz)
            this.ShowInTaskbar = false;

            // Label (Yazı) Ayarları
            Label lblMetin = new Label();
            lblMetin.Text = metin;
            lblMetin.ForeColor = Color.White;
            lblMetin.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblMetin.AutoSize = true;
            lblMetin.MaximumSize = new Size(400, 0); // Genişlik sınırı
            lblMetin.Location = new Point(10, 10);

            // --- ARAPÇA VE LEHÇELER İÇİN YÖN AYARI ---
            // Unicode tablosunda Arapça karakterler 0x0600 ile 0x06FF arasındadır.
            // Eğer metinde bu aralıkta bir harf varsa yönü değiştiriyoruz.
            if (metin.Any(c => c >= 0x0600 && c <= 0x06FF))
            {
                lblMetin.RightToLeft = RightToLeft.Yes;       // Yönü Sağdan Sola yap
                lblMetin.TextAlign = ContentAlignment.TopRight; // Sağa yasla
            }
            else
            {
                lblMetin.RightToLeft = RightToLeft.No;        // Normal (Soldan Sağa)
                lblMetin.TextAlign = ContentAlignment.TopLeft;  // Sola yasla
            }
            // -------------------------------------------

            this.Controls.Add(lblMetin);

            // Formu metne göre boyutlandır
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Konumlandırma (Seçilen alanın altına)
            this.Location = new Point(x, y + h + 5);

            // --- KAPATMA OLAYLARI ---

            // 1. Yazının veya Kutunun üzerine tıklayınca kapat
            this.Click += (s, e) => this.Close();
            lblMetin.Click += (s, e) => this.Close();

            // 2. Başka bir yere tıklayınca (Odak kaybedince) kapat
            this.Deactivate += (s, e) => this.Close();
        }

        // ESC tuşuna basınca kapat
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
            base.OnKeyDown(e);
        }

        // Form açıldığında odağı üzerine alsın (Deactivate çalışması için şart)
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Activate();
            this.Focus();
        }
    }
}