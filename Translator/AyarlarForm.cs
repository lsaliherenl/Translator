using System;
using System.Drawing;
using System.Windows.Forms;

namespace Translator
{
    // ComboBox için yardımcı sınıf
    public class DilSecenegi
    {
        public string GorunenAd { get; set; }
        public string Kod { get; set; }

        public override string ToString() { return GorunenAd; }
    }

    public partial class AyarlarForm : Form
    {
        private TextBox txtTus;
        private TextBox txtApiKey;
        private TextBox txtRegion;
        private ComboBox cmbKaynakDil;
        private ComboBox cmbHedefDil;
        private int secilenTusKodu;

        public AyarlarForm()
        {
            InitializeComponent();
            TasariimiOlustur();
            VerileriYukle();
        }

        private void TasariimiOlustur()
        {
            this.Text = "Uygulama Ayarları";
            this.Size = new Size(360, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int y = 20;

            // 1. GÜVENLİK (API Key)
            Label lblKey = new Label() { Text = "Azure API Key:", Location = new Point(20, y), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            txtApiKey = new TextBox() { Location = new Point(20, y + 25), Width = 300, PasswordChar = '*' }; // Şifreli görünüm

            y += 60;
            Label lblRegion = new Label() { Text = "Azure Region (Örn: global):", Location = new Point(20, y), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            txtRegion = new TextBox() { Location = new Point(20, y + 25), Width = 300 };

            y += 60;
            //  2. KISAYOL
            Label lblTus = new Label() { Text = "Kısayol Tuşu (Basarak değiştirin):", Location = new Point(20, y), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            txtTus = new TextBox() { Location = new Point(20, y + 25), Width = 300, ReadOnly = true, BackColor = Color.White };
            txtTus.KeyDown += TusaBasildi;

            y += 60;
            // 3. DİLLER 
            Label lblKaynak = new Label() { Text = "Resimdeki Dil (Kaynak):", Location = new Point(20, y), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            cmbKaynakDil = new ComboBox() { Location = new Point(20, y + 25), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

            // Kaynak Dil Listesi (Tesseract Dosyaları)
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "İngilizce", Kod = "eng" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Türkçe", Kod = "tur" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Almanca", Kod = "deu" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Fransızca", Kod = "fra" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "İspanyolca", Kod = "spa" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Rusça", Kod = "rus" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Lehçe (Polonya)", Kod = "pol" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Arapça", Kod = "ara" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Japonca", Kod = "jpn" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Korece", Kod = "kor" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Çince (Basit)", Kod = "chi_sim" });
            cmbKaynakDil.Items.Add(new DilSecenegi { GorunenAd = "Portekizce", Kod = "por" });

            y += 60;
            Label lblHedef = new Label() { Text = "Çevrilecek Dil (Hedef):", Location = new Point(20, y), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            cmbHedefDil = new ComboBox() { Location = new Point(20, y + 25), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

            // Hedef Dil Listesi (Azure Kodları)
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Türkçe", Kod = "tr" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "İngilizce", Kod = "en" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Almanca", Kod = "de" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Fransızca", Kod = "fr" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "İspanyolca", Kod = "es" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Rusça", Kod = "ru" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Lehçe (Polonya)", Kod = "pl" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Arapça", Kod = "ar" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Japonca", Kod = "ja" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Korece", Kod = "ko" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Çince (Basit)", Kod = "zh-Hans" });
            cmbHedefDil.Items.Add(new DilSecenegi { GorunenAd = "Portekizce", Kod = "pt" });

            y += 70;
            Button btnKaydet = new Button() { Text = "Ayarları Kaydet ve Uygula", Location = new Point(20, y), Width = 300, Height = 40, BackColor = Color.LightGreen };
            btnKaydet.Click += Kaydet_Click;

            this.Controls.Add(lblKey); this.Controls.Add(txtApiKey);
            this.Controls.Add(lblRegion); this.Controls.Add(txtRegion);
            this.Controls.Add(lblTus); this.Controls.Add(txtTus);
            this.Controls.Add(lblKaynak); this.Controls.Add(cmbKaynakDil);
            this.Controls.Add(lblHedef); this.Controls.Add(cmbHedefDil);
            this.Controls.Add(btnKaydet);
        }

        private void VerileriYukle()
        {
            // Tuş
            secilenTusKodu = Properties.Settings.Default.KısayolTusu;
            txtTus.Text = ((Keys)secilenTusKodu).ToString();

            // API
            txtApiKey.Text = Properties.Settings.Default.ApiKey;
            txtRegion.Text = Properties.Settings.Default.ApiRegion;

            // Diller
            string kayitliKaynak = Properties.Settings.Default.KaynakDil;
            foreach (DilSecenegi dil in cmbKaynakDil.Items)
                if (dil.Kod == kayitliKaynak) cmbKaynakDil.SelectedItem = dil;

            string kayitliHedef = Properties.Settings.Default.HedefDil;
            foreach (DilSecenegi dil in cmbHedefDil.Items)
                if (dil.Kod == kayitliHedef) cmbHedefDil.SelectedItem = dil;
        }

        private void TusaBasildi(object sender, KeyEventArgs e)
        {
            secilenTusKodu = (int)e.KeyCode;
            txtTus.Text = e.KeyCode.ToString();
            e.SuppressKeyPress = true;
        }

        private void Kaydet_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.KısayolTusu = secilenTusKodu;
            Properties.Settings.Default.ApiKey = txtApiKey.Text.Trim();
            Properties.Settings.Default.ApiRegion = txtRegion.Text.Trim();

            if (cmbKaynakDil.SelectedItem is DilSecenegi secilenKaynak)
                Properties.Settings.Default.KaynakDil = secilenKaynak.Kod;

            if (cmbHedefDil.SelectedItem is DilSecenegi secilenHedef)
                Properties.Settings.Default.HedefDil = secilenHedef.Kod;

            Properties.Settings.Default.Save();

            MessageBox.Show("Ayarlar başarıyla kaydedildi! Uygulama yeniden başlatılıyor...");
            Application.Restart();
        }
    }
}