using System.Linq;

namespace RE_HighVoltage
{
    public class Berlo
    {
        public string PersonalID;
        public string Name;
        public DateOnly Dateofbirth;
        public int PostalCode;
        public string City;
        public string Address;
        public string County;

        public Berlo(string id, string name, string dob, string post, string city, string address, string county) 
        {
            PersonalID = id;
            Name = name;
            Dateofbirth = DateOnly.Parse(dob);
            PostalCode = Convert.ToInt32(post);
            City = city;
            Address = address;
            County = county;
        }
    }

    public class Laptop
    {
        public string Invnumber;
        public string Model;
        public int RAM;
        public string Color;

        public Laptop(string invnumber, string model, string rAM, string color)
        {
            Invnumber = invnumber;
            Model = model;
            RAM = Convert.ToInt32(rAM);
            Color = color;
        }

        public override string ToString()
        {
            return $"{Invnumber} {Model} {Color}";
        }
    }
    public class Berles
    {
        public Berlo berlo;
        public Laptop laptop;
        public int DailyFee;
        public int Deposit;
        public DateOnly StartDate;
        public DateOnly EndDate;
        public bool UseDeposit;
        public double Uptime;

        public Berles(string Line)
        {
            string[] line = Line.Split(";");
            berlo = new Berlo(
                line[0], line[1], line[2], line[3], line[4], line[5], line[8]
            );
            laptop = new Laptop(
                line[6], line[7], line[9], line[10]
            );
            DailyFee = Convert.ToInt32(line[11]);
            Deposit = Convert.ToInt32(line[12]);
            StartDate = DateOnly.Parse(line[13]);
            EndDate = DateOnly.Parse(line[14]);
            UseDeposit = Convert.ToBoolean(Convert.ToInt32(line[15]));
            Uptime = Convert.ToDouble(line[16]);
        }
    }
    internal class Program
    {

        static void Main(string[] args)
        {
            List<Berles> berles = new List<Berles>();
            StreamReader reader = new StreamReader("laptoprentals2022.csv");
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                berles.Add(new Berles(reader.ReadLine()));
            }

            void Feladat_3(List<Berles> berlesek)
            {
                Console.WriteLine($"3. feladat: Bérlések darabszáma: {berlesek.Count}");
            }

            void Feladat_4(List<Berles> berlesek)
            {
                var szurt = berlesek.Where((Berles berles) => berles.laptop.Color == "szürke" && berles.laptop.Model.StartsWith("Acer")).OrderBy((Berles berles) => berles.laptop.Invnumber);
                Console.WriteLine("4. feladat: Szürke Acer bérlések");
                foreach (Berles berles in szurt)
                {
                    Console.WriteLine($"\t{berles.laptop.Invnumber} {berles.laptop.Model} --- {berles.berlo.PersonalID} {berles.berlo.Name}");
                }
            }

            void Feladat_5(List<Berles> berlesek)
            {
                Dictionary<string, int> megyeSzamPar = new Dictionary<string, int>();
                var megyek = berlesek.Select((Berles berles) => berles.berlo.County);
                foreach (var megye in megyek)
                {
                    var szam = berlesek.Where((Berles berles) => berles.berlo.County == megye).Count();
                    megyeSzamPar[megye] = szam;
                }
                var Rendezett = megyeSzamPar.OrderBy(x => x.Value).Take(2);

                Console.WriteLine("5. feladat: Vármegyék, ahol a legkevesebb laptopot bérelték");


                foreach (var rendezett in Rendezett)
                {
                    Console.WriteLine($"\t{rendezett.Key} : {rendezett.Value}");
                }
            }

            string LeltarBekeres()
            {
                string szam = Console.ReadLine();
                if (szam == string.Empty || szam == "0" || szam == null || szam.Length != 9)
                {
                    return string.Empty;
                }

                if (szam[..3] != "LPT" || !szam[3..].All(x => Char.IsDigit(x)))
                {
                    return string.Empty;
                }

                return szam;
            }

            void Feladat_6(List<Berles> berlesek)
            {
                Console.WriteLine("6. feladat:");
                Console.Write("\tKeresett leltári szám: ");
                string szam = LeltarBekeres();

                if (szam == string.Empty)
                {
                    return;
                }

                Berles? berles = berlesek.FirstOrDefault(x => x.laptop.Invnumber == szam, null);

                if (berles == null)
                {
                    Console.WriteLine("\tNincs ilyen leltári számú laptop!");
                } else
                {
                    Console.WriteLine($"\t{berles.laptop}");
                }
            }

            void Feladat_7(List<Berles> berlesek)
            {
                var ossz = berlesek.Sum(x => (x.EndDate.DayNumber - x.StartDate.DayNumber + 1) * x.DailyFee);
                var kauciobevetelek = berlesek.Sum(x => x.UseDeposit ? x.Deposit : 0);

                Console.WriteLine($"7. feladat: A cég összes bevétele: {ossz + kauciobevetelek:# ### ###} FT");
            }

            void Feladat_8(List<Berles> berlesek)
            {
                Console.Write("Keresett leltári szám: ");
                string szam = LeltarBekeres();

                var atlag = berlesek.Sum(x => x.laptop.Invnumber == szam ? x.Uptime : 0) / berlesek.Where(x => x.laptop.Invnumber == szam).Count();
                Console.WriteLine($"8. feladat: Az {szam} leltári számú laptop bérlésenkénti átlagos üzemideje: {Math.Round(atlag, 2)} óra");
            }

            Feladat_3(berles);
            Feladat_4(berles);
            Feladat_5(berles);
            Feladat_6(berles);
            Feladat_7(berles);
            Feladat_8(berles);
        }
    }
}
