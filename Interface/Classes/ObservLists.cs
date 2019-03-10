using Interface.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Classes
{
    public class ObservLists<T> : INotifyPropertyChanged
    {
        public ObservLists() => List = new ObservableCollection<T>();

        /// <summary>
        /// Конструктор с передачей листов 
        /// </summary>
        /// <param name="listR">Список читателей</param>
        public ObservLists(List<T> list) : this() => list.ForEach(x => List.Add(x));

        public ObservableCollection<T> List { get; set; }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                ((INotifyPropertyChanged)List).PropertyChanged += value;
            }

            remove
            {
                ((INotifyPropertyChanged)List).PropertyChanged -= value;
            }
        }
    }
}
