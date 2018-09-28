using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IslamicSearch.Models
{
    public class Old_refrence
    {
        public int id { get; set; }
        public int vol { get; set; } = 0;
        public int book { get; set; } = 0;
        public int hadith { get; set; } = 0;
    }

    public class In_Book_Refrence
    {
        public int id { get; set; }
        public int book { get; set; } = 0;
        public int hadith { get; set; } = 0;
        public int vol { get; set; } = 0;
        public string tag { get; set; } = "";
    }

    public class HadithModel
    {
        public int id { get; set; }
        public int src { get; set; }
        public int number { get; set; } = 0;
        public string arabicHTML { get; set; }
        public string arabicText { get; set; }
        public string englishHTML { get; set; }
        public string englishText { get; set; }
        public In_Book_Refrence in_book_refrence { get; set; }
        public Old_refrence old_refrence { get; set; }
    }



    public class MuslimHadithModel
    {
        public int id { get; set; }
        public int number { get; set; } = 0;
        public string arabicHTML { get; set; } = "";
        public string arabicText { get; set; } = "";
        public string englishHTML { get; set; } = "";
        public string englishText { get; set; } = "";
        public In_Book_Refrence in_book_refrence { get; set; }
        public Old_refrence old_refrence { get; set; }
    }



    public class BukhariHadithModel
    {
        public int? id { get; set; }
        public int number { get; set; } = 0;
        public string arabicHTML { get; set; }
        public string arabicText { get; set; }
        public string englishHTML { get; set; }
        public string englishText { get; set; }
        public In_Book_Refrence in_book_refrence { get; set; }
        public Old_refrence old_refrence { get; set; }
    }

    public class NasaiHadithModel
    {
        public int id { get; set; }
        public int number { get; set; } = 0;
        public string arabicHTML { get; set; }
        public string arabicText { get; set; }
        public string englishHTML { get; set; }
        public string englishText { get; set; }
        public NasaiIn_Book_Refrence in_book_refrence { get; set; }
        public NasaiOld_refrence old_refrence { get; set; }
        public string grade { get; set; }
    }

    public class NasaiOld_refrence
    {
        public int id { get; set; }
        public int vol { get; set; } = 0;
        public int book { get; set; } = 0;
        public int hadith { get; set; } = 0;
    }

    public class NasaiIn_Book_Refrence
    {
        public int id { get; set; }
        public int? book { get; set; } = 0;
        public int? hadith { get; set; } = 0;
        public int? vol { get; set; } = 0;
        public string tag { get; set; } = "";
    }


    public class HadithRequest
    {
        public int src { get; set; }
        public int? id { get; set; } = null;
        public int? number { get; set; } = null;
        public In_Book_Refrence in_book_refrence { get; set; } = null;
        public Old_refrence old_refrence { get; set; } = null;
    }
}//Namespace