using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disconnected_SQL.ViewModels
{
    internal class DeleteViewModel:BaseViewModel
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }



    }
}
