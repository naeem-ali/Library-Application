using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Application.Models
{
    public class RentBook

    {
       public int ID { get; set; }

        public int U_ID { get; set; }

        public int B_ID { get; set; }

        public string Title { get; set; }

        public int BookReturn { get; set; }
    }
}