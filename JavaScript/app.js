const readline = require("readline");

class KuantumCokusuException extends Error {
    constructor(id) {
        super("Kuantum çöküşü! Patlayan: " + id);
    }
}

class KuantumNesnesi {
    constructor(id) {
        this.id = id;
        this.stabilite = 100;
    }

    azalt(m) {
        this.stabilite -= m;
        if (this.stabilite <= 0) throw new KuantumCokusuException(this.id);
        if (this.stabilite > 100) this.stabilite = 100;
    }

    analizEt() {}
    durum() { return `ID: ${this.id}, Stabilite: ${this.stabilite}`; }
}

class VeriPaketi extends KuantumNesnesi {
    analizEt() {
        console.log("Veri okundu.");
        this.azalt(5);
    }
}

class KaranlikMadde extends KuantumNesnesi {
    analizEt() { this.azalt(15); }
    sogut() { this.stabilite = Math.min(this.stabilite + 50, 100); }
}

class AntiMadde extends KuantumNesnesi {
    analizEt() {
        console.log("Evren titriyor...");
        this.azalt(25);
    }
    sogut() { this.stabilite = Math.min(this.stabilite + 50, 100); }
}

function rastgele(id) {
    let t = Math.floor(Math.random() * 3);
    if (t === 0) return new VeriPaketi(id);
    if (t === 1) return new KaranlikMadde(id);
    return new AntiMadde(id);
}

const rl = readline.createInterface({ input: process.stdin, output: process.stdout });

function soru(s) {
    return new Promise(r => rl.question(s, r));
}

let list = [];
let sayac = 1;

async function main() {
    while (true) {
        console.log("\n1-Yeni 2-Liste 3-Analiz 4-Soğut 5-Çıkış");
        let sec = await soru("Seçim: ");

        try {
            if (sec === "1") {
                let n = rastgele("N" + sayac++);
                list.push(n);
                console.log(n.durum());
            }
            else if (sec === "2") {
                list.forEach(x => console.log(x.durum()));
            }
            else if (sec === "3") {
                let id = await soru("ID: ");
                let n = list.find(x => x.id === id);
                if (n) n.analizEt();
            }
            else if (sec === "4") {
                let id = await soru("ID: ");
                let n = list.find(x => x.id === id);
                if (n && typeof n.sogut === "function") n.sogut();
                else console.log("Bu nesne soğutulamaz.");
            }
            else if (sec === "5") break;
        }
        catch (e) {
            console.log("SİSTEM ÇÖKTÜ!");
            console.log(e.message);
            break;
        }
    }
    rl.close();
}

main();
