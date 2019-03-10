using Interface.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
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
    public partial class ListOfDeptors : Window
    {
        DiagramContainer db;
        List<List> t = new List<List>();
        public ListOfDeptors()
        {
            InitializeComponent();
            db = new DiagramContainer();
            Deptors();
        }

        public static List<Debtor> Search_words(List<Debtor> list, string word) => list.Where(x => IsGood(x, word)).ToList();
        private static int Find(Debtor s, int istart, char c)
        {
            var fio = s.fio.ToCharArray();
            for (int i = istart; i < fio.Length; i++)
                if (c == fio[i])
                    return i;
            return -1;
        }
        private static bool IsGood(Debtor x, string word)
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

        public void Deptors()
        {
            dgDeptors.Visibility = Visibility.Visible;
            var f = db.BookExtras.Select(x => new Debtor { fio = x.Readers.fio, get_books = x.ArticulBooks.Books.name, debt = EntityFunctions.DiffDays(x.date_get, DateTime.Now), pho = x.Readers.phone }).Where(x => x.debt > 0);
            foreach (var d in f)
                dgDeptors.Items.Add(new Debtor { fio = d.fio, get_books = d.get_books, debt = d.debt, pho = d.pho });
        }
        private void Button_Click(object sender, RoutedEventArgs e) => Close();

        private void tbSearchDebtor_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Debtor> deptorsList = new List<Debtor>();
            var f = db.BookExtras.Select(x => new Debtor { fio = x.Readers.fio, get_books = x.ArticulBooks.Books.name, debt = EntityFunctions.DiffDays(x.date_get, DateTime.Now), pho = x.Readers.phone }).Where(x => x.debt > 0);
            foreach (var d in f)
                deptorsList.Add(new Debtor { fio = d.fio, get_books = d.get_books, debt = d.debt, pho = d.pho });
            dgDeptors.Items.Clear();
            Search_words(deptorsList, tbSearchDebtor.Text).ForEach(x => dgDeptors.Items.Add(x));
        }
    }
}
