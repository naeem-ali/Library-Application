using System.Collections.Generic;
using System.ComponentModel;

namespace Library_Application.Models
{
    public class BookModel
    {

        public int ID { get; set; }
        public string Title { get; set; }

        public int Available { get; set; }

        public int Quantity { get; set; }

        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public List<int> Author { get; set; }

    }
}