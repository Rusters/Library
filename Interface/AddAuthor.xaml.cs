using Interface.Classes;
using Interface.Models;
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

namespace Interface
{
    public partial class AddAuthor : Window
    {
        DiagramContainer db;
        public AddAuthor()
        {
            InitializeComponent();
            db = new DiagramContainer();
        }
        private void btnAddBook_Reset_Click(object sender, RoutedEventArgs e)
        {
            txtAddAuthor_fio.Text = "";
            txtAddAuthor_year.Text = "";
        }
        private void btnAddBook_Add_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in new ObservLists<Authors>(db.Authors.ToList()).List)
                if (item.fio == txtAddAuthor_fio.Text)
                {
                    MessageBox.Show("Данный автор уже существует!");
                    return;
                }
            db.Authors.Add(new Authors
            {
                fio = txtAddAuthor_fio.Text,
                year_of_life = txtAddAuthor_year.Text,
            });
            db.SaveChanges();
            MessageBox.Show("Данные о новом авторе - добавлены!");
            AddA = true;
            DialogResult = true;
            Close();
        }
        private void btnAddBook_Close_Click(object sender, RoutedEventArgs e) => Close();
        public bool AddA { get; set; }
    }
}
