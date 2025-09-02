using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace sifra
{
    class Program
    {

        // pole
        struct DlouheCislo
        {
            public int[] cifra;
        }

        // vytvoreni dlouhoheho cisla
        static DlouheCislo VyrobDlouhe(int hodnota)
        {
            DlouheCislo nove;
            nove.cifra = new int[100];
            for (int i = 99; i >= 0; i--)
            {
                nove.cifra[i] = hodnota % 10;
                hodnota = hodnota / 10;
            }
            return nove;
        }

        // secteni dlouhych cisel
        static DlouheCislo SectiDlouhe(DlouheCislo prvni, DlouheCislo druhe)
        {
            DlouheCislo vysledek;
            vysledek.cifra = new int[100];
            int prevod = 0;
            for (int i = 99; i >= 0; i--)
            {
                vysledek.cifra[i] = (prvni.cifra[i] + druhe.cifra[i] + prevod) % 10;
                if ((prvni.cifra[i] + druhe.cifra[i] + prevod) >= 10) prevod = 1;
                else prevod = 0;
            }
            return vysledek;
        }

        // vynasobeni dlouheho cisla + kolikrat
        static DlouheCislo VynasobDlouhe(DlouheCislo cislo, int kolikrat)
        {
            DlouheCislo vysledek = VyrobDlouhe(0);
            for (int i = 1; i <= kolikrat; i++)
            {
                vysledek = SectiDlouhe(vysledek, cislo);
            }
            return vysledek;
        }

        // vypis dlouheho cisla
        static void VypisDlouhe(DlouheCislo co)
        {
            for (int i = 0; i < 100; i++)
            {
                Console.Write(co.cifra[i]);
            }
            Console.WriteLine();
        }

        // zakodovani
        static string Zakodovat(string text, int pocet)
        {
            char[] pole = text.ToCharArray();
            int prvnipismeno = 'a';
            int abeceda = ('a' - 'z') - 1;
            for (int i = 0; i < pole.Length; i++)
            {
                char zadano = pole[i];
                int delej = zadano - prvnipismeno;
                int proved = (delej + pocet) % abeceda;
                char znak = (char)(prvnipismeno + proved);
                pole[i] = znak;
            }
            return new string(pole);
        }

        // dekodovani
        static string Dekodovat(string zasifrovano, int pocet)
        {
            return Zakodovat(zasifrovano, pocet * -1);
        }


        static void Main(string[] args)
        {
            char c; 
            string b;
            int pocet = 0;// default prepise se od uzivatele
            // menu
            do
            {
                Console.Clear();
                Console.WriteLine("Sifra + vypocet faktorialu pro dlouha cisla");
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("Zakodovat slovo [z]");
                Console.WriteLine("Dekodovat slovo [d]");
                Console.WriteLine("Vypocet faktorialu [f]");
                Console.WriteLine("Zobrazit dekodovane slovo? [v]");
                Console.WriteLine("Ukoncit program [k]");
                c = char.ToLower(Console.ReadKey().KeyChar);
                Console.Clear();

                switch (c)
                {
                    case 'z':
                        while (true)
                        {
                            Console.WriteLine("Zadejte slovo, ktere chcete zasifrovat: ");
                            b = (Console.ReadLine()).ToLower();
                            if (Regex.IsMatch(b, @"^[a-zA-Z]+$")) // osetreni vstupu
                            {
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Zadali jste cisla nebo jste nezadal nic, prosim zadejte jen pismena");
                            }
                        }
                        while (true)
                        {
                            Console.WriteLine("Zadejte pocet posunu: ");
                            try // cte pouze cisla
                            {
                                pocet = Convert.ToInt32(Console.ReadLine());
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.Clear();
                                Console.WriteLine("Nezadal jste cislo!");
                            }
                        }
                        Console.Clear();
                        string zakoduj = Zakodovat(b, pocet); // zavolani funkce
                        Console.WriteLine(zakoduj);
                        // volba na ulozeni
                        int zapo;
                        Console.WriteLine("Chcete zasifrovane slovo ulozit do souboru?");
                        Console.WriteLine("1. Ano");
                        Console.WriteLine("2. Ne");
                        zapo = Convert.ToInt32(Console.ReadLine());
                        if (zapo == 1)
                        {
                            // připsání textu do souboru
                            using (StreamWriter sw = new StreamWriter(@"zakodovane.txt", true))
                            {
                                sw.WriteLine(zakoduj);
                                sw.Flush();
                            }
                            Console.WriteLine("Do souboru bylo připsáno.");
                            Console.ReadKey();
                        }
                        else
                        {
                        }
                        break;
                    case 'd': // stejny postup jako u predchoziho
                        Console.WriteLine("1. Chcete zadat slovo?");
                        Console.WriteLine("2. Chcete nacist zakodovane slovo?");
                        int nac;
                        nac = Convert.ToInt32(Console.ReadLine());
                        if (nac == 1)
                        {
                            Console.Clear();
                            while (true)
                            {
                                Console.WriteLine("Zadejte slovo, ktere chcete desifrovat: ");
                                b = (Console.ReadLine()).ToLower();
                                if (Regex.IsMatch(b, @"^[a-zA-Z]+$"))
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Zadali jste cisla nebo jste nezadal nic, prosim zadejte jen pismena");
                                }
                            }
                            while (true)
                            {
                                Console.WriteLine("Zadejte pocet posunu: ");
                                try
                                {
                                    pocet = Convert.ToInt32(Console.ReadLine());
                                    break;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Nezadal jste cislo");
                                }
                            }
                            string dekoduj = Dekodovat(b, pocet);
                            Console.WriteLine(dekoduj);
                            Console.ReadKey();
                            // volba na ulozeni
                            int dekk;
                            Console.WriteLine("Chcete ulozit dekodovane slovo do souboru?");
                            Console.WriteLine("1. Ano");
                            Console.WriteLine("2. Ne");
                            dekk = Convert.ToInt32(Console.ReadLine());
                            if (dekk == 1)
                            {
                                // připsání textu do souboru
                                using (StreamWriter sw = new StreamWriter(@"dekodovane.txt", true))
                                {
                                    sw.WriteLine(dekoduj);
                                    sw.Flush();
                                }
                                Console.WriteLine("Do souboru bylo připsáno.");
                                Console.ReadKey();
                            }
                        }
                        else if (nac == 2)
                        {
                            if (File.Exists("zakodovane.txt"))
                            {
                                using (StreamReader sr = new StreamReader(@"zakodovane.txt"))
                                {
                                    string s;
                                    while ((s = sr.ReadLine()) != null)
                                    {
                                        Console.WriteLine(s);
                                        Console.WriteLine("Zadejte pocet posunu: ");
                                        pocet = Convert.ToInt32(Console.ReadLine());
                                        string dekoduj = Dekodovat(s, pocet);
                                        Console.WriteLine(dekoduj);
                                        Console.ReadKey();
                                    }
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Soubor neexistuje");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nevybral jste ani jednu moznost");
                            Console.ReadKey();
                            break;
                        }
                        break;
                    case 'f':
                        Console.WriteLine("Zadejte cislo pro vypocet faktorialu: ");
                        int.TryParse(Console.ReadLine(), out int faktorial);
                        DlouheCislo vysledek = VyrobDlouhe(1);
                        for (int i = 1; i <= faktorial; i++)
                        {
                            vysledek = VynasobDlouhe(vysledek, i);
                        }
                        VypisDlouhe(vysledek);
                        Console.ReadKey();
                        break;
                    case 'v':
                        // výpis obsahu souboru
                        if (File.Exists("dekodovane.txt"))
                        {
                            Console.WriteLine("Vypisuji soubor:");
                            using (StreamReader sr = new StreamReader(@"dekodovane.txt"))
                            {
                                string s;
                                while ((s = sr.ReadLine()) != null)
                                {
                                    Console.WriteLine(s);
                                    Console.ReadKey();
                                }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Soubor neexistuje");
                            Console.ReadKey();
                        }
                        break;
                }
            } while (c != 'k');
        }
    }
}
