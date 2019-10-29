using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace AukcioProjekt_ns
{
    class Festmeny
    {
        private string cim;
        private string festo;
        private string stilus;
        private int licitekSzama;
        private int legmagasabbLicit;
        private DateTime legmagasabbLicitIdeje;
        private bool elkelt;
        public Festmeny(string cim_, string festo_, string stilus_)
        {
            cim = cim_;
            festo = festo_;
            stilus = stilus_;
            licitekSzama = 0;
            legmagasabbLicit = 0;
            legmagasabbLicitIdeje = default;
            elkelt = false;
        }
        public string getFesto()
        {
            return this.festo;
        }
        public string getCim()
        {
            return this.cim;
        }

        public string getStilus()
        {
            return this.stilus;
        }
        public int getLicitekSzama()
        {
            return this.licitekSzama;
        }
        public int getLegmagasabbLicit()
        {
            return this.legmagasabbLicit;
        }
        public bool getElkelt()
        {
            return this.elkelt;
        }

        public DateTime getLegmagasabbIdeje()
        {
            return this.legmagasabbLicitIdeje;
        }
        public int getLegmagasabbIdejeOta()
        {
            return Convert.ToInt32((DateTime.Now - this.legmagasabbLicitIdeje).Minutes);
        }

        public void setElkelt(bool elkelt_)
        {
            this.elkelt = elkelt_;
        }

        public override string ToString()
        {
            //d.) festo: cim(stilus)  elkelt / nem kelt el  legmagasabbLicit $ -legutolsoLicitIdeje(összesen: licitekSzama db)
            //            string vissza = $"{this.festo}: {this.cim} - {this.elkelt} {this.legmagasabbLicit}$ {this.legmagasabbLicitIdeje} (összesen: {this.licitekSzama}db)";
            return $"{festo}: {cim} - {elkelt} {legmagasabbLicit}$ {legmagasabbLicitIdeje} (összesen: {licitekSzama}db)";
        }
        public void licit()
        {
            if (this.elkelt)
            {
                Console.WriteLine("A festmény már elkelt!");
            }
            else
            {
                switch (this.licitekSzama)
                {
                    case 0:
                        this.licitekSzama++;
                        this.legmagasabbLicit = 100;
                        this.legmagasabbLicitIdeje = DateTime.Now;
                        break;
                    default:
                        this.licit(10);
                        break;
                }
            }

        }


        public void licit(int mertek)
        {
            switch (this.licitekSzama)
            {
                case 0:
                    this.licit();
                    break;
                default:
                    this.licitekSzama++;
                    double szorzo = (1 + Math.Round(Convert.ToDouble(mertek) / 100,2));
                    this.legmagasabbLicit = kerekit(this.legmagasabbLicit * szorzo);
                    this.legmagasabbLicitIdeje = DateTime.Now;
                    break;
            }
        }
        public int kerekit(double alap_double)
        {
            string alap = Convert.ToString(Math.Round(alap_double,0));
            char[]  kerekitett_char = Convert.ToString(Math.Round(alap_double, 0)).ToCharArray(0, alap.Length);

            for (int i = 2; i < kerekitett_char.Length; i++)
            {
                kerekitett_char[i] = '0';
            }
            string kerekitett_str = new string(kerekitett_char);
            return Convert.ToInt32(kerekitett_str);
        }

    }

    class Program
    {
        static void beolvas()
        {
            StreamReader sr = new StreamReader(fajl);
            while (!sr.EndOfStream)
            {
                string[] egy_sor = sr.ReadLine().Split(';');
                festmenyek.Add(new Festmeny(egy_sor[1], egy_sor[0], egy_sor[2]));
            }
            sr.Close();
        }

        static void felhasznalo_feltolt()
        {
            int db = 0;
            string cim, festo, stilus;
            Console.Write("Hány festményt szeretne felvinni?:");
            db = Convert.ToInt32(Console.ReadLine());
            if (db > 0)
            {

                for (int i = 1; i <= db; i++)
                {
                    Console.Write($"{i}. kép címe?:");
                    cim = Console.ReadLine();
                    Console.Write($"Festője?:");
                    festo = Console.ReadLine();
                    Console.Write($"Kép stílusa?:");
                    stilus = Console.ReadLine();
                    festmenyek.Add(new Festmeny(cim, festo, stilus));
                }
            }
        }

        static void osszes_kiir()
        {
            int i = 0;
            foreach (var item in festmenyek)
            {
                i++;
                Console.WriteLine($"{i}: {item.ToString()}");
//              Console.WriteLine(item.ToString());
            }
        }


        static void veletlen_licit(int licitek_szama)
        {
            Console.WriteLine($"És most {licitek_szama} véletlen licit következik:");
            Console.WriteLine("-------------------------------------");
            for (int i = 1; i <= licitek_szama; i++)
            {
                Random licit_rnd = new Random();

                int licit_index = licit_rnd.Next(0, festmenyek.Count());
                int licit_szazalek = licit_rnd.Next(1, 101);
                
                festmenyek[licit_index].licit(licit_szazalek);
                
                Console.WriteLine($"{i}. festmény ({festmenyek[licit_index].getCim()}): {licit_szazalek}%");
                
                System.Threading.Thread.Sleep(300);
            }
        }
        /*
         * d.) A felhasználó konzolon is licitálhasson a festményekre.
         * 
         * •A felhasználó először a festmény sorszámát adja meg. A sorszám megadásánál használjon index eltolást.
         * (Ha a felhasználó 3-at ad meg akkor a lista 2. elemére licitál). 0 megadásával lépjen ki a felhasználó a licitálásból.
         * •Nem szám beírása esetén a program hibaüzenetet írjon ki majd álljon le.
         * •Nem létező sorszám esetén a program hibaüzenetet írjon ki, majd kérjen beúj sorszámot.
         * •Ha a festmény elkelt, akkor hibaüzenetet írjon ki, majd kérjen be új sorszámot.
         * 
         * •A sorszám megadása után kérje be, hogy milyen mértékkel szeretne licitálni a felhasználó.
         * •A megadás ne legyen kötelező, ha a felhasználó egyből entert üt le akkor az alap 10%-os licittel lehessen licitálni.
         * •Ha a felhasználó nem számot ad meg akkor a program hibaüzenettel álljon le.
         * •A sorszám megadása után, ha az adott festményre több mint 2 perce érkezett utoljára licit akkor állítsa be elkeltre,
         * majd hibaüzenetet írjon ki, majd kérjen be új sorszámot
         * •Miután a felhasználó befejezte a licitálást, az összes olyan festmény, amire érkezett licit legyen elkelt.
         */
        static bool felhasznalo_licit()
        {
            int ssz = -1;
            bool jo_ssz;
            do
            {
                jo_ssz = true;
                Console.Write("Kép sorszama??:");
                try
                {
                    ssz = int.Parse(Console.ReadLine());
                }
                catch (FormatException hiba)
                {
                   Console.WriteLine(hiba.Message);
                    Environment.Exit(0);

                }
                if (ssz == 0)
                {
                    break; ;
                }

                if ((ssz > festmenyek.Count()) && (ssz != -1))
                {
                    Console.WriteLine($"Nincs is ennyi festmeny! Max. sorszám: {festmenyek.Count()}");
                    jo_ssz = false;
                }

                if (jo_ssz)
                {
                    if ((festmenyek[ssz-1].getLicitekSzama() > 0) && ((DateTime.Now - festmenyek[ssz - 1].getLegmagasabbIdeje()).Minutes >= 1))
                    {
                        festmenyek[ssz - 1].setElkelt(true);
                        Console.WriteLine($"A {ssz} sorszámú festmény már elkelt");
                        jo_ssz = false;
                    }
                }

                if (jo_ssz)
                {
                    if (festmenyek[ssz - 1].getElkelt())
                    {
                        Console.WriteLine($"A {ssz} sorszámú festmény már elkelt");
                        jo_ssz = false;
                    }
                }

            }

            while (!jo_ssz);

            if (ssz == 0)
            {
                Console.WriteLine("Felhasználói licit vége!");
                Console.ReadKey();
                return false;
            }
            else
            {
                int licit = 0;
                string licit_str;

                Console.Write("Kérem a licit mértékét:");
                licit_str = Console.ReadLine();
                if (licit_str.Length == 0)
                {
                    licit = 0;
                }
                else
                {
                    try
                    {
                        licit = int.Parse(licit_str);
                    }
                    catch (FormatException hiba)
                    {
                        Console.WriteLine(hiba.Message);
                        Environment.Exit(0);
                    }
                }

                if (licit == 0)
                {
                    Console.WriteLine($"Érvényes 10%-os licit a(z) {ssz} sorszámú képre!");
                    festmenyek[ssz - 1].licit();
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine($"Érvényes {licit}%-os licit a(z) {ssz} sorszámú képre!");
                    festmenyek[ssz - 1].licit(licit);
                    Console.ReadKey();
                }
                return true;
            }
        }


        static void Legdragabb()
        {
            int max_licit = 0;
            int max_licit_index = 0;
            for (int i = 0; i<festmenyek.Count() ; i++)
            {
                if (max_licit<=festmenyek[i].getLegmagasabbLicit())
                {
                    max_licit = festmenyek[i].getLegmagasabbLicit();
                    max_licit_index = i;
                }
            }

            Console.WriteLine(festmenyek[max_licit_index].ToString());

        }

        static void elkeltek()
        {

            foreach (var elem in festmenyek)
            {
                if (elem.getLicitekSzama() > 0)
                {
                    elem.setElkelt(true);
                    Console.WriteLine(elem.ToString());
                }
            }

        }

        static bool vantiz()
        {
            foreach (var item in festmenyek)
            {
                if (item.getLicitekSzama() >= 10)
                {
                    return true;
                }
            }
            return false;
        }

        static List<Festmeny> festmenyek = new List<Festmeny>();
        static string fajl = @"f:\suli\szoftverfejleszto\c_sharp\AukcioProjekt\festmenyek.csv";
        static string fajl_out = @"f:\suli\szoftverfejleszto\c_sharp\AukcioProjekt\festmenyek_rendezett.csv";
        static void Main(string[] args)
        {


            festmenyek.Add(new Festmeny("Hómező Bálint Ferenccel", "Tóth Péter", "Neoprimitív"));
            festmenyek.Add(new Festmeny("Hómező Bálint Ferenc nélkül", "Tóth Péter", "Neoprimitív"));
            
            Console.WriteLine("2 festmény hozzáadva:");
            Console.WriteLine("---------------------");
            osszes_kiir();
            Console.ReadKey();

            Console.Clear();
            Console.WriteLine("Felhasználói felvitel:");
            Console.WriteLine("----------------------");
            felhasznalo_feltolt();

            Console.Clear();
            Console.WriteLine($"Feltöltöm a képeket {fajl} fájlból!");
            Console.ReadKey();
            beolvas();
            Console.WriteLine("Feltöltés megtörtént!");
            Console.ReadKey();

            Console.Clear();
            Console.WriteLine("Az adatbázisban lévő festmények:");
            Console.WriteLine("--------------------------------");
            osszes_kiir();
            Console.ReadKey();

            Console.Clear();
            veletlen_licit(20);
            Console.ReadKey();


            Console.Clear();
            Console.WriteLine("Az adatbázisban lévő festmények:");
            Console.WriteLine("--------------------------------");
            osszes_kiir();
            Console.ReadKey();
            Console.Clear();

            do
            {
                Console.Clear();
                Console.WriteLine("Felhasználói licit:");
                Console.WriteLine("Szabályok: - licit a kép sorszámával, kilépés:0!");
                Console.WriteLine("--------------------------------");

                osszes_kiir();
            }
            while (felhasznalo_licit());

            /*a.) Keresd meg a legdrágábban elkelt festményt, majd az adatait konzolra.
            b.) Döntsd el, hogy van-e olyan festmény, amelyre 10-nél több alkalommal
            licitáltak.
            c.) Számold meg, hogy hány olyan festmény van, amely nem kelt el.
            d.) Rendezd át a listát a Legmagasabb Licit szerint csökkenő sorrendben,
            majd írd ki újra a festményeket.
            +.) A rendezett lista tartalmát írd ki egy festmenyek_rendezett.csv nevű
            fájlba.*/

            Console.Clear();
            Console.WriteLine("Elkelt festmények:");
            Console.WriteLine("------------------");
            elkeltek();
            Console.ReadKey();

            Console.WriteLine("\nA legmagasabb licittel rendelkező kép:");
            Console.WriteLine("--------------------------------------");
            Legdragabb();
            Console.ReadKey();

            Console.WriteLine();
            if (vantiz())
            {
                Console.WriteLine("Van 10 vagy több licit!");
            }
            else
            {
                Console.WriteLine("Nincs 10 vagy több licit!");
            }
            Console.ReadKey();
            Console.WriteLine($"\nEladatlan festmények száma: {festmenyek.FindAll(a => !a.getElkelt()).Count()} db");
            Console.ReadKey();
            Console.Clear();

            festmenyek = festmenyek.OrderByDescending(x => x.getLegmagasabbLicit()).ToList();
            Console.WriteLine("Rendezett lista:");
            Console.WriteLine("----------------");
            osszes_kiir();

            StreamWriter sw = new StreamWriter(fajl_out);
            foreach (var item in festmenyek)
            {
                sw.WriteLine(item.ToString());
            }
            sw.Close();
            Console.WriteLine($"\nRendezett lista kiirva a {fajl_out} fájlba");
            Console.ReadKey();
            Console.WriteLine("Program vége");
            Console.ReadKey();
        }
    }
}
