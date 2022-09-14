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
using Excel = Microsoft.Office.Interop.Excel;

namespace MFC.pages
{
    /// <summary>
    /// Логика взаимодействия для Print.xaml
    /// </summary>
    public partial class Print : Window
    {
        private mfc_databaseEntities _clients = new mfc_databaseEntities();
        mfc_databaseEntities db;
        public Print()
        {
            InitializeComponent();
            db = new mfc_databaseEntities();
            cbx_stat.ItemsSource = db.Status.Select(x => x.stat_title).ToList();
            cbx_usl.ItemsSource = db.Usluga.Select(x => x.usl_title).ToList();
            cbx_usl.SelectedIndex = 0;
            cbx_stat.SelectedIndex = 0;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Admin adm = new Admin();
            this.Close();            
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            var current = mfc_databaseEntities.GetContext().Clients.ToList();
            
           
            if (cbx_usl.SelectedIndex == 0)
            {
                current = current.OrderBy(x => x.familia).ToList();
            }

            if (cbx_usl.SelectedIndex != 0 && cbx_usl.SelectedItem != null)
            {
                current = current.Where(x => x.id_usl.Equals(cbx_usl.SelectedIndex + 1)).OrderBy(x => x.familia).ToList();
            }
            else
            {
                current = current.OrderBy(x => x.familia).ToList();
            }

            if (cbx_stat.SelectedIndex == 0)
            {
                current = current.OrderBy(x => x.familia).ToList();
            }

            if (cbx_stat.SelectedIndex != 0 && cbx_stat.SelectedItem != null)
            {
                current = current.Where(x => x.id_stat == cbx_stat.SelectedIndex + 1).OrderBy(x => x.familia).ToList();
            }
            else
            {
                current = current.OrderBy(x => x.familia).ToList();
            }


            if (cbx_sr.SelectedIndex == 0)
            {   var application = new Word.Application();
                Word.Document document = application.Documents.Add();
                Word.Paragraph paragraph = document.Paragraphs.Add();
                Word.Range userRange = paragraph.Range;

                userRange.Text = DateTime.Today.ToString("dd.MM.yyyy") + "\nКлиенты | " + cbx_usl.SelectedItem.ToString() + "| " + cbx_stat.SelectedItem.ToString() + " \nКоличество записей: " + current.Count.ToString() +"шт.";
                userRange.InsertParagraphAfter();
                Word.Paragraph tableparagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableparagraph.Range;

                Word.Table infoTable = document.Tables.Add(tableRange, current.Count() + 1, 5);

                infoTable.Borders.InsideLineStyle = infoTable.Borders.OutsideLineStyle
                        = Word.WdLineStyle.wdLineStyleSingle;
                infoTable.Range.Cells.VerticalAlignment
                        = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                Word.Range cellRange;
                cellRange = infoTable.Cell(1, 1).Range;
                cellRange.Text = "Фамилия";
                cellRange = infoTable.Cell(1, 2).Range;
                cellRange.Text = "Имя";
                cellRange = infoTable.Cell(1, 3).Range;
                cellRange.Text = "Отчество";
                cellRange = infoTable.Cell(1, 4).Range;
                cellRange.Text = "Дата рождения";
                cellRange = infoTable.Cell(1, 5).Range;
                cellRange.Text = "Адрес регистрации";

                infoTable.Rows[1].Range.Bold = 1;
                for (int i = 0; i < current.Count(); i++)
                {
                    cellRange = infoTable.Cell(i + 2, 1).Range;
                    cellRange.Text = current[i].familia;
                    cellRange = infoTable.Cell(i + 2, 2).Range;
                    cellRange.Text = current[i].name;
                    cellRange = infoTable.Cell(i + 2, 3).Range;
                    cellRange.Text = current[i].otchestvo;
                    cellRange = infoTable.Cell(i + 2, 4).Range;
                    cellRange.Text = current[i].date_birth.ToString();
                    cellRange = infoTable.Cell(i + 2, 5).Range;
                    cellRange.Text = current[i].address_reg.ToString();
                }

                application.Visible = true;
                //document.SaveAs(@"C:\Users\Dasha Borkina\Desktop.docx");

                this.Close();
            }
            else if (cbx_sr.SelectedIndex == 1)
            {
                var application = new Excel.Application();
                application.SheetsInNewWorkbook = 1;

                Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);
                Excel.Worksheet worksheet = application.Worksheets.Item[1];

                Excel.Range uslstat = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[5][1]]; 
                uslstat.Merge();
                uslstat.Value = cbx_usl.SelectedItem.ToString() + "|" + cbx_stat.SelectedItem.ToString();
                uslstat.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;               

                Excel.Range clicount = worksheet.Range[worksheet.Cells[1][2], worksheet.Cells[5][2]];
                clicount.Merge();
                clicount.Value = "Количество записей: " + current.Count().ToString();
                clicount.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                worksheet.Name = "Клиенты " + DateTime.Today.ToString("dd.MM.yyyy");
                worksheet.Cells[1][4] = "Фамилия";
                worksheet.Cells[2][4] = "Имя";
                worksheet.Cells[3][4] = "Отчество";
                worksheet.Cells[4][4] = "Дата рождения";
                worksheet.Cells[5][4] = "Адрес прописки";
                for (int i = 0; i < current.Count(); i++)
                {
                    worksheet.Cells[1][i + 5] = current[i].familia.ToString();
                    worksheet.Cells[2][i + 5] = current[i].name.ToString();
                    worksheet.Cells[3][i + 5] = current[i].otchestvo.ToString();
                    worksheet.Cells[4][i + 5] = current[i].date_birth.ToString();
                    worksheet.Cells[5][i + 5] = current[i].address_reg.ToString();
                }

                Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1][4], worksheet.Cells[5][current.Count() + 4]];
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                worksheet.Columns.AutoFit();
                application.Visible = true;
                
                this.Close();

            }
            else
            {
                MessageBox.Show("Пожалуйста выберите среду вывода отчётности");  
            }

            

            
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void Window_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //this.Close();
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            //Close();

        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            cbx_sr.SelectedItem = null;
            cbx_stat.SelectedIndex = 0;
            cbx_usl.SelectedIndex = 0;
        }
    }
}
