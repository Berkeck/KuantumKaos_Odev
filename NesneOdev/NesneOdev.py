import random
from abc import ABC, abstractmethod

# 1. ÖZEL HATA YÖNETİMİ
class KuantumCokusuException(Exception):
    def __init__(self, nesne_id):
        # Hata mesajı formatı
        super().__init__(f"SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR... (Patlayan Nesne ID: {nesne_id})")

# 2. ARAYÜZ (INTERFACE) - Python'da Abstract Class olarak tanımlanır
class IKritik(ABC):
    @abstractmethod
    def acil_durum_sogutmasi(self): #
        pass

# 3. TEMEL YAPI (ABSTRACT CLASS)
class KuantumNesnesi(ABC):
    def __init__(self, nesne_id, tehlike_seviyesi):
        self.id = nesne_id #
        self.tehlike_seviyesi = tehlike_seviyesi #
        self._stabilite = 100.0 # Varsayılan başlangıç

    # Kapsülleme (Encapsulation) - Property Decorator
    @property
    def stabilite(self):
        return self._stabilite

    @stabilite.setter
    def stabilite(self, value):
        # 0-100 kontrolü
        if value <= 0:
            self._stabilite = 0
            # Hata fırlatma
            raise KuantumCokusuException(self.id)
        elif value > 100:
            self._stabilite = 100.0
        else:
            self._stabilite = value

    @abstractmethod
    def analiz_et(self): #
        pass

    def durum_bilgisi(self): #
        return f"ID: {self.id} | Stabilite: %{self.stabilite:.2f} | Tehlike: {self.tehlike_seviyesi}"

# 4. NESNE ÇEŞİTLERİ (INHERITANCE)

# A. VeriPaketi
class VeriPaketi(KuantumNesnesi):
    def __init__(self, nesne_id):
        super().__init__(nesne_id, 1)

    def analiz_et(self):
        self.stabilite -= 5 #
        print("Veri içeriği okundu.")

# B. KaranlikMadde - Hem KuantumNesnesi hem IKritik (Çoklu Kalıtım)
class KaranlikMadde(KuantumNesnesi, IKritik):
    def __init__(self, nesne_id):
        super().__init__(nesne_id, 5)

    def analiz_et(self):
        self.stabilite -= 15 #

    def acil_durum_sogutmasi(self):
        self.stabilite += 50 #
        print(f"{self.id} soğutuldu. Stabilite yenilendi.")

# C. AntiMadde
class AntiMadde(KuantumNesnesi, IKritik):
    def __init__(self, nesne_id):
        super().__init__(nesne_id, 10)

    def analiz_et(self):
        self.stabilite -= 25 #
        print("Evrenin dokusu titriyor...") #

    def acil_durum_sogutmasi(self):
        self.stabilite += 50
        print(f"{self.id} üzerinde kritik soğutma yapıldı!")

# 5. OYNANIŞ DÖNGÜSÜ (MAIN)
def main():
    envanter = [] # Generic List karşılığı
    sayac = 1
    
    print("Omega Sektörü Kuantum Veri Ambarı'na Hoşgeldiniz...")

    while True: # Sonsuz döngü
        try:
            print("\n--- KUANTUM AMBARI KONTROL PANELİ ---")
            print("1. Yeni Nesne Ekle")
            print("2. Tüm Envanteri Listele")
            print("3. Nesneyi Analiz Et")
            print("4. Acil Durum Soğutması Yap")
            print("5. Çıkış")
            secim = input("Seçiminiz: ")

            if secim == "1":
                tur = random.randint(1, 3)
                yeni_id = f"NESNE-{sayac}"
                sayac += 1
                yeni_nesne = None

                if tur == 1: yeni_nesne = VeriPaketi(yeni_id)
                elif tur == 2: yeni_nesne = KaranlikMadde(yeni_id)
                elif tur == 3: yeni_nesne = AntiMadde(yeni_id)
                
                envanter.append(yeni_nesne)
                print(f"{type(yeni_nesne).__name__} eklendi: {yeni_id}")

            elif secim == "2":
                print("\n--- ENVANTER DURUMU ---")
                for nesne in envanter:
                    print(nesne.durum_bilgisi()) # Polimorfizm

            elif secim == "3":
                inp_id = input("Analiz edilecek ID: ")
                # List comprehension ile arama
                bulunan = next((x for x in envanter if x.id == inp_id), None)
                if bulunan:
                    bulunan.analiz_et()
                    print(f"Güncel Stabilite: {bulunan.stabilite}")
                else:
                    print("Nesne bulunamadı!")

            elif secim == "4":
                inp_id = input("Soğutulacak ID: ")
                bulunan = next((x for x in envanter if x.id == inp_id), None)
                if bulunan:
                    # Type Checking (isinstance)
                    if isinstance(bulunan, IKritik):
                        bulunan.acil_durum_sogutmasi()
                    else:
                        print("Bu nesne soğutulamaz!") #
                else:
                    print("Nesne bulunamadı!")

            elif secim == "5":
                print("Çıkış yapılıyor...")
                break

        except KuantumCokusuException as ex: #
            print("\n**************************************")
            print(str(ex).upper())
            print("**************************************")
            break # Game over
        except Exception as ex:
            print(f"Hata: {ex}")

if __name__ == "__main__":
    main()