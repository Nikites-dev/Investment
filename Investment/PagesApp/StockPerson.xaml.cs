using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Investment.ADOApp;
using static System.Net.Mime.MediaTypeNames;
using Color = System.Drawing.Color;

namespace Investment.PagesApp
{
    public partial class StockPerson : Window
    {
        public Stock StockCurrent { get; set; }
        public User UserCurrent { get; set; }

        public StockPerson()
        {
            InitializeComponent();
        }

        public StockPerson(Stock stock, User user)
        {
            InitializeComponent();
            StockCurrent = stock;
            UserCurrent = user;

            SetData();
        }

        public void SetData()
        {
            TxtName.Text = StockCurrent.Company.Name;
            TxtShortName.Text = StockCurrent.Company.ShortName;
            TxtSector.Text = StockCurrent.Company.Sector.NameSector;
            TxtPrice.Text = StockCurrent.Price.ToString();
            TxtInfo.Text = StockCurrent.Company.Info;

            MemoryStream stream = new MemoryStream(StockCurrent.Company.ImageLogo);
            ImageLogo.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

        }



        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

