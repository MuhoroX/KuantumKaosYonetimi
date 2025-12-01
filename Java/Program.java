import java.util.*;

class KuantumCokusuException extends RuntimeException {
    public KuantumCokusuException(String id) {
        super("Kuantum çöküşü! Patlayan nesne: " + id);
    }
}

abstract class KuantumNesnesi {
    protected double stabilite = 100;
    protected String id;

    public KuantumNesnesi(String id) { this.id = id; }

    protected void azalt(double miktar) {
        stabilite -= miktar;
        if (stabilite <= 0) throw new KuantumCokusuException(id);
        if (stabilite > 100) stabilite = 100;
    }

    public abstract void analizEt();

    public String durum() {
        return "ID: " + id + ", Stabilite: " + stabilite;
    }
}

interface IKritik {
    void sogut();
}

class VeriPaketi extends KuantumNesnesi {
    public VeriPaketi(String id) { super(id); }

    public void analizEt() {
        System.out.println("Veri okundu.");
        azalt(5);
    }
}

class KaranlikMadde extends KuantumNesnesi implements IKritik {
    public KaranlikMadde(String id) { super(id); }

    public void analizEt() { azalt(15); }
    public void sogut() { stabilite = Math.min(stabilite + 50, 100); }
}

class AntiMadde extends KuantumNesnesi implements IKritik {
    public AntiMadde(String id) { super(id); }

    public void analizEt() {
        System.out.println("Evren titriyor...");
        azalt(25);
    }

    public void sogut() { stabilite = Math.min(stabilite + 50, 100); }
}

public class Program {
    public static void main(String[] args) {
        Scanner s = new Scanner(System.in);
        Random r = new Random();
        ArrayList<KuantumNesnesi> list = new ArrayList<>();
        int sayac = 1;

        while (true) {
            System.out.println("\n1-Yeni 2-Liste 3-Analiz 4-Soğut 5-Çıkış");
            String sec = s.nextLine();

            try {
                if (sec.equals("1")) {
                    String id = "N" + sayac++;
                    int t = r.nextInt(3);

                    KuantumNesnesi n = (t == 0)
                            ? new VeriPaketi(id)
                            : (t == 1 ? new KaranlikMadde(id) : new AntiMadde(id));

                    list.add(n);
                    System.out.println(n.durum());
                }
                else if (sec.equals("2")) {
                    list.forEach(n -> System.out.println(n.durum()));
                }
                else if (sec.equals("3")) {
                    System.out.print("ID: ");
                    String id = s.nextLine();
                    for (var n : list)
                        if (n.id.equals(id)) n.analizEt();
                }
                else if (sec.equals("4")) {
                    System.out.print("ID: ");
                    String id = s.nextLine();
                    for (var n : list)
                        if (n.id.equals(id) && n instanceof IKritik k) k.sogut();
                }
                else if (sec.equals("5")) break;
            }
            catch (KuantumCokusuException ex) {
                System.out.println("SİSTEM ÇÖKTÜ!");
                System.out.println(ex.getMessage());
                break;
            }
        }
    }
}
