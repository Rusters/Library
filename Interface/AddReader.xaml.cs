using Interface.Classes;
using Interface.Models;
using System;
using System.Collections.Generic;
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
    public partial class AddReader : Window
    {
        DiagramContainer db;
        //List<Readers> LReader = new List<Readers>();
        public AddReader()
        {
            InitializeComponent();
            db = new DiagramContainer();
        }
        private void btnInsertReader_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in new ObservLists<Readers>(db.Readers.ToList()).List)
                if (item.fio == txtAddReader_fio.Text)
                {
                    MessageBox.Show("Данный читатель уже существует!");
                    return;
                }
            db.Readers.Add(new Readers
            {
                fio = txtAddReader_fio.Text,
                adress = txtAddReader_adress.Text,
                phone = txtAddReader_phone.Text,
                date_reg = DateTime.Today
            });
            db.SaveChanges();
            MessageBox.Show("Данные о читателе - добавлены!");
            AddR = true;
            DialogResult = true;
            Close();
        }
        public bool AddR { get; set; }
    }
}
