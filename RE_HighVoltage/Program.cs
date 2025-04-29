using System.Linq;

namespace RE_HighVoltage
{
    public class Berles
    {
        public string PersonalID;
        public string Name;
        public DateOnly Dateofbirth;
        public int PostalCode;
        public string City;
        public string Address;
        public string Invnumber;
        public string Model;
        public string County;
        public int RAM;
        public string Color;
        public int DailyFee;
        public int Deposit;
        public DateOnly StartDate;
        public DateOnly EndDate;
        public bool UseDeposit;
        public double Uptime;

        public Berles(string Line)
        {
            string[] line = Line.Split(";");
            PersonalID = line[0];
            Name = line[1];
            Dateofbirth = DateOnly.Parse(line[2]);
            PostalCode = Convert.ToInt32(line[3]);
            City = line[4];
            Address = line[5];
            Invnumber = line[6];
            Model = line[7];
            County = line[8];
            RAM = Convert.ToInt32(line[9]);
            Color = line[10];
            DailyFee = Convert.ToInt32(line[11]);
            Deposit = Convert.ToInt32(line[12]);
            StartDate = DateOnly.Parse(line[13]);
            EndDate = DateOnly.Parse(line[14]);
            UseDeposit = Convert.ToBoolean(Convert.ToInt32(line[15]));
            Uptime = Convert.ToDouble(line[16]);
        }

        public override string ToString()
        {
            return $"{Invnumber} {Model} {Color}";
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
                var szurt = berlesek.Where((Berles berles) => berles.Color == "Szürke" && berles.Model.StartsWith("Acer")).OrderBy((Berles berles) => berles.Invnumber);
                Console.WriteLine("4. feladat: Szürke Acer bérlések");
                foreach (Berles berles in szurt)
                {
                    Console.WriteLine($"\t{berles.Invnumber} {berles.Model} --- {berles.PersonalID} {berles.Name}");
                }
            }

            void Feladat_5(List<Berles> berlesek)
            {
                Dictionary<string, int> megyeSzamPar = new Dictionary<string, int>();
                var megyek = berlesek.Select((Berles berles) => berles.County);
                foreach (var megye in megyek)
                {
                    var szam = berlesek.Where((Berles berles) => berles.County == megye).Count();
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

                Berles? berles = berlesek.FirstOrDefault(x => x.Invnumber == szam, null);

                if (berles == null)
                {
                    Console.WriteLine("\tNincs ilyen leltári számú laptop!");
                } else
                {
                    Console.WriteLine($"\t{berles}");
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

                var atlag = berlesek.Sum(x => x.Invnumber == szam ? x.Uptime : 0) / berlesek.Where(x => x.Invnumber == szam).Count();
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
