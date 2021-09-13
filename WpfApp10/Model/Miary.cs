using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp10.Model
{
    class Miary
    {
        public int id { get; set; }
        public string typMiary { get; set; }
        public override string ToString()
        {
            return typMiary;
        }
    }
}
