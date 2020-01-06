using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BookModel
    {
        string Title { get; set; }
        string Author { get; set; }
        string Status { get; set; }

        public BookModel(string title, string author, string status)
        {
            title = Title;
            author = Author;
            status = Status;

        }

    }
}
