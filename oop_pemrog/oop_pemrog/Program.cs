using System;

abstract class Kendaraan
{
    public string Nama { get; set; }

    public Kendaraan(string nama)
    {
        Nama = nama;
    }

    public abstract void Bergerak();
}

class Mobil : Kendaraan
{
    public Mobil(string nama) : base(nama) { }

    public override void Bergerak()
    {
        Console.WriteLine(Nama + " bergerak dengan roda empat.");
    }
}

class Sepeda : Kendaraan
{
    public Sepeda(string nama) : base(nama) { }

    public override void Bergerak()
    {
        Console.WriteLine(Nama + " bergerak dengan menggunakan tenaga kaki.");
    }
}

class Pesawat : Kendaraan
{
    public Pesawat(string nama) : base(nama) { }

    public override void Bergerak()
    {
        Console.WriteLine(Nama + " terbang di udara.");
    }
}

class Garasi
{
    public void Parkir(Kendaraan kendaraan)
    {
        Console.WriteLine("Memarkirkan " + kendaraan.Nama);
        kendaraan.Bergerak();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Mobil sedan = new Mobil("Sedan");
        Sepeda sepeda = new Sepeda("Polygon");
        Pesawat pesawat = new Pesawat("Boeing 747");

        Garasi garasi = new Garasi();

        garasi.Parkir(sedan);
        garasi.Parkir(sepeda);
        garasi.Parkir(pesawat);
    }
}
