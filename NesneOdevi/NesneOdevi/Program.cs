using System;
using System.Collections.Generic;

namespace KuantumKaos
{
    // 1. ÖZEL HATA YÖNETİMİ [cite: 41, 43]
    public class KuantumCokusuException : Exception
    {
        public KuantumCokusuException(string id) 
            : base($"SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR... (Patlayan Nesne ID: {id})") { }
    }

    // 2. ARAYÜZ (INTERFACE) [cite: 24, 25]
    public interface IKritik
    {
        void AcilDurumSogutmasi(); // [cite: 26]
    }

    // 3. TEMEL YAPI (ABSTRACT CLASS) [cite: 15, 16]
    public abstract class KuantumNesnesi
    {
        // Özellikler [cite: 17]
        public string ID { get; set; } // [cite: 18]

        private double _stabilite;
        public double Stabilite // [cite: 19]
        {
            get { return _stabilite; }
            set
            {
                // Kapsülleme ile 0-100 kontrolü ve Hata Fırlatma
                if (value <= 0)
                {
                    _stabilite = 0;
                    // Stabilite 0 veya altına düşerse hata fırlat 
                    throw new KuantumCokusuException(this.ID);
                }
                else if (value > 100)
                {
                    _stabilite = 100;
                }
                else
                {
                    _stabilite = value;
                }
            }
        }

        public int TehlikeSeviyesi { get; set; } // [cite: 20]

        // Kurucu Metot (Constructor)
        public KuantumNesnesi(string id, int tehlike)
        {
            ID = id;
            TehlikeSeviyesi = tehlike;
            // Başlangıçta varsayılan bir stabilite (örn: 50-90 arası) atayabiliriz 
            // veya manuel set edebiliriz. Burada 100 ile başlatıyorum.
            _stabilite = 100; 
        }

        // Metotlar [cite: 21]
        public abstract void AnalizEt(); // [cite: 22]

        public string DurumBilgisi() // [cite: 23]
        {
            return $"ID: {ID} | Stabilite: %{Stabilite} | Tehlike: {TehlikeSeviyesi}";
        }
    }

    // 4. NESNE ÇEŞİTLERİ [cite: 28]

    // A. VeriPaketi [cite: 30]
    public class VeriPaketi : KuantumNesnesi
    {
        public VeriPaketi(string id) : base(id, 1) { } // Tehlike seviyesi düşük

        public override void AnalizEt()
        {
            Stabilite -= 5; // [cite: 32]
            Console.WriteLine("Veri içeriği okundu."); // [cite: 32]
        }
    }

    // B. KaranlikMadde [cite: 33]
    public class KaranlikMadde : KuantumNesnesi, IKritik // [cite: 34]
    {
        public KaranlikMadde(string id) : base(id, 5) { }

        public override void AnalizEt()
        {
            Stabilite -= 15; // [cite: 35]
        }

        public void AcilDurumSogutmasi()
        {
            Stabilite += 50; // [cite: 27]
            Console.WriteLine($"{ID} soğutuldu. Stabilite yenilendi."); // [cite: 36]
        }
    }

    // C. AntiMadde [cite: 37]
    public class AntiMadde : KuantumNesnesi, IKritik // [cite: 38]
    {
        public AntiMadde(string id) : base(id, 10) { }

        public override void AnalizEt()
        {
            Stabilite -= 25; // [cite: 39]
            Console.WriteLine("Evrenin dokusu titriyor..."); // [cite: 40]
        }

        public void AcilDurumSogutmasi()
        {
            Stabilite += 50; // [cite: 27]
            Console.WriteLine($"{ID} üzerinde kritik soğutma yapıldı!");
        }
    }

    // 5. OYNANIŞ DÖNGÜSÜ (MAIN) [cite: 45]
    class Program
    {
        static void Main(string[] args)
        {
            // Generic List [cite: 55]
            List<KuantumNesnesi> envanter = new List<KuantumNesnesi>();
            Random rnd = new Random();
            int sayac = 1;

            Console.WriteLine("Omega Sektörü Kuantum Veri Ambarı'na Hoşgeldiniz..."); // [cite: 9]

            while (true) // Sonsuz döngü [cite: 46]
            {
                try // 
                {
                    Console.WriteLine("\n--- KUANTUM AMBARI KONTROL PANELİ ---"); // [cite: 47]
                    Console.WriteLine("1. Yeni Nesne Ekle"); // [cite: 48]
                    Console.WriteLine("2. Tüm Envanteri Listele"); // [cite: 49]
                    Console.WriteLine("3. Nesneyi Analiz Et"); // [cite: 50]
                    Console.WriteLine("4. Acil Durum Soğutması Yap"); // [cite: 51]
                    Console.WriteLine("5. Çıkış"); // [cite: 52]
                    Console.Write("Seçiminiz: "); // [cite: 53]

                    string secim = Console.ReadLine();

                    if (secim == "1")
                    {
                        // Rastgele nesne üretimi
                        int tur = rnd.Next(1, 4); // 1-3 arası sayı
                        string yeniId = "NESNE-" + sayac++;
                        KuantumNesnesi yeniNesne = null;

                        switch (tur)
                        {
                            case 1: yeniNesne = new VeriPaketi(yeniId); break;
                            case 2: yeniNesne = new KaranlikMadde(yeniId); break;
                            case 3: yeniNesne = new AntiMadde(yeniId); break;
                        }
                        
                        envanter.Add(yeniNesne);
                        Console.WriteLine($"{yeniNesne.GetType().Name} eklendi: {yeniId}");
                    }
                    else if (secim == "2")
                    {
                        Console.WriteLine("\n--- ENVANTER DURUMU ---");
                        // Polimorfizm ile listeleme [cite: 56]
                        foreach (var nesne in envanter)
                        {
                            Console.WriteLine(nesne.DurumBilgisi()); 
                        }
                    }
                    else if (secim == "3")
                    {
                        Console.Write("Analiz edilecek ID: ");
                        string id = Console.ReadLine();
                        var nesne = envanter.Find(x => x.ID == id);

                        if (nesne != null)
                        {
                            nesne.AnalizEt(); // Polimorfik çağrı
                            Console.WriteLine($"Güncel Stabilite: {nesne.Stabilite}");
                        }
                        else
                        {
                            Console.WriteLine("Nesne bulunamadı!");
                        }
                    }
                    else if (secim == "4")
                    {
                        Console.Write("Soğutulacak ID: ");
                        string id = Console.ReadLine();
                        var nesne = envanter.Find(x => x.ID == id);

                        if (nesne != null)
                        {
                            // Type Checking (is kontrolü) [cite: 57]
                            if (nesne is IKritik kritikNesne)
                            {
                                kritikNesne.AcilDurumSogutmasi();
                            }
                            else
                            {
                                Console.WriteLine("Bu nesne soğutulamaz!"); // [cite: 58]
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nesne bulunamadı!");
                        }
                    }
                    else if (secim == "5")
                    {
                        Console.WriteLine("Çıkış yapılıyor...");
                        break;
                    }
                }
                catch (KuantumCokusuException ex) // 
                {
                    // Game Over senaryosu
                    Console.WriteLine("\n**************************************");
                    Console.WriteLine(ex.Message.ToUpper()); // Büyük harflerle yazdır 
                    Console.WriteLine("**************************************");
                    return; // Programı sonlandır
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Beklenmedik bir hata: {ex.Message}");
                }
            }
        }
    }
}