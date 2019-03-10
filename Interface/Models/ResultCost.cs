using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models
{
    public class ResultCost
    {
        public string column1 { get; } = null;
        public string column2 { get; } = null;
        public string column3 { get; } = "ИТОГО (руб.):";
        public int column4 { get; set; } = 0;
    }
}
