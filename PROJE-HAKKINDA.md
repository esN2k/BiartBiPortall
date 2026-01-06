# Biart Mini İş Zekası Portalı - Proje Hakkında

## Projenin Amacı
Bu proje, şirket içindeki raporların kategorilendirilmesi ve kullanıcılar ile paylaşılması için geliştirilmiş bir web uygulamasıdır. Yöneticiler rapor ve kategori yönetimi yapabilir, normal kullanıcılar ise yayınlanmış raporları görüntüleyebilir.

## Neden Bu Proje?
İş dünyasında raporların düzenli bir şekilde saklanması ve ilgili kişilerle paylaşılması önemlidir. Bu proje, bu ihtiyacı karşılamak için basit ve kullanışlı bir çözüm sunmaktadır.

## Kullanılan Teknolojiler ve Neden Seçildiler

### .NET 8
- Modern ve güncel framework
- Dersteki konularla uyumlu
- Performanslı ve güvenli

### ASP.NET Core MVC
- Web uygulamaları için standart mimari
- Model-View-Controller ayrımı sayesinde düzenli kod yapısı
- Öğrenmesi kolay

### Entity Framework Core
- Code First yaklaşımı ile veritabanı yönetimi
- Migration'lar ile veritabanı versiyonlama
- LINQ sorguları ile rahat veri erişimi

### SQLite
- Basit kurulum, dosya tabanlı
- Küçük projeler için ideal
- Sunucu gerektirmiyor

### Bootstrap 5
- Hazır bileşenler ile hızlı geliştirme
- Responsive tasarım desteği
- Modern ve temiz görünüm

## Proje Yapısı

### Areas/Admin
Yönetici paneli ve işlemleri burada bulunur. Sadece Admin rolündeki kullanıcılar erişebilir.

### Controllers
Ana controller'lar (Home, Reports). Normal kullanıcılar için işlemler burada.

### Data
Veritabanı context ve konfigürasyonlar.

### Models
Veri modelleri (Report, Category, ReportCategory).

### Views
Kullanıcı arayüzü sayfaları.

### Migrations
Veritabanı değişiklik kayıtları.

## Özellikler

### Kullanıcı Yönetimi
- ASP.NET Core Identity ile kullanıcı kayıt ve giriş
- Admin ve User rolleri
- Rol bazlı yetkilendirme

### Kategori Yönetimi
- Kategori ekleme, düzenleme, silme
- Sadece yöneticiler erişebilir

### Rapor Yönetimi
- Rapor oluşturma ve düzenleme
- Raporlara birden fazla kategori atama (çoka-çok ilişki)
- Yayında/Taslak durumu
- Sadece yöneticiler yönetebilir

### Rapor Görüntüleme
- Tüm kullanıcılar yayınlanmış raporları görebilir
- Kategoriye göre filtreleme
- Arama özelliği
- Excel'e aktarma

## Zorluklar ve Çözümler

### Çoka-Çok İlişki
**Sorun:** Bir rapor birden fazla kategoriye ait olabilir, bir kategori birden fazla rapor içerebilir.
**Çözüm:** ReportCategory ara tablosu ile ilişki yönetimi. Entity Framework Core ile otomatik yönetim.

### Yetkilendirme
**Sorun:** Admin ve normal kullanıcı erişimlerini ayırmak.
**Çözüm:** `[Authorize(Roles = "Admin")]` attribute'ü ile controller seviyesinde kontrol.

### Excel Export
**Sorun:** Raporları Excel formatında dışa aktarmak.
**Çözüm:** ClosedXML kütüphanesi ile Excel dosyası oluşturma.

## Gelecek Geliştirmeler
- Dashboard'a istatistikler ekleme
- Rapor yorumlama sistemi
- Rapor arşivleme
- Kategori hiyerarşisi (alt kategoriler)
- Gelişmiş arama ve filtreleme

## Kaynaklar
- Microsoft .NET Dokümantasyonu
- Entity Framework Core Dokümantasyonu
- Bootstrap 5 Dokümantasyonu
- Stack Overflow (Sorun çözümlerinde)
