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
using MFC.pages;
using MFC.source;
using System.Data.Entity;


namespace MFC.pages
{
    /// <summary>
    /// Логика взаимодействия для AddObrabotchika.xaml
    /// </summary>
    public partial class AddObrabotchika : Page
    {
        mfc_databaseEntities db;
        public AddObrabotchika()
        {
            InitializeComponent();
            db = new mfc_databaseEntities();
            db.Role.Load();
            cbx_role.ItemsSource = db.Role.Select(x => x.role_title).ToList();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(null);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (tbx_name.Text != null
                && tbx_login.Text != null
                && tbx_password.Text != null)
            {
                Employ emp = new Employ()
                {
                    emp_name = tbx_name.Text.ToString(),
                    login = tbx_login.Text.ToString(),
                    password = tbx_password.Text.ToString(),
                    id_role = Convert.ToInt32(cbx_role.SelectedIndex + 1)
                };

                db.Employ.Add(emp);
                db.SaveChanges();
                MessageBox.Show("Добавлена новая учетная запись!");

            }
            else
                MessageBox.Show("Заполните все поля ввода");
        }
    }
}
