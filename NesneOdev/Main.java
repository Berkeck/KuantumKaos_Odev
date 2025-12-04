import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.Random;

// 1. ÖZEL HATA YÖNETİMİ [cite: 41, 43]
class KuantumCokusuException extends RuntimeException {
    public KuantumCokusuException(String id) {
        super("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR... (Patlayan Nesne ID: " + id + ")");
    }
}

// 2. ARAYÜZ (INTERFACE) [cite: 25]
interface IKritik {
    void acilDurumSogutmasi(); // [cite: 26]
}

// 3. TEMEL YAPI (ABSTRACT CLASS) [cite: 15, 16]
abstract class KuantumNesnesi {
    private String id;
    private double stabilite;
    private int tehlikeSeviyesi;

    public KuantumNesnesi(String id, int tehlikeSeviyesi) {
        this.id = id;
        this.tehlikeSeviyesi = tehlikeSeviyesi;
        this.stabilite = 100.0; // Varsayılan başlangıç
    }

    public String getId() { return id; }
    
    public double getStabilite() { return stabilite; }
    
    // Kapsülleme ve Hata Fırlatma [cite: 19, 44]
    public void setStabilite(double stabilite) {
        if (stabilite <= 0) {
            this.stabilite = 0;
            throw new KuantumCokusuException(this.id);
        } else if (stabilite > 100) {
            this.stabilite = 100;
        } else {
            this.stabilite = stabilite;
        }
    }

    public int getTehlikeSeviyesi() { return tehlikeSeviyesi; }

    // Abstract Metot [cite: 22]
    public abstract void analizEt();

    // Durum Bilgisi [cite: 23]
    public String durumBilgisi() {
        return "ID: " + id + " | Stabilite: %" + String.format("%.2f", stabilite) + " | Tehlike: " + tehlikeSeviyesi;
    }
}

// 4. NESNE ÇEŞİTLERİ

// A. VeriPaketi [cite: 30]
class VeriPaketi extends KuantumNesnesi {
    public VeriPaketi(String id) {
        super(id, 1);
    }

    @Override
    public void analizEt() {
        // Stabiliteyi getter/setter ile yönetiyoruz
        setStabilite(getStabilite() - 5); // [cite: 32]
        System.out.println("Veri içeriği okundu.");
    }
}

// B. KaranlikMadde [cite: 33]
class KaranlikMadde extends KuantumNesnesi implements IKritik {
    public KaranlikMadde(String id) {
        super(id, 5);
    }

    @Override
    public void analizEt() {
        setStabilite(getStabilite() - 15); // [cite: 35]
    }

    @Override
    public void acilDurumSogutmasi() {
        setStabilite(getStabilite() + 50); // [cite: 36]
        System.out.println(getId() + " soğutuldu. Stabilite yenilendi.");
    }
}

// C. AntiMadde [cite: 37]
class AntiMadde extends KuantumNesnesi implements IKritik {
    public AntiMadde(String id) {
        super(id, 10);
    }

    @Override
    public void analizEt() {
        setStabilite(getStabilite() - 25); // [cite: 39]
        System.out.println("Evrenin dokusu titriyor..."); // [cite: 40]
    }

    @Override
    public void acilDurumSogutmasi() {
        setStabilite(getStabilite() + 50);
        System.out.println(getId() + " üzerinde kritik soğutma yapıldı!");
    }
}

// 5. MAIN SINIFI
public class Main {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        List<KuantumNesnesi> envanter = new ArrayList<>(); // [cite: 55]
        Random rnd = new Random();
        int sayac = 1;

        System.out.println("Omega Sektörü Kuantum Veri Ambarı'na Hoşgeldiniz...");

        // Sonsuz Döngü [cite: 46]
        while (true) {
            try {
                System.out.println("\n--- KUANTUM AMBARI KONTROL PANELİ ---");
                System.out.println("1. Yeni Nesne Ekle");
                System.out.println("2. Tüm Envanteri Listele");
                System.out.println("3. Nesneyi Analiz Et");
                System.out.println("4. Acil Durum Soğutması Yap");
                System.out.println("5. Çıkış");
                System.out.print("Seçiminiz: ");

                String secim = scanner.nextLine();

                if (secim.equals("1")) {
                    int tur = rnd.nextInt(3) + 1; // 1-3
                    String yeniId = "NESNE-" + sayac++;
                    KuantumNesnesi yeniNesne = null;

                    switch (tur) {
                        case 1: yeniNesne = new VeriPaketi(yeniId); break;
                        case 2: yeniNesne = new KaranlikMadde(yeniId); break;
                        case 3: yeniNesne = new AntiMadde(yeniId); break;
                    }
                    envanter.add(yeniNesne);
                    System.out.println(yeniNesne.getClass().getSimpleName() + " eklendi: " + yeniId);
                } 
                else if (secim.equals("2")) {
                    System.out.println("\n--- ENVANTER DURUMU ---");
                    for (KuantumNesnesi nesne : envanter) {
                        System.out.println(nesne.durumBilgisi()); // Polimorfizm [cite: 56]
                    }
                } 
                else if (secim.equals("3")) {
                    System.out.print("Analiz edilecek ID: ");
                    String id = scanner.nextLine();
                    boolean bulundu = false;
                    
                    for (KuantumNesnesi nesne : envanter) {
                        if (nesne.getId().equals(id)) {
                            nesne.analizEt();
                            System.out.println("Güncel Stabilite: " + nesne.getStabilite());
                            bulundu = true;
                            break;
                        }
                    }
                    if (!bulundu) System.out.println("Nesne bulunamadı!");
                } 
                else if (secim.equals("4")) {
                    System.out.print("Soğutulacak ID: ");
                    String id = scanner.nextLine();
                    boolean bulundu = false;

                    for (KuantumNesnesi nesne : envanter) {
                        if (nesne.getId().equals(id)) {
                            // Type Checking [cite: 57]
                            if (nesne instanceof IKritik) {
                                ((IKritik) nesne).acilDurumSogutmasi();
                            } else {
                                System.out.println("Bu nesne soğutulamaz!"); // [cite: 58]
                            }
                            bulundu = true;
                            break;
                        }
                    }
                    if (!bulundu) System.out.println("Nesne bulunamadı!");
                } 
                else if (secim.equals("5")) {
                    System.out.println("Çıkış yapılıyor...");
                    break;
                }
            } catch (KuantumCokusuException ex) {
                // Game Over [cite: 59]
                System.out.println("\n**************************************");
                System.out.println(ex.getMessage().toUpperCase());
                System.out.println("**************************************");
                System.exit(0); // Programı sonlandır
            } catch (Exception ex) {
                System.out.println("Hata: " + ex.getMessage());
            }
        }
    }
}