from abc import ABC, abstractmethod
import random

class KuantumCokusuException(Exception):
    pass

class KuantumNesnesi(ABC):
    def __init__(self, id):
        self.id = id
        self.stabilite = 100

    def azalt(self, miktar):
        self.stabilite -= miktar
        if self.stabilite <= 0:
            raise KuantumCokusuException(f"Kuantum çöküşü! Patlayan: {self.id}")
        if self.stabilite > 100:
            self.stabilite = 100

    @abstractmethod
    def analiz_et(self):
        pass

    def durum(self):
        return f"ID: {self.id}, Stabilite: {self.stabilite}"

class IKritik:
    def sogut(self):
        pass

class VeriPaketi(KuantumNesnesi):
    def analiz_et(self):
        print("Veri okundu.")
        self.azalt(5)

class KaranlikMadde(KuantumNesnesi, IKritik):
    def analiz_et(self):
        self.azalt(15)

    def sogut(self):
        self.stabilite = min(self.stabilite + 50, 100)

class AntiMadde(KuantumNesnesi, IKritik):
    def analiz_et(self):
        print("Evren titriyor...")
        self.azalt(25)

    def sogut(self):
        self.stabilite = min(self.stabilite + 50, 100)

def rastgele(id):
    t = random.randint(0,2)
    if t == 0: return VeriPaketi(id)
    if t == 1: return KaranlikMadde(id)
    return AntiMadde(id)

listem = []
sayac = 1

while True:
    print("\n1-Yeni 2-Liste 3-Analiz 4-Soğut 5-Çıkış")
    sec = input("Seçim: ")

    try:
        if sec == "1":
            n = rastgele(f"N{sayac}")
            sayac += 1
            listem.append(n)
            print(n.durum())

        elif sec == "2":
            for n in listem:
                print(n.durum())

        elif sec == "3":
            idd = input("ID: ")
            for n in listem:
                if n.id == idd:
                    n.analiz_et()

        elif sec == "4":
            idd = input("ID: ")
            for n in listem:
                if n.id == idd and isinstance(n, IKritik):
                    n.sogut()
                else:
                    print("Bu nesne soğutulamaz.")

        elif sec == "5":
            break

    except KuantumCokusuException as ex:
        print("SİSTEM ÇÖKTÜ!")
        print(ex)
        break
