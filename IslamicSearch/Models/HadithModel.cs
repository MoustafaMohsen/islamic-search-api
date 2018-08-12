using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IslamicSearch.Models
{
    public class Old_refrence
    {
        public int vol { get; set; } = 0;
        public int book { get; set; } = 0;
        public int hadith { get; set; } = 0;
    }

    public class In_Book_Refrence
    {
        public int book { get; set; } = 0;
        public int hadith { get; set; } = 0;
        public string tag { get; set; } = "";
    }

    public class HadithModel
    {
        public int id { get; set; }
        public int number { get; set; } = 0;
        public string arabicHTML { get; set; }
        public string arabicText { get; set; }
        public string englishHTML { get; set; }
        public string englishText { get; set; }
        public In_Book_Refrence in_book_refrence { get; set; }
        public Old_refrence old_refrence { get; set; }
    }


}
