using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace IslamicSearch.Models.Collections
{
    public class ReferenceDefinitions
    {
        public int id { get; set; }
        public string isPrimary { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string valuePrefix { get; set; }
        public List<Astring> parts { get; set; }
    }


    public class Reference
    {
        public int id { get; set; }
        public string code { get; set; }
        public string suffix { get; set; }
        public List<Astring> parts { get; set; }
    }


    public class Astring
    {
        public int id { get; set; }
        public string astring { get; set; }
    }

    public class VerseReference
    {
        public int id { get; set; }
        public string chapter { get; set; }
        public string firstVerse { get; set; }
        public string lastVerse { get; set; }
    }

    public class Hadith
    {
        public int id { get; set; }
        public List<Reference> references { get; set; }
        public List<Astring> arabic { get; set; }
        public List<Astring> english { get; set; }
        public List<VerseReference> verseReferences { get; set; }
        public int src { get; set; }
        public int number { get; set; }
    }

    public class Hadiths
    {
        public int id { get; set; }
        public List<Hadith> hadith { get; set; }
    }

    public class HadithCollection
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string copyright { get; set; }
        public List<ReferenceDefinitions> referenceDefinitions { get; set; }
        public Hadiths hadiths { get; set; }
    }

}

//=============================TEST++++++++======================
/*
namespace IslamicSearch.Models.Lib2
{
    public class Value
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Defenition
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Value> value { get; set; }
    }

    public class Block
    {
        public int id { get; set; }
        public List<Value> references { get; set; }
        public List<Value> content { get; set; }
        public List<Value> Sources { get; set; }
    }

    public class Book
    {
        public int id { get; set; }
        public List<Defenition> Defenitions { get; set; }
        public List<Block> blocks { get; set; }
    }
}*/

//===================================== Hadith Books ==================================//
namespace IslamicSearch.Models.Lib3
{
    [NotMapped]
    public class MyProperty
    {
        public string Name { get; set; } = "";
        public int Vol { get; set; } = 0;
        public int Book { get; set; } = 0;
        public int Hadith { get; set; } = 0;
        public string type { get; set; } = "";
        public string TagChar { get; set; } = "";
        public int TagNumber { get; set; } = 0;


    }

    [NotMapped]
    public class IncomingRequest
    {
        public int src { get; set; }
        public int Method { get; set; }
        public string Refrencetype { get; set; } = "";
        public string name { get; set; } = "";
        public int value1 { get; set; } = 0;
        public int value2 { get; set; } = 0;
        public int value3 { get; set; } = 0;
        public int value4 { get; set; } = 0;
        public string tag1 { get; set; } = "";
        public string tag2 { get; set; } = "";
        public string source { get; set; } = "hadith";
        public string url { get; set; } = "";
        public string lang { get; set; } = "";
    }
    public class Value
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }


    public class Refrence
    {
        public int id { get; set; }
        public string name { get; set; }
        public string Refrencetype { get; set; }
        public int value1 { get; set; } = -2;
        public int value2 { get; set; } = -2;
        public int value3 { get; set; } = -2;
        public int value4 { get; set; } = -2;
        public string tag1 { get; set; } = "";
        public string tag2 { get; set; } = "";


        /*
        [NotMapped]
        public MyProperty myProperty
        {
            get {
                if (Refrencetype == "vol book hadith")
                {
                    MyProperty obj = new MyProperty()
                    { Name = name, Vol = value1, Book = value2, Hadith = value3, type = Refrencetype };
                    
                    return obj;
                }

                if (Refrencetype == "book hadith")
                {
                    MyProperty obj = new MyProperty() { Name = name, Book = value1, Hadith = value2, type = Refrencetype };
                    return obj;
                }

                if (Refrencetype == "hadith")
                {
                    MyProperty obj = new MyProperty() { Name = name, Hadith = value1 , type = Refrencetype };
                    return obj;
                }

                if (Refrencetype == "tag")
                {
                    MyProperty obj = new MyProperty() { Name = name, TagNumber = value4, TagChar = tag1 , type = Refrencetype };
                    return obj;
                }


                return null;
            }
        }
        */
    }

    public class HadithBlocks
    {
        public int id { get; set; }
        public List<Refrence> Refrences { get; set; }
        public List<Value> content { get; set; }
        public List<Value> sources { get; set; }
        public int src { get; set; }
        public int number { get; set; }
    }



    public class hadithRefrenceMap
    {
        public int identifier { get; set; }
        public int number { get; set; }
        public List<ChIndex> vch { get; set; }
        public List<Value> tag { get; set; }
        public int src { get; set; }

    }
    public class ChIndex
    {
        public string name { get; set; }
        public int vol { get; set; }
        public int ch { get; set; }
        public int had { get; set; }
    }
}