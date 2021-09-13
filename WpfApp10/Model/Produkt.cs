using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApp10.Model
{
    class Produkt
    {

        public string Nazwa { get; set; }
        public string Rodzaj { get; set; }
        public double Cena { get; set; }
        public double Ilosc { get; set; }
        public static double Suma;
        public string miara
        {
            get { return miara; }
            set { miara = value; }
        }
        public static double GetSuma()
        {
            return Math.Round(Suma, 2);
        }
        public static void SetSuma(double liczba, double liczba2)
        {
            Suma += liczba * liczba2;
        }
        public static void SetSuma(int liczba)
        {
            Suma = liczba;
        }
        public Produkt(string nazwa, string rodzaj, double cena, double ilosc)
        {
            Nazwa = nazwa;
            Rodzaj = rodzaj;
            Cena = cena;
            Ilosc = ilosc;
        }


        public override string ToString()
        {
            return $"{Nazwa} {Ilosc} {Rodzaj} {Math.Round(Cena * Ilosc, 2)} zlotych";
        }


    }
}
