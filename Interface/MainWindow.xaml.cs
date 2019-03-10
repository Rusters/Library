using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using Interface.Models;
using Interface.Classes;
using System.Collections.ObjectModel;
using System.IO;

namespace Interface
{
    public partial class MainWindow : Window
    {
        bool ad = false; //проверка добавления читателей, авторов
        DiagramContainer db; //класс контекста
        List<Authors> LAuthor = new List<Authors>();
        public int art = 0; //артикул выбранной книги на вкладке "База..."
        int selected_item = 0;
        public MainWindow()
        {
            InitializeComponent();
            db = new DiagramContainer();
            ViewData.UpdateTableAuthor(db, listAuthor); //обновляем listbox с ФИО авторов
            ViewData.UpdateTableReader(db, dgReader);//обновляем listbox с книгами выбранного автора
        }
        public MainWindow(string name) : this() => Check_Name_Book = name;
        public string Check_Name_Book { get; set; }
        /// <summary>
        /// Обработка клика на кнопку "Добавить читателя" на вкладке "Читатели"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddReader_Click(object sender, RoutedEventArgs e)
        {
            AddReader AddReader = new AddReader();
            if (AddReader.ShowDialog() == true)
            {
                ad = AddReader.AddR;
                ViewData.UpdateTableReader(db, dgReader);//обновляем listbox с книгами выбранного автора
            }
        }
        /// <summary>
        /// Открытие формы поиска книги и оформления аренды
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchBooks_Click(object sender, RoutedEventArgs e) => new Search().Show();
        /// <summary>
        /// Открытие формы для добавления новых книг
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddBooks_Click(object sender, RoutedEventArgs e) => new AddBook().Show();
        /// <summary>
        /// Открытие формы со списком должников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListOfDeptors_Click(object sender, RoutedEventArgs e) => new ListOfDeptors().Show();
        /// <summary>
        /// Обработка клика на кнопку "Добавить автора" на вкладке "База авторов и книг"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            AddAuthor AddAuthor = new AddAuthor();
            if (AddAuthor.ShowDialog() == true)
            {
                ad = AddAuthor.AddA;
                ViewData.UpdateTableAuthor(db, listAuthor); //обновляем listbox с ФИО авторов;
            }
        }
        /// <summary>
        /// При клике по автору выводятся все его книги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listAuthor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbArticul_Books.SelectedValue = null;
            cmbArticul_Books.Items.Clear();
            listBook.SelectedValue = null;
            listBook.Items.Clear();
            grid_Reader.Visibility = Visibility.Hidden;
            grid_Info_Book.Visibility = Visibility.Hidden;
            delete_Book.Visibility = Visibility.Hidden;
            tb_Name.Text = "";
            tb_Cost.Text = "";
            tb_Year.Text = "";
            tb_Date_out.Text = "";
            ViewData.Update_listbook(db, listBook, listAuthor);
        }
        /// <summary>
        /// После клика по названию книги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            delete_Book.Visibility = Visibility.Visible;
            btn_Add_Order_Main.Visibility = Visibility.Visible;
            cmbArticul_Books.SelectedValue = null;
            cmbArticul_Books.Items.Clear();
            tb_Date_out.Text = "";
            tb_Reader.Text = "";
            delete_Book.Visibility = Visibility.Visible;
            grid_Info_Book.Visibility = Visibility.Visible;
            btnDelete_AllBooks.Visibility = Visibility.Visible;
            if (listBook.SelectedValue != null)
            {
                Check_Name_Book = listBook.SelectedValue.ToString();
                selected_item = listBook.SelectedIndex;
            }
                
            if (listBook.SelectedItem != null)
                ViewData.UpdateComboArticul(db, cmbArticul_Books, listBook);
            cmbArticul_Books.SelectedIndex = 0;
            if (tb_Date_out.Text != "")
            {
                delete_Book.Visibility = Visibility.Hidden;
                btn_Add_Order_Main.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// Показ картинок Занято или Свободно в зависимости от наличия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_check_exemp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_check_exemp.Text == "0")
            {
                Image_check.Source = new BitmapImage(new Uri("pack://application:,,,/Images/off.jpg"));
                btn_Add_Order_Main.Visibility = Visibility.Hidden;
            }
            else
            {
                Image_check.Source = new BitmapImage(new Uri("pack://application:,,,/Images/on.jpg"));
                btn_Add_Order_Main.Visibility = Visibility.Visible;
            }
            Image_check.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Обработка клика по кнопке "Выдать книгу"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Order_Main_Click(object sender, RoutedEventArgs e)
        {
            Search search = new Search();
            search.Width = 700;
            search.list_result_search.Items.Add(Check_Name_Book);
            search.list_result_search.SelectedIndex = 0;
            if (search.ShowDialog().Equals(true))
            {
                List<TextBox> txb = new List<TextBox>() { tb_Name, tb_Cost, tb_Year, tb_check_exemp };
                ViewData.Insert_Data_Tb(
                new ObservLists<ArticulBooks>(db.ArticulBooks.ToList()).List.FirstOrDefault(x => x.Books.name.Equals(Check_Name_Book)),
                db, txb);
                art = Convert.ToInt32(cmbArticul_Books.SelectedItem);
                if (db.BookExtras.FirstOrDefault(x => x.ArticulBooks.articul.Equals(art)) != null)
                    tb_Reader.Text = db.BookExtras.FirstOrDefault(x => x.ArticulBooks.articul.Equals(art)).Readers.fio;
                if (tb_Reader.Text != "")
                    grid_Reader.Visibility = Visibility.Visible;

            }
        }
        /// <summary>
        /// Обработка клика по кнопке "Удалить данного читателя"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteReader_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно хотите удалить данного читателя?", "Удалить?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Readers r = ((Readers)dgReader.SelectedItem);
                db.Database.ExecuteSqlCommand($"Delete From BookExtras Where Readers_id = '{r.id}'");
                db.Database.ExecuteSqlCommand($"Delete From Readers Where id = '{r.id}'");
                ViewData.UpdateTableReader(db, dgReader);
            }
        }
        /// <summary>
        /// В зависимости от выбранного артикула выводятся данные о книге
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbArticul_Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<TextBox> txb = new List<TextBox>() { tb_Name, tb_Cost, tb_Year, tb_check_exemp };
            if (listBook.SelectedValue != null)
                ViewData.Insert_Data_Tb(
                    new ObservLists<ArticulBooks>(db.ArticulBooks.ToList()).List.FirstOrDefault(x => x.Books.name.Equals(Check_Name_Book)),
                    db, txb);
            art = Convert.ToInt32(cmbArticul_Books.SelectedItem);
            if (db.BookExtras.Any(be => be.ArticulBooks.articul == art))
                tb_Reader.Text = db.BookExtras.FirstOrDefault(x => x.ArticulBooks.articul.Equals(art)).Readers.fio;
            else
            {
                tb_Reader.Text = "";
                tb_Date_out.Text = "";
                grid_Reader.Visibility = Visibility.Hidden;
                delete_Book.Visibility = Visibility.Visible;
                btn_Add_Order_Main.Visibility = Visibility.Visible;
            }
            if (tb_Reader.Text != "")
                grid_Reader.Visibility = Visibility.Visible;
            delete_Book.Visibility = Visibility.Visible;
            btn_Add_Order_Main.Visibility = Visibility.Visible;
            if (tb_Date_out.Text != "")
            {

            }
        }
        /// <summary>
        /// В зависимости от читающего выбранную книгу - выводится дата выдачи книги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_Reader_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_Reader.Text.Length > 0)
                tb_Date_out.Text = db.BookExtras.FirstOrDefault(x => x.Readers.fio.Equals(tb_Reader.Text) && x.ArticulBooks.articul == art).
                date_out.ToShortDateString();
        }
        /// <summary>
        /// Обработка клика по кнопке "Удалить книгу"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Book_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно хотите списать данную книгу?", "Удалить?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.Database.ExecuteSqlCommand($"Delete From BookExtras Where ArticulBooks_Id = '{db.ArticulBooks.FirstOrDefault(x => x.articul == art).Id}'");
                db.Database.ExecuteSqlCommand($"Delete From ArticulBooks Where articul = '{art}'");
                var n = db.ArticulBooks.Count(x => x.Books.name == tb_Name.Text);
                if (n == 0)
                    if (MessageBox.Show("Был списан последний экземпляр данной книги. \nЖелаете списать весь тираж?", "Удалить тираж?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        db.Database.ExecuteSqlCommand($"Delete From Books Where name = '{tb_Name.Text}'");

                ViewData.Update_listbook(db, listBook, listAuthor);
                cmbArticul_Books.SelectedIndex = 0;
                tb_Name.Text = "";
                tb_Cost.Text = "";
                tb_Year.Text = "";
                listBook.SelectedIndex = selected_item;
            }
        }
        /// <summary>
        /// Обработка клика по кнопке "Удалить весь тираж"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_AllBooks_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно хотите списать весь тираж?", "Удалить?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.Database.ExecuteSqlCommand($"Delete From BookExtras Where ArticulBooks_Id = '{db.ArticulBooks.FirstOrDefault(x => x.articul == art).Id}'");
                db.Database.ExecuteSqlCommand($"Delete From ArticulBooks Where Books_id = '{db.Books.FirstOrDefault(x => x.name == tb_Name.Text).id}'");
                db.Database.ExecuteSqlCommand($"Delete From Books Where id = '{db.Books.FirstOrDefault(x => x.name == tb_Name.Text).id}'");
                cmbArticul_Books.SelectedIndex = 0;
                ViewData.Update_listbook(db, listBook, listAuthor);
                tb_Name.Text = "";
                tb_Cost.Text = "";
                tb_Year.Text = "";
                grid_Info_Book.Visibility = Visibility.Hidden;
            }
        }
        private void dgReader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewData.Cost_Take_Books(dgrTakeBooks, dgReader, dgrTakeBooks_Result, db);
        }
        private void btn_MakeBook_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Принять указанную книгу?", "Принять?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var make_book = (BookExtras)dgrTakeBooks.SelectedItem; //выбранная книга
                db.Database.ExecuteSqlCommand($"Delete From BookExtras Where id = '{make_book.id}'");
                ViewData.UpdateTableTakeBooks(db, dgrTakeBooks, make_book.Readers.id);
                ViewData.Cost_Take_Books(dgrTakeBooks, dgReader, dgrTakeBooks_Result, db);
            }
        }
        private void dgrMakeBooks_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (((BookExtras)(e.Row.DataContext)).date_get < DateTime.Today)
                e.Row.Background = new SolidColorBrush(Color.FromArgb(85, 207, 91, 91));
        }
        private void dgrTakeBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgrTakeBooks.Items.Count > 0 && dgrTakeBooks.SelectedIndex > -1)
                btn_MakeBook.Visibility = Visibility.Visible;
            else
                btn_MakeBook.Visibility = Visibility.Hidden;
        }

        public static List<Readers> Search_readers(List<Readers> list, string word, bool tel) => list.Where(x => IsGood(x, word, tel)).ToList();
        private static int Find(Readers s, int istart, char c, bool tel)
        {
            var param = !tel ? s.fio.ToCharArray() : s.phone.ToCharArray();
            for (int i = 0; i < param.Length; i++)
                if (c == param[i])
                    return i;
            return -1;
        }
        private static bool IsGood(Readers x, string word, bool tel)
        {
            int res = -1;
            for (int i = 0; i < word.Length; i++)
            {
                res = Find(x, res + 1, word[i], tel);
                if (res == -1)
                    return false;
            }
            return true;
        }

        private void tbSeurchPhoneReader_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgReader.Items.Clear();
            Search_readers(db.Readers.ToList(), tbSeurchPhoneReader.Text, true).ForEach(x => dgReader.Items.Add(x));
        }

        private void tbSeurchFioReader_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgReader.Items.Clear();
            Search_readers(db.Readers.ToList(), tbSeurchFioReader.Text, false).ForEach(x => dgReader.Items.Add(x));
        }
    }
}