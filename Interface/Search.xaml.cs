using Interface.Models;
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
using System.Windows.Shapes;

namespace Interface
{
    public partial class Search : Window
    {
        DiagramContainer db;
        bool check_width = false;
        List<string> listR = new List<string>();
        public int price = 0;
        public int ID_R = 0; //id читателя
        public Search()
        {
            InitializeComponent();
            db = new DiagramContainer();
        }
        private void btnCloseSearch_Click(object sender, RoutedEventArgs e) => Close();
        public static List<string> Search_words(List<string> list, string word) => list.Where(x => IsGood(x, word)).ToList();
        private static int Find(string s, int istart, char c)
        {
            for (int i = istart; i < s.Length; i++)
                if (c == s[i])
                    return i;
            return -1;
        }
        private static bool IsGood(string x, string word)
        {
            int res = -1;
            for (int i = 0; i < word.Length; i++)
            {
                res = Find(x, res + 1, word[i]);
                if (res == -1)
                    return false;
            }
            return true;
        }
        private void btnSearch_Name_Click(object sender, RoutedEventArgs e)
        {
            list_result_search.Items.Clear();
            Search_words(db.Books.Select(x => x.name).ToList(), txtSearch_Name.Text).ForEach(x => list_result_search.Items.Add(x));
            if (list_result_search.Items.Count == 0) // если нет результатов - сообщение об ошибке
                MessageBox.Show("Совпадений не найдено...");
        }
        private void btnSet_Books_Click(object sender, RoutedEventArgs e)
        {
            if (Width == 420 && check_width == false)
            {
                btnSet_Books.Content = "←----";
                for (double i = 0; i < 280; i += 10)
                    Width += 10;
                check_width = true;
                return;
            }
            if (check_width == true)
            {
                btnSet_Books.Content = "Выдать книгу";
                for (double i = 0; i < 280; i += 10)
                    Width -= 10;
                check_width = false;
            }
        }
        private void list_result_search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSet_Books.Visibility = Visibility.Visible;
            dateExtra_Get.Text = "";
            cmbExtraBook.Items.Clear();
            db.ArticulBooks.Where(x => x.Books.name.Equals(list_result_search.SelectedValue.ToString()) && x.BookExtras == null).ToList().ForEach(x => cmbExtraBook.Items.Add(x.articul));
            if (cmbExtraBook.Items.Count == 0)
                MessageBox.Show("Свободных экземпляров указанной книги не найдено!");
        }
        private void btnExtra_Add_Order_Click(object sender, RoutedEventArgs e)
        {  
            if (tb_idReader.Text == "" ||
                dateExtra_Get.Text == "")
                MessageBox.Show("Не все поля заполнены!");
            else if (DateTime.Today > dateExtra_Get.SelectedDate)
                MessageBox.Show("Дата возврата указана неверно!");
            else if (list_InfoReader.Items.Count == 1)
                MessageBox.Show("Данный читатель не обнаружен в базе!");
            else
            {
                ID_R = int.Parse(tb_idReader.Text);
                var r = db.Readers.FirstOrDefault(x => x.id == ID_R);
                var a = db.ArticulBooks
                        .Where(x => x.Books.name.Equals(list_result_search.SelectedValue.ToString()))
                        .FirstOrDefault(x => x.BookExtras == null);
                if (a != null)
                {
                    db.Database.ExecuteSqlCommand($"INSERT INTO BookExtras VALUES (GetDate(), '{dateExtra_Get.SelectedDate.Value.Date}', '{int.Parse(txtExtra_Cost.Text)}', '{r.id}', '{a.Id}')");
                    r.kolvo_books += 1;
                    db.SaveChanges();
                    MessageBox.Show("Успешно. Заказ оформлен!");
                    //DialogResult = true;
                    Check_Success = true;
                    GetNewList = db.Books.ToList();
                }
                else
                    MessageBox.Show("Свободных книг с данным названием не обнаружено!");
                Close();
            }
        }
        public List<Books> GetNewList { get; set; }
        public bool Check_Success { get; set; }
        private void dateExtra_Get_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            int artic = Convert.ToInt32(cmbExtraBook.SelectedItem.ToString());
            var date = dateExtra_Get.SelectedDate - DateTime.Today;
            if(dateExtra_Get.Text.Length > 0)
                txtExtra_Cost.Text = (Math.Ceiling((double)date.Value.Days / 7) * db.ArticulBooks.FirstOrDefault(x => x.articul == artic).Books.cost).ToString();
        }
        private void txtSearch_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            list_result_search.Items.Clear();
            Search_words(db.Books.Select(x => x.name).ToList(), txtSearch_Name.Text).ForEach(x => list_result_search.Items.Add(x));
            if (list_result_search.Items.Count == 0) // если нет результатов - сообщение об ошибке
                list_result_search.Items.Add("Совпадений не найдено...");
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            //int ar = int.Parse(cmbExtraBook.SelectionBoxItem.ToString());
            //MainWindow mainWindow = new MainWindow(db.ArticulBooks.FirstOrDefault(x => x.articul == ar).Books.name);
        }

        private void tb_idReader_TextChanged(object sender, TextChangedEventArgs e)
        {
            list_InfoReader.Items.Clear();
            if (tb_idReader.Text.Length > 0)
            {
                ID_R = int.Parse(tb_idReader.Text);
                if (db.Readers.Any(x => x.id == ID_R))
                {
                    list_InfoReader.Items.Clear();
                    var r = db.Readers.FirstOrDefault(x => x.id == ID_R);
                    ListBoxItem item = new ListBoxItem();
                    item.Content = "Информация о данном читателе:";
                    item.FontWeight = FontWeights.Bold;
                    item.FontSize = 12;
                    list_InfoReader.Items.Add(item);
                    list_InfoReader.Items.Add("ФИО: " + r.fio);
                    list_InfoReader.Items.Add("Телефон: " + r.phone);
                    list_InfoReader.Items.Add("Адрес: " + r.adress);
                }
                else
                {
                    list_InfoReader.Items.Clear();
                    list_InfoReader.Items.Add("Данный читатель не обнаружен в базе...");
                }
            }
        }
        private void cmbExtraBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            int a = int.Parse(cmbExtraBook.SelectedItem.ToString());
            txtExtra_Cost.Text = db.ArticulBooks.FirstOrDefault(x => x.articul == a).Books.cost.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => db.Books.ToList().ForEach(x => list_result_search.Items.Add(x.name));
    }
}