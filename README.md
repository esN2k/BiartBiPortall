# Biart Mini İş Zekası Portalı

## Proje Hakkında
Bu proje, .NET 8 MVC kullanılarak geliştirilmiş bir iş zekası rapor yönetim sistemidir. Kullanıcılar kategorilere göre raporları görüntüleyebilir ve filtreleyebilir. Yöneticiler ise rapor ve kategori yönetimi yapabilir.

## Kullanılan Teknolojiler
- .NET 8
- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQLite Veritabanı
- Bootstrap 5
- ASP.NET Core Identity (Kullanıcı yönetimi)

## Kurulum ve Çalıştırma

### İlk Kurulum
1. Projeyi bilgisayarınıza indirin
2. Terminal veya komut satırında proje klasörüne gidin
3. `dotnet restore` komutunu çalıştırın (NuGet paketlerini yükler)
4. `dotnet ef database update` komutunu çalıştırın (Veritabanını oluşturur)
5. `dotnet run` komutuyla uygulamayı başlatın

### Uygulama Adresi
Varsayılan olarak `https://localhost:5001` adresinden erişilebilir.

## Test Kullanıcıları
Geliştirme ortamında test için hazır kullanıcılar mevcuttur:

**Yönetici Hesabı:**
- Kullanıcı Adı: `admin`
- Şifre: `Admin123!`

Yeni kullanıcı kaydı da `/Identity/Account/Register` sayfasından yapılabilir.

## Özellikler

### Yönetici Paneli
- Kategori ekleme, düzenleme ve silme
- Rapor ekleme, düzenleme ve silme
- Raporlara birden fazla kategori atama
- Raporları yayında/taslak olarak işaretleme

### Kullanıcı Paneli
- Yayınlanmış raporları görüntüleme
- Kategorilere göre filtreleme ve arama
- Raporları Excel formatında indirme
- Rapor detaylarını görüntüleme

## Veritabanı
SQLite veritabanı kullanılmaktadır. Migration'lar ile veritabanı otomatik oluşturulur.
