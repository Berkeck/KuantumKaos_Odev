const readline = require('readline');

// Konsol Okuma Arayüzü
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

// Yardımcı Fonksiyon: Input almak için
const soruSor = (soru) => {
    return new Promise((resolve) => rl.question(soru, resolve));
};

// 1. ÖZEL HATA YÖNETİMİ
class KuantumCokusuException extends Error {
    constructor(id) {
        super(`SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR... (Patlayan Nesne ID: ${id})`); // [cite: 44]
        this.name = "KuantumCokusuException";
    }
}

// 2. ARAYÜZ SİMÜLASYONU
// JS'de Interface yoktur, "Type Checking" için bu sınıfı miras almaları gerekecek.
// Ancak JS çoklu kalıtımı desteklemez.
// Bu yüzden "Mixin" veya "Duck Typing" kullanacağız.
// Ödevde "is/as" kontrolü istendiği için, nesnenin metodu var mı diye bakacağız veya
// manuel bir etiket sistemi kullanacağız. En akademik yöntem:
class IKritik {
    acilDurumSogutmasi() {
        throw new Error("Bu metod implement edilmeli!");
    }
}

// 3. TEMEL YAPI (ABSTRACT CLASS SİMÜLASYONU)
class KuantumNesnesi {
    constructor(id, tehlikeSeviyesi) {
        if (this.constructor === KuantumNesnesi) {
            throw new Error("Abstract sınıf nesneleştirilemez!"); // [cite: 16]
        }
        this.id = id;
        this.tehlikeSeviyesi = tehlikeSeviyesi;
        this._stabilite = 100.0;
    }

    // Kapsülleme
    get stabilite() {
        return this._stabilite;
    }

    set stabilite(value) {
        if (value <= 0) {
            this._stabilite = 0;
            throw new KuantumCokusuException(this.id); // [cite: 44]
        } else if (value > 100) {
            this._stabilite = 100;
        } else {
            this._stabilite = value;
        }
    }

    analizEt() {
        throw new Error("Bu metod override edilmeli!"); // Abstract Metot
    }

    durumBilgisi() {
        return `ID: ${this.id} | Stabilite: %${this._stabilite.toFixed(2)} | Tehlike: ${this.tehlikeSeviyesi}`;
    }
}

// 4. NESNE ÇEŞİTLERİ

class VeriPaketi extends KuantumNesnesi {
    constructor(id) {
        super(id, 1);
    }

    analizEt() {
        this.stabilite -= 5; // [cite: 32]
        console.log("Veri içeriği okundu.");
    }
}

// IKritik davranışını simüle eden sınıflar
class KaranlikMadde extends KuantumNesnesi {
    constructor(id) {
        super(id, 5);
    }

    // IKritik Implementasyonu
    acilDurumSogutmasi() {
        this.stabilite += 50; // [cite: 36]
        console.log(`${this.id} soğutuldu. Stabilite yenilendi.`);
    }

    analizEt() {
        this.stabilite -= 15; // [cite: 35]
    }
}

class AntiMadde extends KuantumNesnesi {
    constructor(id) {
        super(id, 10);
    }

    // IKritik Implementasyonu
    acilDurumSogutmasi() {
        this.stabilite += 50;
        console.log(`${this.id} üzerinde kritik soğutma yapıldı!`);
    }

    analizEt() {
        this.stabilite -= 25; // [cite: 39]
        console.log("Evrenin dokusu titriyor..."); // [cite: 40]
    }
}

// 5. MAIN DÖNGÜSÜ
async function main() {
    let envanter = [];
    let sayac = 1;
    console.log("Omega Sektörü Kuantum Veri Ambarı'na Hoşgeldiniz...");

    while (true) { // [cite: 46]
        try {
            console.log("\n--- KUANTUM AMBARI KONTROL PANELİ ---");
            console.log("1. Yeni Nesne Ekle");
            console.log("2. Tüm Envanteri Listele");
            console.log("3. Nesneyi Analiz Et");
            console.log("4. Acil Durum Soğutması Yap");
            console.log("5. Çıkış");
            
            const secim = await soruSor("Seçiminiz: "); // Async input

            if (secim === "1") {
                const tur = Math.floor(Math.random() * 3) + 1;
                const yeniId = `NESNE-${sayac++}`;
                let yeniNesne;

                if (tur === 1) yeniNesne = new VeriPaketi(yeniId);
                else if (tur === 2) yeniNesne = new KaranlikMadde(yeniId);
                else if (tur === 3) yeniNesne = new AntiMadde(yeniId);

                envanter.push(yeniNesne);
                console.log(`${yeniNesne.constructor.name} eklendi: ${yeniId}`);
            }
            else if (secim === "2") {
                console.log("\n--- ENVANTER DURUMU ---");
                envanter.forEach(nesne => {
                    console.log(nesne.durumBilgisi());
                });
            }
            else if (secim === "3") {
                const id = await soruSor("Analiz edilecek ID: ");
                const nesne = envanter.find(n => n.id === id);
                
                if (nesne) {
                    nesne.analizEt();
                    console.log(`Güncel Stabilite: ${nesne.stabilite}`);
                } else {
                    console.log("Nesne bulunamadı!");
                }
            }
            else if (secim === "4") {
                const id = await soruSor("Soğutulacak ID: ");
                const nesne = envanter.find(n => n.id === id);

                if (nesne) {
                    // Type Checking: JS'de 'interface' olmadığı için metod kontrolü veya instance kontrolü yapılır.
                    // [cite: 57] "IKritik olup olmadığı kontrolü"
                    if (nesne instanceof KaranlikMadde || nesne instanceof AntiMadde) {
                        nesne.acilDurumSogutmasi();
                    } else {
                        console.log("Bu nesne soğutulamaz!"); // [cite: 58]
                    }
                } else {
                    console.log("Nesne bulunamadı!");
                }
            }
            else if (secim === "5") {
                console.log("Çıkış yapılıyor...");
                rl.close();
                break;
            }

        } catch (e) {
            if (e.name === "KuantumCokusuException") { // [cite: 59]
                console.log("\n**************************************");
                console.log(e.message.toUpperCase());
                console.log("**************************************");
                rl.close();
                process.exit(1); // Game Over
            } else {
                console.log(`Beklenmedik Hata: ${e.message}`);
            }
        }
    }
}

main();