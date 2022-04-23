using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Library_Application.Models
{
    public class Author
    {
        
        public int A_ID { get; set; }

        [DisplayName("Author Name")]
        public string A_Name { get; set; }
        
    }
}