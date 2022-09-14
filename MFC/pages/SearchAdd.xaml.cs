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
using System.Diagnostics;


namespace MFC.pages
{
    /// <summary>
    /// Логика взаимодействия для SearchAdd.xaml
    /// </summary>
    public partial class SearchAdd : Window
    {
        private Clients _current_client = new Clients();

        mfc_databaseEntities db = new mfc_databaseEntities();
        
        public SearchAdd()
        {
            InitializeComponent();
            DataContext = _current_client;
            lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();

            cbx_Status.ItemsSource = mfc_databaseEntities.GetContext().Status.ToList();
            cbx_Usluga.ItemsSource = mfc_databaseEntities.GetContext().Usluga.ToList();
            cbx_nomerUdost1.ItemsSource = mfc_databaseEntities.GetContext().Udost_lich.ToList();
            cbx_family.ItemsSource = mfc_databaseEntities.GetContext().Family.ToList();
            cbx_Usluga.SelectedIndex = 0;
            cbx_Status.SelectedIndex = 0;
            lv_usl.ItemsSource = mfc_databaseEntities.GetContext().Usluga.Where(x => x.id_usl > 1).ToList();



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

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            Search();
            lv_clientSearch.SelectedItem = null;
            var sup = mfc_databaseEntities.GetContext().Usluga.Where(x => x.usl_title == txb_usl_add.Text.ToString()).Select(x => x.id_usl).ToString();
            MessageBox.Show(sup);
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
            lv_clientSearch.SelectedItem = null;
            lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();            
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            lv_clientSearch.SelectedItem = null;
            stp_clientData.IsEnabled = true;
            cbx_Usluga.SelectedIndex = 0;
            cbx_Status.SelectedIndex = 0;
            cbx_OrderBy.SelectedItem = null;
            tbx_Familia1.Text = tbx_Familia.Text;
            txb_Name1.Text = txb_Name.Text;
            tbx_Otchestvo1.Text = tbx_Otchestvo.Text;
            dtp_Birth1.SelectedDate = dtp_Birth.SelectedDate;
            tbx_Familia.Text = null;
            txb_Name.Text = null;
            tbx_Otchestvo.Text = null;
            dtp_Birth.SelectedDate = null;
            tbx_snils.Text = null;
            tbx_nomerUdost1.Text = null;
            tbx_addressReg.Text = null;
            cbx_family.Text = null;
            cbx_nomerUdost1.Text = null;
        }

        private void btn_print_Click(object sender, RoutedEventArgs e)
        {
            Print pr = new Print();
            pr.Show();
        }

