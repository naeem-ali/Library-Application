using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Application.Models
{
    public class Book_Author
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int AuthorId { get; set; }

        public int DateCreated { get; set; }
    }
}