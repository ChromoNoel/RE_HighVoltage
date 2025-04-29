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

            //Feladat_3(berles);
            //Feladat_4(berles);
            Feladat_5(berles);
        }
    }
}
