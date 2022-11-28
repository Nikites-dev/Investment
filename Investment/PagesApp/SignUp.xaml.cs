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
using Investment.ADOApp;

namespace Investment.PagesApp
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if(EdName.Text != "" && EdLogin.Text != "" && EdPassword.Password != "")
            {
                if(App.Connection.Authorization.Where(x => x.Login == EdLogin.Text).FirstOrDefault() == null)
                {
                    User user = new User();
                    user.Balance = 10000;
                    user.Name = EdName.Text;

                    App.Connection.User.Add(user);
                    App.Connection.SaveChanges();


                    Authorization auth = new Authorization();
                    auth.Login = EdLogin.Text;
                    auth.Password = EdPassword.Password;
                    auth.IdUser = user.IdUser;

                    App.Connection.Authorization.Add(auth);
                    App.Connection.SaveChanges();

                    NavigationService.Navigate(new SignIn());
                } else
                {
                    MessageBox.Show("such user already exists!");
                }
            } else
            {
                MessageBox.Show("fields is empty!");
            }
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }
    }
}
