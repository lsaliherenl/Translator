# 📖 Ekran Çevirici (Translator)

Ekrandan seçtiğiniz metinleri otomatik olarak okuyup çeviren Windows masaüstü uygulaması. Tesseract OCR ve Azure Translator API kullanarak gerçek zamanlı çeviri hizmeti sunar.

## ✨ Özellikler

- **Ekran Seçimi**: Mouse ile ekrandan istediğiniz alanı seçerek metin okuyabilirsiniz
- **OCR Desteği**: Tesseract OCR ile görüntüden metin çıkarma
- **Çoklu Dil Desteği**: 12 farklı dilde kaynak ve hedef dil desteği
- **Global Hotkey**: Özelleştirilebilir klavye kısayolu ile hızlı erişim
- **System Tray Entegrasyonu**: Arka planda çalışır, görev çubuğunda yer kaplamaz
- **Overlay Sonuç Gösterimi**: Çeviri sonuçları seçilen alanın yanında görüntülenir
- **Arapça RTL Desteği**: Arapça gibi sağdan sola yazılan diller için otomatik yön ayarı
- **Ayarlar Paneli**: API anahtarları, dil seçenekleri ve kısayol tuşu yönetimi

## 📋 Gereksinimler

- **İşletim Sistemi**: Windows 10 veya üzeri
- **.NET Runtime**: .NET 10.0 (Windows)
- **Azure Translator API**: Geçerli bir Azure Translator API anahtarı ve bölge bilgisi
- **İnternet Bağlantısı**: Çeviri işlemleri için gerekli

## 🚀 Kurulum

### 1. Projeyi İndirin

```bash
git clone https://github.com/kullaniciadi/translator.git
cd translator/Translator
```

### 2. Bağımlılıkları Yükleyin

Proje, NuGet paket yöneticisi ile bağımlılıkları otomatik olarak yükler. Visual Studio veya .NET CLI kullanarak restore edebilirsiniz:

```bash
dotnet restore
```

### 3. Projeyi Derleyin

```bash
dotnet build
```

veya Visual Studio'da `F6` tuşuna basarak derleyebilirsiniz.

### 4. Uygulamayı Çalıştırın

```bash
dotnet run
```

veya `bin/Debug/net10.0-windows/Translator.exe` dosyasını çalıştırın.

## 📖 Kullanım

### İlk Kurulum

1. Uygulamayı ilk kez çalıştırdığınızda, system tray'de (saat yanında) bir ikon görünecektir.
2. İkona sağ tıklayın ve **"Ayarlar"** seçeneğine tıklayın.
3. Ayarlar penceresinde:
   - **Azure API Key**: Azure Translator API anahtarınızı girin
   - **Azure Region**: API bölgenizi girin (örn: `global`)
   - **Kısayol Tuşu**: Ekran seçimini başlatmak için kullanmak istediğiniz tuşu seçin
   - **Kaynak Dil**: Resimdeki metnin dilini seçin (OCR için)
   - **Hedef Dil**: Çevrilecek dili seçin
4. **"Ayarları Kaydet ve Uygula"** butonuna tıklayın.

### Çeviri Yapma

1. Çevirmek istediğiniz metni ekranda görünür hale getirin.
2. Ayarladığınız **kısayol tuşuna** basın.
3. Ekran kararacak ve kırmızı bir çerçeve görünecektir.
4. Mouse ile çevirmek istediğiniz metin alanını seçin (sürükle-bırak).
5. Seçim tamamlandığında, uygulama otomatik olarak:
   - Seçilen alanın ekran görüntüsünü alır
   - OCR ile metni okur
   - Azure Translator API ile çevirir
   - Sonucu seçilen alanın yanında gösterir

### Sonuç Penceresi

- Çeviri sonucu, seçilen alanın hemen altında görüntülenir.
- Sonuç penceresini kapatmak için:
  - Pencereye tıklayın
  - ESC tuşuna basın
  - Başka bir yere tıklayın

### İptal Etme

Ekran seçimi sırasında **ESC** tuşuna basarak işlemi iptal edebilirsiniz.

## ⚙️ Ayarlar

Ayarlar penceresine erişmek için:
- System tray ikonuna **sağ tıklayın** → **"Ayarlar"**
- System tray ikonuna **çift tıklayın**

### Ayarlanabilir Parametreler

- **Azure API Key**: Microsoft Azure Translator API anahtarınız
- **Azure Region**: API bölgeniz (genellikle `global`)
- **Kısayol Tuşu**: Ekran seçimini başlatan tuş (varsayılan: ayarlardan seçilir)
- **Kaynak Dil**: OCR için kullanılacak dil (Tesseract dil dosyası)
- **Hedef Dil**: Çevrilecek hedef dil (Azure Translator kodu)

