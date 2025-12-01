using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    using System;
    using System.Collections.Generic;

    namespace KuantumKaos
    {
        //  Exception
        public class KuantumCokusuException : Exception
        {
            public string NesneId { get; }

            public KuantumCokusuException(string nesneId)
                : base($"Kuantum çöküşü! Patlayan nesne ID: {nesneId}")
            {
                NesneId = nesneId;
            }
        }

        // Abstract base class
        public abstract class KuantumNesnesi
        {
            private double stabilite;
            private int tehlikeSeviyesi;

            public string ID { get; }

            public double Stabilite
            {
                get => stabilite;
                protected set
                {
                    if (value > 100) value = 100;
                    if (value <= 0)
                    {
                        stabilite = 0;
                        throw new KuantumCokusuException(ID);
                    }
                    stabilite = value;
                }
            }

            public int TehlikeSeviyesi
            {
                get => tehlikeSeviyesi;
                protected set
                {
                    if (value < 1) value = 1;
                    if (value > 10) value = 10;
                    tehlikeSeviyesi = value;
                }
            }

            protected KuantumNesnesi(string id, double stabilite, int tehlikeSeviyesi)
            {
                ID = id;
                this.stabilite = 100;
                this.tehlikeSeviyesi = 1;

                Stabilite = stabilite;
                TehlikeSeviyesi = tehlikeSeviyesi;
            }

            protected void DegistirStabilite(double delta)
            {
                Stabilite = Stabilite + delta;
            }

            public abstract void AnalizEt();

            public virtual string DurumBilgisi()
            {
                return $"ID: {ID}, Stabilite: {Stabilite:F2}, Tehlike: {TehlikeSeviyesi}";
            }

            protected void GuvenliSogutma(double miktar)
            {
                double yeni = Stabilite + miktar;
                if (yeni > 100) yeni = 100;
                Stabilite = yeni;
            }
        }

        // Interface
        public interface IKritik
        {
            void AcilDurumSogutmasi();
        }

        // VeriPaketi
        public class VeriPaketi : KuantumNesnesi
        {
            public VeriPaketi(string id)
                : base(id, 100, 1)
            {
            }

            public override void AnalizEt()
            {
                Console.WriteLine("Veri içeriği okundu.");
                DegistirStabilite(-5);
            }
        }

        // KaranlikMadde
        public class KaranlikMadde : KuantumNesnesi, IKritik
        {
            public KaranlikMadde(string id)
                : base(id, 100, 7)
            {
            }

            public override void AnalizEt()
            {
                Console.WriteLine("Karanlık madde analiz ediliyor...");
                DegistirStabilite(-15);
            }

            public void AcilDurumSogutmasi()
            {
                Console.WriteLine("Karanlık madde soğutma işlemi uygulanıyor...");
                GuvenliSogutma(50);
            }
        }

        // AntiMadde
        public class AntiMadde : KuantumNesnesi, IKritik
        {
            public AntiMadde(string id)
                : base(id, 100, 10)
            {
            }

            public override void AnalizEt()
            {
                Console.WriteLine("Evrenin dokusu titriyor...");
                DegistirStabilite(-25);
            }

            public void AcilDurumSogutmasi()
            {
                Console.WriteLine("Antimadde acil soğutma devrede...");
                GuvenliSogutma(50);
            }
        }

        internal class Program
        {
            static void Main(string[] args)
            {
                List<KuantumNesnesi> envanter = new List<KuantumNesnesi>();
                Random random = new Random();

                while (true)
                {
                    Console.WriteLine("\nKUANTUM AMBARI KONTROL PANELİ");
                    Console.WriteLine("1. Yeni Nesne Ekle (Rastgele)");
                    Console.WriteLine("2. Tüm Envanteri Listele");
                    Console.WriteLine("3. Nesneyi Analiz Et (ID ile)");
                    Console.WriteLine("4. Acil Durum Soğutması Yap");
                    Console.WriteLine("5. Çıkış");
                    Console.Write("Seçiminiz: ");
                    string secim = Console.ReadLine();

                    try
                    {
                        switch (secim)
                        {
                            case "1":
                                KuantumNesnesi yeni = RastgeleNesneUret(random, envanter.Count + 1);
                                envanter.Add(yeni);
                                Console.WriteLine($"Yeni nesne eklendi: {yeni.DurumBilgisi()}");
                                break;

                            case "2":
                                if (envanter.Count == 0)
                                {
                                    Console.WriteLine("Envanter boş.");
                                }
                                else
                                {
                                    foreach (var nesne in envanter)
                                    {
                                        Console.WriteLine(nesne.DurumBilgisi());
                                    }
                                }
                                break;

                            case "3":
                                Console.Write("Analiz edilecek nesnenin ID'si: ");
                                string analizId = Console.ReadLine();
                                var analizNesne = envanter.Find(n => n.ID == analizId);
                                if (analizNesne == null)
                                {
                                    Console.WriteLine("Bu ID ile nesne bulunamadı.");
                                }
                                else
                                {
                                    analizNesne.AnalizEt();
                                    Console.WriteLine("Analiz tamamlandı.");
                                }
                                break;

                            case "4":
                                Console.Write("Soğutma uygulanacak nesnenin ID'si: ");
                                string sogutId = Console.ReadLine();
                                var sogutNesne = envanter.Find(n => n.ID == sogutId);
                                if (sogutNesne == null)
                                {
                                    Console.WriteLine("Bu ID ile nesne bulunamadı.");
                                }
                                else if (sogutNesne is IKritik kritikNesne)
                                {
                                    kritikNesne.AcilDurumSogutmasi();
                                    Console.WriteLine("Soğutma işlemi tamamlandı.");
                                }
                                else
                                {
                                    Console.WriteLine("Bu nesne soğutulamaz!");
                                }
                                break;

                            case "5":
                                Console.WriteLine("Programdan çıkılıyor...");
                                return;

                            default:
                                Console.WriteLine("Geçersiz seçim.");
                                break;
                        }
                    }
                    catch (KuantumCokusuException ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
            }

            static KuantumNesnesi RastgeleNesneUret(Random random, int sayac)
            {
                int tip = random.Next(0, 3);
                string id = $"N{sayac:000}";

                if (tip == 0)
                    return new VeriPaketi(id);
                else if (tip == 1)
                    return new KaranlikMadde(id);
                else
                    return new AntiMadde(id);
            }
        }
    }

}
