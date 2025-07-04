# TodoAPI - Domain Driven Design ile Çoklu Veritabanı Desteği

Bu proje, **.NET 9** kullanılarak geliştirilmiş, **Domain Driven Design (DDD)** prensipleriyle yapılandırılmış bir Todo API uygulamasıdır.  
Projede **PostgreSQL** ve **SQL Server** olmak üzere iki farklı veritabanı desteklenmekte ve çalışma zamanında istenilen veritabanı seçilebilmektedir.

---

## İçindekiler

- [Genel Bakış](#genel-bakış)
- [Mimari - Domain Driven Design (DDD)](#mimari---domain-driven-design-ddd)
- [Çoklu Veritabanı Desteği ve Geçiş](#çoklu-veritabanı-desteği-ve-geçiş)
- [Guid Id ve Veritabanları Arası Uyum](#guid-id-ve-veritabanları-arası-uyum)
- [Migration'ları Import etmek](#migrationlar-ve-versiyonsuz-import)
- [Kurulum ve Çalıştırma](#kurulum-ve-çalıştırma)
- [API Özellikleri](#api-özellikleri)
- [Lisans](#lisans)

---

## Genel Bakış

TodoAPI, klasik bir yapılacaklar listesi uygulaması olup, temel CRUD operasyonlarını sağlar.  
Projede temiz ve sürdürülebilir bir mimari için Domain Driven Design uygulanmış ve veritabanı bağımsızlığı sağlanmıştır.

---

## Mimari - Domain Driven Design (DDD)

- **Domain Katmanı:** İş kuralları ve entity'ler burada bulunur. Örneğin `TodoItem` entity’si.
- **Application Katmanı:** Servisler ve iş mantığı burada yer alır. Domain katmanını kullanarak iş süreçlerini yönetir.
- **Infrastructure Katmanı:** Veritabanı erişimi ve dış kaynak entegrasyonları (ORM, Repository Pattern vs.) burada yer alır.
- **API Katmanı:** Kullanıcıdan gelen HTTP isteklerini karşılayan Controller ve API uç noktaları bu katmanda bulunur.

DDD yaklaşımı sayesinde uygulama iş kurallarına ve domain modeline odaklanır, teknolojik bağımlılıklar altyapı katmanına soyutlanır.

---

## Çoklu Veritabanı Desteği ve Geçiş

Projede PostgreSQL ve SQL Server desteklenir. Veritabanı tercihi `appsettings.json` dosyasındaki `DatabaseProvider` alanıyla belirlenir:

```json
{
  "DatabaseProvider": "postgres", // veya "sqlserver"
  "ConnectionStrings": {
    "PostgresConnection": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres",
    "SqlServerConnection": "Server=localhost,1433;Database=TodoItems;User Id=sa;Password=YourStrongPassword;"
  }
}
```

### DbContext yapılandırması buna göre PostgreSQL ya da SQL Server bağlantısıyla yapılır:

```csharp
if (dbProvider == "postgres")
    options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection"));
else if (dbProvider == "sqlserver")
    options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"));
else
    throw new Exception("Geçersiz veritabanı sağlayıcısı.");
```

Böylece tek bir kod tabanıyla istediğin veritabanına bağlanabilir, geçiş yapabilirsin.  
Migration’lar iki veritabanı için de ayrı oluşturulabilir.

---
## Guid Id ve Veritabanları Arası Uyum

Projede Guid tipinde Id alanı kullanılmıştır ve bu alan, SQL Server’da uniqueidentifier, PostgreSQL’de ise uuid olarak karşılık bulur.

SQL Server: Guid otomatik olarak uniqueidentifier türüne çevrilir.

PostgreSQL: Guid tipi uuid olarak tanımlanır.

Eğer projeyi PostgreSQL’den SQL Server’a (ya da tersi) geçirirken, eski migrationlar ve tablolar uyumsuzluk nedeniyle hata verebilir. Bunun sebebi, migrationların her veritabanı için ayrı ayrı oluşturulması gerektiğidir.

Çözüm:

Var olan migrationları ve veritabanı tablolarını silip temiz bir başlangıç yap.

Daha sonra, hedef veritabanı için migrationları yeniden oluştur ve uygulayarak veritabanını baştan kur.

Böylece Guid alanı doğru veri türüyle (PostgreSQL’de uuid, SQL Server’da uniqueidentifier) tanımlanır.

Özetle: Migration’lar veritabanına özeldir, aynı migrationları farklı veritabanlarında kullanmak hata oluşturabilir. Bu yüzden veritabanı değiştirildiğinde migrationlar sıfırlanmalıdır.
---
## Migration'ları Import etmek

Farklı veritabanları için migrationlar ayrı tutulmalıdır.
Mevcut migrationları temizlemek için:
```bash
dotnet ef migrations remove
```
Eğer tüm migrationları ve veritabanını sıfırlamak istersen:

1.Veritabanını sil veya resetle (örn. Docker konteynerini kaldırıp yeniden oluştur).

2.Migration klasörlerini temizle (migration dosyalarını sil).

3.Yeni veritabanı için migration oluştur:
```bash
dotnet ef migrations add InitialCreate
```
4.Migrationları uygula:
```bash
dotnet ef database update
```
Bu sayede, migrationlar hedef veritabanına uygun şekilde yeniden oluşturulur ve import edilir.
---
## Kurulum ve Çalıştırma

### Gereksinimler

- .NET 9 SDK  
- Docker (PostgreSQL ve SQL Server konteynerleri için)  
- Tercih edilen veritabanı bağlantı ayarları `appsettings.json` içinde yapılandırılmalı

### Adımlar

Projeyi klonla veya indir:

```bash
git clone https://github.com/bilaltozluyurt/TodoAPI.git
cd TodoAPI
```

Docker ile veritabanlarını ayağa kaldır:

```bash
docker-compose up -d
```

> `docker-compose.yml` dosyası mevcutsa

### Migration’ları uygula:

```bash
dotnet ef database update
```

Uygulamayı çalıştır:

```bash
dotnet run
```

API, genellikle `https://localhost:5001` adresinde çalışır.

---

## API Özellikleri

- Todo öğesi oluşturma, listeleme, güncelleme ve silme  
- Domain Driven Design prensiplerine uygun yapı  
- PostgreSQL ve SQL Server veritabanı desteği  
- Swagger ile API dokümantasyonu

---

## Lisans

MIT License © 2025

> Not: Bu proje DDD mimarisi ve çoklu veritabanı yönetimi için örnek teşkil eder. Kendi ihtiyaçlarına göre uyarlayabilir, genişletebilirsin.
