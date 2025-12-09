using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace Translator
{
    public partial class Form1 : Form
    {
        private string endpoint = "https://api.cognitive.microsofttranslator.com/";

        // GLOBAL HOTKEY ÝÇÝN WINDOWS KÜTÜPHANELERÝ 
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // System Tray Bileþenleri
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public Form1()
        {
            InitializeComponent();

            // Formu tamamen gizle (Sadece kod çalýþsýn)
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.None;

            TraySisteminiKur();
            HotkeyKayitEt();
        }

        // Formun açýlýþta görünmesini engelle
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        private void TraySisteminiKur()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Ayarlar", null, Ayarlar_Click);
            trayMenu.Items.Add("-");
            trayMenu.Items.Add("Çýkýþ", null, Cikis_Click);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Ekran Çevirici (Hazýr)";
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Properties.Resources.TranslatorIcon))
            {
                trayIcon.Icon = new System.Drawing.Icon(ms);
            }

            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;

            trayIcon.DoubleClick += Ayarlar_Click;
        }

        private void HotkeyKayitEt()
        {
            int tusKodu = Properties.Settings.Default.KýsayolTusu;
            // ID: 1, Modifiers: 0 (Sadece tuþ)
            RegisterHotKey(this.Handle, 1, 0, tusKodu);
        }

        // Windows mesajlarýný dinle
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == 1)
            {
                EkranSeciminiBaslat();
            }
            base.WndProc(ref m);
        }

        private void EkranSeciminiBaslat()
        {
            if (Application.OpenForms["OverlayForm"] != null) return;

            OverlayForm secimEkrani = new OverlayForm();
            secimEkrani.ResimSecildi = (gelenResim, koordinatlar) =>
            {
                OCRveCeviriAkisi(gelenResim, koordinatlar);
            };
            secimEkrani.Show();
        }

        private void Ayarlar_Click(object sender, EventArgs e)
        {
            AyarlarForm ayarFormu = new AyarlarForm();
            ayarFormu.ShowDialog();
        }

        private void Cikis_Click(object sender, EventArgs e)
        {
            UnregisterHotKey(this.Handle, 1);
            trayIcon.Visible = false;
            Application.Exit();
        }

        //OCR ÝÞLEMÝ (Görüntü Ýþleme)
        private async void OCRveCeviriAkisi(Bitmap resim, Rectangle koordinatlar)
        {
            string okunanMetin = "";
            string kaynakDil = Properties.Settings.Default.KaynakDil; // Ayarlardan al

            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", kaynakDil, EngineMode.Default))
                {
                    using (var stream = new MemoryStream())
                    {
                        resim.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        stream.Position = 0;
                        using (var img = Pix.LoadFromMemory(stream.ToArray()))
                        using (var page = engine.Process(img))
                        {
                            okunanMetin = page.GetText();
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(okunanMetin)) return;

                // Hedef dili de ayarlardan alýp çeviriye gönderiyoruz
                string hedefDil = Properties.Settings.Default.HedefDil;
                await AzureCeviriYap(okunanMetin, hedefDil, koordinatlar);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"OCR Hatasý ({kaynakDil}): " + ex.Message + "\n\nLütfen 'tessdata' klasöründe dil dosyasýnýn olduðundan emin olun.");
            }
        }

        // AZURE ÇEVÝRÝ ÝÞLEMÝ
        private async Task AzureCeviriYap(string metin, string hedefDil, Rectangle koordinatlar)
        {
            // 1. Güvenlik: Anahtarý Ayarlardan Çek
            string currentKey = Properties.Settings.Default.ApiKey;
            string currentRegion = Properties.Settings.Default.ApiRegion;

            if (string.IsNullOrEmpty(currentKey))
            {
                MessageBox.Show("Lütfen Ayarlar menüsünden Azure API Anahtarýnýzý girin.");
                new AyarlarForm().ShowDialog();
                return;
            }

            string route = $"/translate?api-version=3.0&to={hedefDil}";
            object[] body = new object[] { new { Text = metin } };
            string requestBody = JsonSerializer.Serialize(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                // Anahtarlarý ekle
                request.Headers.Add("Ocp-Apim-Subscription-Key", currentKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", currentRegion);

                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    string jsonSonuc = await response.Content.ReadAsStringAsync();

                    var secenekler = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var sonuclar = JsonSerializer.Deserialize<List<AzureSonuc>>(jsonSonuc, secenekler);

                    if (sonuclar != null && sonuclar.Count > 0)
                    {
                        string cevirilmisMetin = sonuclar[0].translations[0].text;
                        SonucForm sonucPenceresi = new SonucForm(cevirilmisMetin, koordinatlar.X, koordinatlar.Y, koordinatlar.Height);
                        sonucPenceresi.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Çeviri Hatasý: " + ex.Message);
                }
            }
        }
    }

    public class AzureSonuc { public List<Ceviri>? translations { get; set; } }
    public class Ceviri { public string? text { get; set; } public string? to { get; set; } }
}