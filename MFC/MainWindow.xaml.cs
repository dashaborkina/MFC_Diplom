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
using System.Data.Entity;
using MFC.pages;
using MFC.source;

namespace MFC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        mfc_databaseEntities db;
        public MainWindow()
        {
            InitializeComponent();
         }
        private void btn_Welcome_Click(object sender, RoutedEventArgs e)
        {
            Admin adm = new Admin();
            SearchAdd searchad = new SearchAdd();
            Obrabotka obrab = new Obrabotka();
            
            
            FrameApp.db.Employ.Load();
            var usr = FrameApp.db.Employ.FirstOrDefault(x => x.login == tbx_login.Text && x.password == tbx_password.Password);
            if (usr != null)
            {
                if (usr.Role.id_role == 1)
                {
                    adm.Show();
                    this.Close();
                }
                else if (usr.Role.id_role == 2)
                {
                    searchad.Show();
                    this.Close();
                }
                else if (usr.Role.id_role == 3)
                {
                    obrab.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка!", "Неверный тип пользователя.");
                }
            }
            else if (tbx_login.Text == "" || tbx_password.Password == "")
            {
                MessageBox.Show("Заполните все поля!", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbx_login.Text = null;
                tbx_password.Clear();
            }

        }
    }
}
