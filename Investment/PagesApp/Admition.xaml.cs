using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Investment.ADOApp;

namespace Investment.PagesApp
{
    public partial class Admition : Page
    {
        public byte[] ImageBin { get; set; }

        public Admition()
        {
            InitializeComponent();
            SetCmbSector();
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();   

            if(openFileDialog.ShowDialog() != null)
            {
                ImageBin = File.ReadAllBytes(openFileDialog.FileName);
                MessageBox.Show("upload succes!");
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            Company company = new Company();

            company.Name = EdName.Text;
            company.NumberOfStocks = int.Parse(EdAmountStocks.Text);
            company.ImageLogo = ImageBin;
            company.Info = EdInfo.Text;
            company.IdSector = CmbSector.SelectedIndex + 1;
            company.ShortName = EdShortName.Text;

            App.Connection.Company.Add(company);
            App.Connection.SaveChanges();

            MessageBox.Show((CmbSector.SelectedIndex + 1).ToString());
        }

        private void CmbSector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
      
        }

        public void SetCmbSector()
        {
            var listSector = App.Connection.Sector.ToList();
            CmbSector.Items.Clear();

            foreach(var sector in listSector)
            {
                CmbSector.Items.Add(sector.NameSector);
            }
        }
    }
}
