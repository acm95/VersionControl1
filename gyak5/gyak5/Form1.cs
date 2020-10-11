using gyak5.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gyak5
{
    public partial class Form1 : Form
    {

        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;
        private List<PortfolioItem> Portfolio1 = new List<PortfolioItem>();
        List<decimal> Nyereségek = new List<decimal>();

        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList();
            dataGridView1.DataSource = Ticks;

            CreatePortfolio();

            //a. Portfóliónk elemszáma:
            int elemszám = Portfolio1.Count();
            //A Count() bálrmilyen megszámlálható listára alkalmazható.

            //b. A portfólióban szereplő részvények darabszáma: 
            decimal részvényekSzáma = (from x in Portfolio1 select x.Volume).Sum();
            MessageBox.Show(string.Format("Részvények száma: {0}", részvényekSzáma));
            //Először egy listába kigyűjtjük csak a darabszámokat, majd az egész bezárójlezett listát summázzuk. 
            //(A zárójelben lévő LINQ egy int-ekből álló listát ad, mert a Count tulajdonság int típusú.)
            //Működik a Min(), Max(), Average(), stb. is.

            //c. A legrégebbi kereskedési nap:
            DateTime minDátum = (from x in Ticks select x.TradingDay).Min();

            //d. A legutolsó kereskedési nap:
            DateTime maxDátum = (from x in Ticks select x.TradingDay).Max();

            //e. A két dátum közt eltelt idő napokban -- két DateTime típusú objektum különbsége TimeSpan típusú eredményt ad.
            //A TimeSpan Day tulajdonsága megadja az időtartam napjainak számát. (Nem kell vacakolni a szökőévekkel stb.)
            int elteltNapokSzáma = (maxDátum - minDátum).Days;

            //f. Az OTP legrégebbi kereskedési napja: 
            DateTime optMinDátum = (from x in Ticks where x.Index == "OTP" select x.TradingDay).Min();

            //g. Össze is lehet kapcsolni dolgokat, ez már bonyolultabb:
            var kapcsolt =
                from
                    x in Ticks
                join
y in Portfolio1
    on x.Index equals y.Index
                select new
                {
                    Index = x.Index,
                    Date = x.TradingDay,
                    Value = x.Price,
                    Volume = y.Volume
                };
            dataGridView1.DataSource = kapcsolt.ToList();

            
            int intervalum = 30;
            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = new DateTime(2016, 12, 30);
            TimeSpan z = záróDátum - kezdőDátum;
            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            var nyereségekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x)
                                        .ToList();
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].ToString());
        }
        private void CreatePortfolio()
        {
            Portfolio1.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio1.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio1.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio1;
        }
        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio1)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim()
                               && date <= x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "Comma Seperated Values (*.csv)|*.csv| All files (*.*)|*.*";
            sfd.DefaultExt = "Save";
            sfd.AddExtension = true;
            sfd.ShowDialog();


            if (sfd.FileName != "")
            {
                int intervalum = 30;
                DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
                DateTime záróDátum = new DateTime(2016, 12, 30);
                TimeSpan z = záróDátum - kezdőDátum;
                for (int i = 0; i < z.Days - intervalum; i++)
                {
                    decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                               - GetPortfolioValue(kezdőDátum.AddDays(i));
                    Nyereségek.Add(ny);
                    Console.WriteLine(i + " " + ny);
                    Console.WriteLine(Nyereségek);
                }

                var nyereségekRendezve = (from x in Nyereségek
                                          orderby x
                                          select x)
                                            .ToList();

                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    sw.Write("Időszak");
                    sw.Write(";");
                    sw.Write("Nyereség");
                    sw.WriteLine();

                    for (int i = 0; i < nyereségekRendezve.Count; i++)
                    {
                        sw.Write(i);
                        sw.Write(";");
                        sw.Write(nyereségekRendezve[i]);
                        sw.WriteLine();
                    }
                }

                MessageBox.Show("A nyereséglista fájlba írása sikeres volt!");
            }
        }
    }
}
