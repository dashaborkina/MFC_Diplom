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
using Word = Microsoft.Office.Interop.Word;
using System.Data.Entity;

namespace MFC.pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        mfc_databaseEntities db;
        public Admin()
        {
            InitializeComponent();
            lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();

            cbx_Status.ItemsSource = mfc_databaseEntities.GetContext().Status.ToList();
            cbx_Usluga.ItemsSource = mfc_databaseEntities.GetContext().Usluga.ToList();
            cbx_nomerUdost1.ItemsSource = mfc_databaseEntities.GetContext().Udost_lich.ToList();
            cbx_family.ItemsSource = mfc_databaseEntities.GetContext().Family.ToList();
            cbx_Usluga.SelectedIndex = 0;
            cbx_Status.SelectedIndex = 0;
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

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            Clients ser = lv_clientSearch.SelectedItem as Clients;
            try
            {
                if (lv_clientSearch.SelectedItems.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить {lv_clientSearch.SelectedItems.Count} запись(-и)?",
                                                          "Предупреждение",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        mfc_databaseEntities.GetContext().Clients.Remove(ser);
                        mfc_databaseEntities.GetContext().SaveChanges();
                        MessageBox.Show("Записть удалена!");
                        lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();

                    }
                    if (result == MessageBoxResult.No)
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
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

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void lv_clientSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stp_clientData.IsEnabled = false;
        }

        private void btn_print_Click(object sender, RoutedEventArgs e)
        {
            Print prin = new Print();
            prin.Show();
            
        }

        private void btn_addEmp_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.frmObj = frm_Admin;
            frm_Admin.Navigate(new AddObrabotchika());
        }

        private void btn_change_Click(object sender, RoutedEventArgs e)
        {
            stp_clientData.IsEnabled = true;
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

        private void btn_save_Click(object sender, RoutedEventArgs e)
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
    }
}
