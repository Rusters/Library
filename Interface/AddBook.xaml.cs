using Interface.Classes;
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
    public partial class AddBook : Window
    {
        DiagramContainer db;
        public AddBook()
        {
            InitializeComponent();
            db = new DiagramContainer();
            db.Authors.ToList().OrderBy(x => x.fio).ToList().ForEach(x => cmbAddBook_Author.Items.Add(x.fio));
        }
        private void btnAddBook_Add_Click(object sender, RoutedEventArgs e)
        {
            var test = new ObservLists<ArticulBooks>(db.ArticulBooks.ToList()).List;
            foreach (var item in test)
            {
                if (item.articul == int.Parse(txtAddBook_Articul.Text))
                {
                    MessageBox.Show("Ошибка! Книга с таким артикулом уже существует!");
                    return;
                }
                if (item.Books.name == txtAddBook_Name.Text && item.Books.Authors.fio == cmbAddBook_Author.SelectedItem.ToString())
                {
                    db.Database.ExecuteSqlCommand($"INSERT INTO ArticulBooks VALUES ('{int.Parse(txtAddBook_Articul.Text)}', '{item.Books.id}')");
                    MessageBox.Show("Данная коллекция найдена. Экземпляр книги добавлен!");
                    return;
                }
            }
            db.Database.ExecuteSqlCommand($"INSERT INTO Books VALUES ('{txtAddBook_Name.Text}', '{txtAddBook_Public.Text}', '{int.Parse(txtAddBook_Cost.Text)}', '{db.Authors.FirstOrDefault(x => x.fio == cmbAddBook_Author.SelectedItem.ToString()).id}')");
            db.Database.ExecuteSqlCommand($"INSERT INTO ArticulBooks VALUES('{int.Parse(txtAddBook_Articul.Text)}','{db.Books.ToList().Last().id}')");
            MessageBox.Show("Успешно! Добавлена новая книга");
            return;
        }
        private void btnAddBook_Reset_Click(object sender, RoutedEventArgs e)
        {
            cmbAddBook_Author.Text = "";
            txtAddBook_Cost.Text = "";
            txtAddBook_Name.Text = "";
            txtAddBook_Public.Text = "";
            txtAddBook_Articul.Text = "";
        }
        private void btnAddBook_Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
