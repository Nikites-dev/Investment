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

namespace Investment.PagesApp
{
    /// <summary>
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Page
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if(EdLogin.Text != "" && EdPassword.Password != "")
            {
                if(App.Connection.Authorization.Where(x => x.Login == EdLogin.Text && x.Password == EdPassword.Password).FirstOrDefault() != null)
                {
                    NavigationService.Navigate(new UserMain());
                } else
                {
                    MessageBox.Show("неверный логин или пароль!");
                }
            }else
            {
                MessageBox.Show("");
            }
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignUp());
        }
    }
}