        private void btn_accept_Click(object sender, RoutedEventArgs e)
        {
            Clients ser = lv_clientSearch.SelectedItem as Clients;

            if (tbl_id.Text != "")
            {
                if (lv_clientSearch.SelectedItems.Count == 1)
                {
                    MessageBoxResult res = MessageBox.Show($"Добавить новую услугу клиенту {ser.familia} {ser.name} {ser.otchestvo}?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (MessageBoxResult.Yes == res)
                    {
                        stp_color.Visibility = Visibility.Hidden;
                        stp_search1.Visibility = Visibility.Hidden;
                        stp_buttons.Visibility = Visibility.Hidden;
                        lb_filtr.Visibility = Visibility.Hidden;
                        brd_filtr.Visibility = Visibility.Hidden;
                        lv_clientSearch.Visibility = Visibility.Hidden;
                        scr_clientData.Visibility = Visibility.Hidden;
                        stp_ads.Visibility = Visibility.Visible;

                        lv_poshlina.ItemsSource = mfc_databaseEntities.GetContext().Poshlina.ToList();
                        lv_medspr.ItemsSource = mfc_databaseEntities.GetContext().Med_spravka.ToList();
                        lv_svidet.ItemsSource = mfc_databaseEntities.GetContext().Svidet.ToList();
                        lv_comp.ItemsSource = mfc_databaseEntities.GetContext().Comp_strah.ToList();

                        lv_usl.SelectedItem = null;
                        lv_poshlina.SelectedItem = null;
                        lv_medspr.SelectedItem = null;
                        lv_svidet.SelectedItem = null;
                        lv_comp.SelectedItem = null;
                        ckb_photo.IsChecked = null;
                        ckb_poshlina.IsChecked = null;
                        tbx_medspr.Clear();
                        tbx_svidet.Clear();
                        tbx_predstav.Clear();
                    }
                }
                else
                    MessageBox.Show("Выберите одну запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                MessageBox.Show("Выберите клиента из списка или добавьте нового!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void btn_sogl_Click(object sender, RoutedEventArgs e)
        {
            Clients ser = lv_clientSearch.SelectedItem as Clients;

            if (ser.id_usl != null)
            {
                var clien = new Clients()
                {
                    familia = ser.familia.ToString(),
                    name = ser.name.ToString(),
                    otchestvo = ser.otchestvo.ToString(),
                    date_birth = ser.date_birth,
                    snils = ser.snils.ToString(),
                    address_reg = ser.address_reg.ToString(),
                    nomer_udost = ser.nomer_udost.ToString(),
                    id_udost = Convert.ToInt32(ser.id_udost),
                    id_family = Convert.ToInt32(ser.id_family),
                    id_usl = Convert.ToInt32(lv_usl.SelectedIndex + 1),
                    id_poshlina = Convert.ToInt32(lv_poshlina.SelectedIndex + 1),
                    id_med = Convert.ToInt32(lv_medspr.SelectedIndex + 1),
                    id_stat = 2,
                    id_svidet = Convert.ToInt32(lv_svidet.SelectedIndex + 1),
                    id_comp = Convert.ToInt32(lv_comp.SelectedIndex + 1),
                    nomer_med = tbx_medspr.Text.ToString(),
                    nomer_predstavit = tbx_predstav.ToString(),
                    nomer_svedet = tbx_svidet.ToString()
                };
                mfc_databaseEntities.GetContext().Clients.Add(clien);
                mfc_databaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные добавлены",
                            "Уведомление",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            }
            else
            {
                mfc_databaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные добавлены",
                            "Уведомление",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
            }

            stp_color.Visibility = Visibility.Visible;
            stp_search1.Visibility = Visibility.Visible;
            stp_buttons.Visibility = Visibility.Visible;

            lb_filtr.Visibility = Visibility.Visible;
            brd_filtr.Visibility = Visibility.Visible;
            lv_clientSearch.Visibility = Visibility.Visible;
            scr_clientData.Visibility = Visibility.Visible;
            stp_ads.Visibility = Visibility.Hidden;
            lv_clientSearch.SelectedItem = null;

            lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();

            //if (txb_Name1.Text != null
            //    && tbx_Familia1.Text != null
            //    && tbx_Otchestvo1.Text != null
            //    && dtp_Birth1.SelectedDate != null
            //    && tbx_snils.Text != null
            //    && tbx_nomerUdost1.Text != null
            //    && tbx_addressReg.Text != null
            //    && cbx_family.Text != null
            //    && cbx_nomerUdost1.Text != null)
            //{
            //    var cli = new Clients()
            //    {
            //        familia = tbx_Familia1.Text.ToString(),
            //        name = txb_Name1.Text.ToString(),
            //        otchestvo = tbx_Otchestvo1.Text.ToString(),
            //        date_birth = Convert.ToDateTime(dtp_Birth1.SelectedDate),
            //        snils = tbx_snils.Text.ToString(),
            //        address_reg = tbx_addressReg.Text.ToString(),
            //        nomer_udost = tbx_nomerUdost1.Text.ToString(),
            //        id_udost = Convert.ToInt32(cbx_nomerUdost1.SelectedIndex + 1),
            //        id_family = Convert.ToInt32(cbx_family.SelectedIndex + 1)

            //    };
            //}

            //mfc_databaseEntities.GetContext().Clients.Add(st);
            //mfc_databaseEntities.GetContext().SaveChanges();
            //MessageBox.Show("Изменения применены",
            //                "Уведомление",
            //                MessageBoxButton.OK,
            //                MessageBoxImage.Information);
            //mfc_databaseEntities.GetContext().SaveChanges();
            //MessageBox.Show("Данные добавлены",
            //                "Уведомление",
            //                MessageBoxButton.OK,
            //                MessageBoxImage.Information);

            //stp_color.Visibility = Visibility.Visible;
            //stp_search1.Visibility = Visibility.Visible;
            //stp_buttons.Visibility = Visibility.Visible;

            //lb_filtr.Visibility = Visibility.Visible;
            //brd_filtr.Visibility = Visibility.Visible;
            //lv_clientSearch.Visibility = Visibility.Visible;
            //scr_clientData.Visibility = Visibility.Visible;
            //stp_ads.Visibility = Visibility.Hidden;
            //lv_clientSearch.SelectedItem = null;
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
            public int id_poshlina { get; set; }
            public bool oplata { get; set; }
            public int id_med { get; set; }
            public string nomer_med { get; set; }
            public int id_svidet { get; set; }
            public string nomer_svedet { get; set; }
            public bool photo { get; set; }
            public string nomer_predstavit { get; set; }
            public int id_comp { get; set; }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            lv_clientSearch.SelectedItem = null;

            if (txb_Name1.Text != null
            && tbx_Familia1.Text != null
            && tbx_Otchestvo1.Text != null
            && dtp_Birth1.SelectedDate != null
            && tbx_snils.Text != null
            && tbx_nomerUdost1.Text != null
            && tbx_addressReg.Text != null
            && cbx_family.Text != null
            && cbx_nomerUdost1.Text != null)
            {
                var cli = new Clients()
                {
                    familia = tbx_Familia1.Text.ToString(),
                    name = txb_Name1.Text.ToString(),
                    otchestvo = tbx_Otchestvo1.Text.ToString(),
                    date_birth = Convert.ToDateTime(dtp_Birth1.SelectedDate),
                    snils = tbx_snils.Text.ToString(),
                    address_reg = tbx_addressReg.Text.ToString(),
                    nomer_udost = tbx_nomerUdost1.Text.ToString(),
                    id_udost = Convert.ToInt32(cbx_nomerUdost1.SelectedIndex + 1),
                    id_family = Convert.ToInt32(cbx_family.SelectedIndex + 1),
                    id_stat = 2,
                    id_usl = Convert.ToInt32(lv_usl.SelectedIndex + 2),
                };

                mfc_databaseEntities.GetContext().Clients.Add(cli);
                mfc_databaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные добавлены!");
                lv_clientSearch.ItemsSource = mfc_databaseEntities.GetContext().Clients.ToList();
                lv_clientSearch.SelectedItem = null;
                stp_clientData.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены");
            }
        }        

        private void btn_esc_Click(object sender, RoutedEventArgs e)
        {
            stp_color.Visibility = Visibility.Visible;
            stp_search1.Visibility = Visibility.Visible;
            stp_buttons.Visibility = Visibility.Visible;
            
            lb_filtr.Visibility = Visibility.Visible;
            brd_filtr.Visibility = Visibility.Visible;
            lv_clientSearch.Visibility = Visibility.Visible;
            scr_clientData.Visibility = Visibility.Visible;
            stp_ads.Visibility = Visibility.Hidden;
            lv_clientSearch.SelectedItem = null;
        }

        private void btn_1_Click(object sender, RoutedEventArgs e)
        {
            txb_usl_add.Text = "";
            lv_usl.SelectedItem = null;
            lv_usl.ItemsSource = mfc_databaseEntities.GetContext().Usluga.Where(x => x.id_usl > 1).ToList();

        }

        private void btn_2_Click(object sender, RoutedEventArgs e)
        {
            lv_poshlina.ItemsSource = mfc_databaseEntities.GetContext().Poshlina.ToList();
            lv_poshlina.SelectedItem = null;           
        }

        private void btn_3_Click(object sender, RoutedEventArgs e)
        {
            lv_medspr.ItemsSource = mfc_databaseEntities.GetContext().Med_spravka.ToList();
            lv_medspr.SelectedItem = null;
        }

        private void btn_4_Click(object sender, RoutedEventArgs e)
        {
            lv_svidet.ItemsSource = mfc_databaseEntities.GetContext().Svidet.ToList();
            lv_svidet.SelectedItem = null;
        }

        private void btn_5_Click(object sender, RoutedEventArgs e)
        {
            lv_comp.ItemsSource = mfc_databaseEntities.GetContext().Comp_strah.ToList();
            lv_comp.SelectedItem = null;
        }

        private void btn_a_Click(object sender, RoutedEventArgs e)
        {
            if (txb_usl_add.Text != "")
            {
                var usl = new Usluga()
                {
                    usl_title = txb_usl_add.Text.ToString(),
                };
                mfc_databaseEntities.GetContext().Usluga.Add(usl);
                mfc_databaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Изменения применены",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                lv_usl.ItemsSource = mfc_databaseEntities.GetContext().Usluga.Where(x => x.id_usl > 1).ToList();
            }
            else
                MessageBox.Show("Введите данные для сохранения");
        }

        private void btn_b_Click(object sender, RoutedEventArgs e)
        {
            if (txb_poshlina_add.Text != "")
            {
                var posh = new Poshlina()
                {
                    posh_title = txb_poshlina_add.Text.ToString(),
                };
                mfc_databaseEntities.GetContext().Poshlina.Add(posh);
                mfc_databaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Изменения применены",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                lv_poshlina.ItemsSource = mfc_databaseEntities.GetContext().Poshlina.ToList();
            }
            else
                MessageBox.Show("Введите данные для сохранения");
        }

        private void btn_c_Click(object sender, RoutedEventArgs e)
        {
            if (txb_medspr_add.Text != "")
            {
                var medspr = new Med_spravka()
                {
                    forma = txb_medspr_add.Text.ToString(),
                };
                mfc_databaseEntities.GetContext().Med_spravka.Add(medspr);
                mfc_databaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Изменения применены",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                lv_medspr.ItemsSource = mfc_databaseEntities.GetContext().Med_spravka.ToList();
            }
            else
                MessageBox.Show("Введите данные для сохранения");
        }

        private void btn_d_Click(object sender, RoutedEventArgs e)
        {
            if (txb_svidet_add.Text != "")
            {
                var svid = new Svidet()
                {
                    svidet_title = txb_svidet_add.Text.ToString(),
                };
                mfc_databaseEntities.GetContext().Svidet.Add(svid);
                mfc_databaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Изменения применены",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                lv_svidet.ItemsSource = mfc_databaseEntities.GetContext().Svidet.ToList();
            }
            else
                MessageBox.Show("Введите данные для сохранения");
        }

        private void btn_e_Click(object sender, RoutedEventArgs e)
        {
            if (txb_comp_add.Text != "")
            {
                var compstr = new Comp_strah()
                {
                    comp_name = txb_svidet_add.Text.ToString(),
                };
                mfc_databaseEntities.GetContext().Comp_strah.Add(compstr);
                mfc_databaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Изменения применены",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                lv_comp.ItemsSource = mfc_databaseEntities.GetContext().Comp_strah.ToList();
            }
            else
                MessageBox.Show("Введите данные для сохранения");
        }

        //private void txb_usl_add_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //var cur = mfc_databaseEntities.GetContext().Usluga.Where(x => x.usl_title.Contains(txb_usl_add.Text.ToString()) &&
        //    //    x.id_usl > 1).ToList();
        //    //lv_usl.ItemsSource = cur.ToList();
        //}
    }
}