## 🌍 Desteklenen Diller

### Kaynak Diller (OCR)
Uygulama, Tesseract OCR ile aşağıdaki dilleri destekler:

- 🇬🇧 İngilizce (`eng`)
- 🇹🇷 Türkçe (`tur`)
- 🇩🇪 Almanca (`deu`)
- 🇫🇷 Fransızca (`fra`)
- 🇪🇸 İspanyolca (`spa`)
- 🇷🇺 Rusça (`rus`)
- 🇵🇱 Lehçe (`pol`)
- 🇸🇦 Arapça (`ara`)
- 🇯🇵 Japonca (`jpn`)
- 🇰🇷 Korece (`kor`)
- 🇨🇳 Çince - Basit (`chi_sim`)
- 🇵🇹 Portekizce (`por`)

### Hedef Diller (Çeviri)
Azure Translator API ile aşağıdaki dillere çeviri yapılabilir:

- 🇹🇷 Türkçe (`tr`)
- 🇬🇧 İngilizce (`en`)
- 🇩🇪 Almanca (`de`)
- 🇫🇷 Fransızca (`fr`)
- 🇪🇸 İspanyolca (`es`)
- 🇷🇺 Rusça (`ru`)
- 🇵🇱 Lehçe (`pl`)
- 🇸🇦 Arapça (`ar`)
- 🇯🇵 Japonca (`ja`)
- 🇰🇷 Korece (`ko`)
- 🇨🇳 Çince - Basit (`zh-Hans`)
- 🇵🇹 Portekizce (`pt`)

## 🛠️ Teknolojiler

- **.NET 10.0**: Ana framework
- **Windows Forms**: Kullanıcı arayüzü
- **Tesseract OCR 5.2.0**: Görüntüden metin okuma
- **Azure Translator API**: Çeviri servisi
- **System Tray API**: Arka plan çalıştırma
- **Global Hotkey API**: Klavye kısayolları

## 🔧 Sorun Giderme

### "OCR Hatası" Mesajı Alıyorum

- `tessdata` klasörünün uygulama dizininde olduğundan emin olun
- Seçtiğiniz kaynak dil için ilgili `.traineddata` dosyasının mevcut olduğunu kontrol edin
- Dosya yolu boşluk veya özel karakter içermemelidir

### "Lütfen Ayarlar menüsünden Azure API Anahtarınızı girin" Mesajı

- System tray ikonuna sağ tıklayıp **"Ayarlar"** seçeneğine gidin
- Azure API Key ve Region alanlarını doldurun
- Ayarları kaydedin ve uygulamayı yeniden başlatın

### Çeviri Yapılmıyor / API Hatası

- İnternet bağlantınızı kontrol edin
- Azure API anahtarınızın geçerli olduğundan emin olun
- API bölgesinin doğru girildiğini kontrol edin
- Azure hesabınızda yeterli kredi/quotanın olduğunu kontrol edin

### Kısayol Tuşu Çalışmıyor

- Ayarlardan farklı bir tuş deneyin
- Başka bir uygulama aynı kısayolu kullanıyor olabilir
- Uygulamayı yönetici olarak çalıştırmayı deneyin

### Sonuç Penceresi Görünmüyor

- Seçilen alanın ekran dışında kalmadığından emin olun
- Başka bir pencere sonuç penceresini kapatmış olabilir
- Uygulamayı yeniden başlatmayı deneyin

### System Tray İkonu Görünmüyor

- Windows bildirim alanı simgelerini kontrol edin
- İkon gizlenmiş olabilir, bildirim alanını genişletin
- Uygulamayı yeniden başlatın

## 📝 Lisans

Bu proje [MIT License](LICENSE) altında lisanslanmıştır.

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen şu adımları izleyin:

1. Bu projeyi fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Bir Pull Request oluşturun

### Katkıda Bulunurken Dikkat Edilmesi Gerekenler

- Kod standartlarına uyun
- Yeni özellikler için testler ekleyin
- README.md dosyasını güncelleyin
- Commit mesajlarınızı açıklayıcı yazın

## 📧 İletişim

Sorularınız, önerileriniz veya hata bildirimleri için:

- GitHub Issues: [Proje Issues Sayfası](https://github.com/kullaniciadi/translator/issues)
- Email: [E-posta adresiniz]

## 🙏 Teşekkürler

- [Tesseract OCR](https://github.com/tesseract-ocr/tesseract) - OCR motoru
- [Azure Translator](https://azure.microsoft.com/services/cognitive-services/translator/) - Çeviri servisi
- Tüm katkıda bulunanlara teşekkürler!

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!

