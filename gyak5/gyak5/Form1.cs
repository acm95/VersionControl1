using gyak5.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList();
            dataGridView1.DataSource = Ticks;

            CreatePortfolio();
        }
        private void CreatePortfolio()
        {
            Portfolio1.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio1.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio1.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio1;
        }
    }
}
