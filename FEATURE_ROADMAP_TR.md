# CardCrisis Geliştirme Yol Haritası (Mobil / Unity C#)

Bu doküman, mevcut proje yapısına göre **animasyon, karakter, efekt, API, backend hariç** kısa vadede eklenebilecek özellikleri ve önerilen sırayı listeler.

## 1) Hemen Eklenebilir (Lokal, backend olmadan)

### Ana Menü (Clash Royale tarzı akış)
- Açılış ekranı / logo
- Ana Menü: Oyna, Kartlar, Envanter, Ayarlar, Çıkış
- Coin/Gem benzeri lokal para birimi göstergesi
- Profil adı ve seviye (lokal)
- Menüde günlük ödül butonu (lokal sayaç)

### Oyun Akışı Ekranları
- Savaş öncesi “desteyi onayla” ekranı
- Pause menüsü
- Oyun sonu ekranı (kazanma/kaybetme + ödül özeti)
- Basit görev ekranı (ör. 3 dalga tamamla, 20 robot öldür)

### Kart Sistemi (Senin istediğin yapı)
- Default 7 kartın ScriptableObject ile tanımlanması
- Her kart için benzersiz stat/özellik:
  - Hasar bonusu
  - Atış hızı
  - Can artışı
  - Kritik şans
  - Mermi hızı
  - Dalga başı kalkan
  - Boss’a ekstra hasar
- Maç başlangıcında bu 7 karttan **6’sının rastgele seçilmesi**
- Aktif destenin maç içinde pasif etkilerinin oyuncuya uygulanması

### Envanter
- Kart koleksiyonu ekranı
- Kart filtreleme/sıralama (nadirlik, seviye, tür)
- Kart detay popup
- Kart yükseltme (lokal kaynak harcayarak)

### İlerleme ve Kayıt (Lokal)
- PlayerPrefs veya JSON dosyası ile kayıt:
  - Coin, seviye, kart seviyeleri
  - Açılan kartlar
  - En iyi skor
  - Son kullanılan deste
- İlk açılışta starter profil oluşturma

### Mobil UX / UI
- Safe Area desteği
- Farklı çözünürlükler için responsive UI
- Dokunmatik giriş optimizasyonu
- Menü geçişlerinde hafif UI tween geçişleri

### Oynanış Dengeleme
- Dalga zorluk eğrisi tuning
- Kart etkilerinin stack kuralları
- Boss güç eğrisi
- Oyun ekonomisi (ödül / upgrade maliyeti)

### Sistem ve Altyapı (Lokal)
- GameState yönetimi (Menu, Loadout, InGame, Result)
- Basit servis katmanı (SaveService, CardService, EconomyService)
- Editor araçları (kart reset, save wipe, debug panel)

## 2) Orta Vadede Eklenebilir (Yine backend’siz devam edebilir)
- Günlük görev rotasyonu (tamamen lokal)
- Basit başarımlar sistemi
- Haftalık “lokal lig” simülasyonu (sunucusuz)
- Reklam ödülü simülasyonu (gerçek SDK bağlamadan)
- Eğitim/Tutorial akışı

## 3) Backend Önerisi (sonradan bağlamak için)

Backend’i en sona bırakmak mantıklı. Önce lokal mimariyi backend’e hazır kurarız.

### Önerilen seçenekler
- **Hızlı MVP:** Firebase (Auth + Firestore + Cloud Functions)
- **Daha esnek/pro yaklaşım:** Unity Gaming Services + custom backend (ASP.NET Core veya Node.js)
- **Tam kontrol:** ASP.NET Core Web API + PostgreSQL + Redis

### Bu proje için önerim
- Önce tüm sistemi lokal servis arayüzleriyle yazalım (`IPlayerDataStore`, `IInventoryStore`, `IMatchResultStore`).
- İlk etapta bu arayüzlerin `Local*` implementasyonları çalışsın.
- Sonra aynı arayüzlerin `Remote*` implementasyonlarını yazarız, UI/oyun mantığı değişmeden backend’e geçeriz.

## 4) Önceliklendirme (önerilen sprint sırası)
1. Ana Menü + ekran akışı
2. Kart veri modeli (7 kart) + rastgele 6 kart seçimi
3. Envanter + kart detay + yükseltme
4. Lokal save/load
5. Oyun içine kart etkilerini uygulama
6. Dengeleme + polish
7. Backend adaptasyon katmanı

## 5) Teknik Not
Mevcut projede dalga, düşman HP ve game over temelleri hazır olduğu için, bu roadmap doğrudan mevcut kod üzerine eklenebilir.
