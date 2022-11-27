using Disconnected_SQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disconnected_SQL.ViewModels
{
    internal class InsertViewModel:BaseViewModel
    {


        private Author author;

        public Author Author
        {
            get { return author; }
            set { author = value;OnPropertyChanged(); }
        }

        public InsertViewModel()
        {
            Author = new Author();
        }
    }
}
