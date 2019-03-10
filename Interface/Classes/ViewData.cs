using Interface.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Interface.Classes
{
    public class ViewData
    {
        /// <summary>
        /// Обновление listbox'a с ФИО авторов на вкладке "База авторов и книг"
        /// </summary>
        /// <param name="db">Класс контекста</param>
        /// <param name="listA">listbox с авторами</param>
        public static void UpdateTableAuthor(DiagramContainer db, ListBox listA)
        {
            listA.Items.Clear();
            db.Authors.ToList().OrderBy(x => x.fio).ToList().ForEach(x => listA.Items.Add(x.fio));
        }
        /// <summary>
        /// Обновление datagrid с читателями на вкладке "Читатели"
        /// </summary>
        /// <param name="db">Класс контекста</param>
        /// <param name="dgR">datagrid с читателями</param>
        public static void UpdateTableReader(DiagramContainer db, DataGrid dgR)
        {
            var observ = new ObservLists<Readers>(db.Readers.ToList()).List.ToBindingList();
            foreach (var item in observ)
                dgR.Items.Add(item);
        }
        /// <summary>
        /// Вывод списка книг, взятых указанным пользователем
        /// </summary>
        /// <param name="db">Класс контекста</param>
        /// <param name="dgR">datagrid со взятыми книгами</param>
        /// <param name="idR">id читателя</param>
        public static void UpdateTableTakeBooks(DiagramContainer db, DataGrid dgR, int idR)
        {
            var k = db.BookExtras.Where(x => x.Readers.id == idR).ToList();
            dgR.ItemsSource = new ObservLists<BookExtras>(db.BookExtras.Where(x=>x.Readers.id == idR).ToList()).List.ToBindingList();
        }
        public static void UpdateComboArticul(DiagramContainer db, ComboBox cmb, ListBox lb)
        {
            cmb.Items.Clear(); 
            new ObservLists<ArticulBooks>(db.ArticulBooks.Where(x => x.Books.name.Equals(lb.SelectedItem.ToString()))
                .ToList()).List.ToList().ForEach(x => cmb.Items.Add(x.articul.ToString()));
        }
        /// <summary>
        /// Обновление listbox'a с названиями книг каждого автора на вкладке "База авторов и книг"
        /// </summary>
        /// <param name="db">Класс контекста</param>
        /// <param name="listB">listbox с книгами</param>
        /// <param name="listA">listbox с авторами</param>
        public static void Update_listbook(DiagramContainer db, ListBox listB, ListBox listA)
        {
            listB.Items.Clear();
            new ObservLists<Books>(db.Books.Where(x => x.Authors.fio.Equals(listA.SelectedValue.ToString()) 
                && !x.ArticulBooks.Count.Equals(0)).ToList()).List.OrderBy(x => x.name).ToList().ForEach(x => listB.Items.Add(x.name));
        }
        /// <summary>
        /// Вставка данных в textbox'ы 
        /// </summary>
        /// <param name="books">Инфо о книге</param>
        /// <param name="db">Класс контекста</param>
        /// <param name="list_tb">Список textBox'ов</param>
        public static void Insert_Data_Tb(ArticulBooks books, DiagramContainer db, List<TextBox> list_tb)
        {
                list_tb[0].Text = books.Books.name.ToString();
                list_tb[1].Text = books.Books.cost.ToString();
                list_tb[2].Text = books.Books.date_public.ToString();
                list_tb[3].Text = db.ArticulBooks.Count(x => x.Books.name.Equals(books.Books.name)
                    && !x.BookExtras.ArticulBooks.Books.name.Equals(books.Books.name)).ToString();
        }

        public static void Cost_Take_Books(DataGrid dgrTake, DataGrid dgrReader, DataGrid dgrTake_Result, DiagramContainer db)
        {
            var r = ((Readers)dgrReader.SelectedItem);
            if (r != null)
                ViewData.UpdateTableTakeBooks(db, dgrTake, r.id);
            List<ResultCost> costList = new List<ResultCost> {new ResultCost()};
            if (dgrTake.Items.Count > 0 && dgrReader.SelectedIndex > -1)
            {
                int price_books = db.BookExtras.Where(x => x.Readers.id == r.id).Sum(x => x.cost); //стоимость всех книг выбранного чтателя
                costList[0].column4 = price_books;
            }
            dgrTake_Result.ItemsSource = costList;
        }
    }
}
