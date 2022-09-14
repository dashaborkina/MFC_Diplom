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
using System.Windows.Shapes;
using MFC.pages;
using MFC.source;
using Word = Microsoft.Office.Interop.Word;
using System.Data.Entity;

namespace MFC.pages
{
    /// <summary>
    /// Логика взаимодействия для Obrabotka.xaml
    /// </summary>
    public partial class Obrabotka : Window
    {
        mfc_databaseEntities db;
        public Obrabotka()
        {
            InitializeComponent();
            lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();

            cbx_Status.ItemsSource = mfc_databaseEntities.GetContext().Status.ToList();
            cbx_Usluga.ItemsSource = mfc_databaseEntities.GetContext().Usluga.ToList();
            
            cbx_newstat.ItemsSource = mfc_databaseEntities.GetContext().Status.Where(x => x.id_stat > 1).ToList();

            cbx_Usluga.SelectedIndex = 0;
            cbx_Status.SelectedIndex = 0;
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (lv_clientSearch.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для изменения");
            }
            else
            {
                try
                {
                    mfc_databaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Изменения применены",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(),
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            cbx_Usluga.SelectedIndex = 0;
            cbx_Status.SelectedIndex = 0;
            cbx_OrderBy.SelectedItem = null;
            tbx_Familia.Text = null;
            txb_Name.Text = null;
            tbx_Otchestvo.Text = null;
            dtp_Birth.SelectedDate = null;
            lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();
        }

        private void lv_clientSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cbx_newstat.SelectedItem = null;
            stp_newstat.IsEnabled = true;
        }

        private void Search()
        {
            var current = mfc_databaseEntities.GetContext().Clients.ToList();

            if (cbx_Usluga.SelectedIndex == 0)
                current = current.ToList();
            if (cbx_Usluga.SelectedIndex != 0 && cbx_Usluga.SelectedItem != null)
                current = current.Where(x => x.id_usl.Equals(cbx_Usluga.SelectedIndex + 1)).ToList();

            if (tbx_Familia.Text != null)
                current = current.Where(x => x.familia.ToLower().Contains(tbx_Familia.Text.ToLower().ToString())).ToList();
            if (txb_Name.Text.ToString() != "")
                current = current.Where(x => x.name.ToLower().Contains(txb_Name.Text.ToLower().ToString())).ToList();
            if (tbx_Otchestvo.Text.ToString() != "")
                current = current.Where(x => x.otchestvo.ToLower().Contains(tbx_Otchestvo.Text.ToLower().ToString())).ToList();
            if (dtp_Birth.SelectedDate.ToString() != "")
                current = current.Where(x => x.date_birth.Equals(dtp_Birth.SelectedDate)).ToList();

            if (cbx_Status.SelectedIndex == 0)
                current = current.ToList();
            if (cbx_Status.SelectedIndex != 0 && cbx_Status.SelectedItem != null)
                current = current.Where(x => x.id_stat == cbx_Status.SelectedIndex + 1).ToList();

            if (cbx_OrderBy.SelectedItem != null)
            {
                if (cbx_OrderBy.SelectedIndex == 0)
                    current = current.OrderBy(x => x.familia).ToList();
                else if (cbx_OrderBy.SelectedIndex == 1)
                    current = current.OrderByDescending(x => x.familia).ToList();
            }

            lv_clientSearch.ItemsSource = current.ToList();
        }

        public class Client
        {
            public int id_client { get; set; }
            public string familia { get; set; }
            public string name { get; set; }
            public string otchestvo { get; set; }
            public DateTime date_birth { get; set; }
            public string address_reg { get; set; }
            public string snils { get; set; }
            public int id_udost { get; set; }
            public string nomer_udost { get; set; }
            public int id_usl { get; set; }
            public string id_family { get; set; }
            public int id_doc { get; set; }
            public int id_stat { get; set; }
        }

        private void btn_po_Click(object sender, RoutedEventArgs e)
        {
            lv_clientSearch.SelectedItem = null;
        }
    }
}
