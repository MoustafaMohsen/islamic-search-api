using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using IslamicSearch.Data;
using IslamicSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IslamicSearch.Controllers
{
    [Route("api/[controller]")]
    public class HadithController : Controller
    {
        private AppDbContext db;
        public HadithController ( AppDbContext _db)
        {
            db = _db;
        }


        // GET: api/<controller>
        [HttpGet]
        public List<HadithModel> Get()
        {

            List<HadithModel> model = new List<HadithModel>();
            int NumberOfFiles = 56;
            string ketab = "muslim";
            //Read Folder
            for (int i = 0 ; i < NumberOfFiles + 1; i++)
            {
                //Read File
                var fileName = "Json/"+ ketab + "/"+ i +".json";
                var fileString = JsonfileReader(fileName);
                var hadithObj = JsonConvert.DeserializeObject<List<JMuslimHadithObject>>(fileString);

                for (int i2 = 0; i2 < hadithObj.Count; i2++)
                {
                    var row = MuslimConvertHadithObject(hadithObj[i2]);
                    model.Add(row);
                }
            }

            

            return model;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //take book and Return volume number
        public int muslimVol(int b) { return b < 5 ? 1 : b < 6 ? 2 : b < 9 ? 3 : b < 21 ? 4 : b < 30 ? 5 : b < 40 ? 6 : b < 44 ? 7 : 0; }

        public int stringToInt(string str)
        {
            int x = 0;
            if (str == null)
            {
                return x;
            }
            for (System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"\d+"); match.Success; match = match.NextMatch())
            {
                x = int.Parse(match.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
            }

            return x;
        }

        // To convert the Extracted from the browser to Model
        public HadithModel otherConvertHadithObject(JHadithObject obj)
        {
            var model = new HadithModel();
            model.in_book_refrence = new Models.In_Book_Refrence();
            model.old_refrence = new Models.Old_refrence();
            model.arabicHTML = obj.arabicHTML;
            model.arabicText = obj.arabicText;
            model.englishHTML = obj.englishHTML;
            model.englishText = obj.englishText;
            model.id = obj.id;


            model.number = stringToInt(obj.number);

            model.in_book_refrence.book = stringToInt(obj.in_book_refrence.book);
            model.in_book_refrence.hadith = stringToInt(obj.in_book_refrence.hadith);

            model.old_refrence.vol =   stringToInt(obj.old_refrence.vol);
            model.old_refrence.book = stringToInt(obj.old_refrence.book);
            model.old_refrence.hadith = stringToInt(obj.old_refrence.hadith);


            return model;
        }

        public string JsonfileReader(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                return r.ReadToEnd();
            }
        }

        //Special converter for muslim ketab
        // To convert the Extracted from the browser to Model
        public HadithModel MuslimConvertHadithObject(JMuslimHadithObject obj)
        {
            var model = new HadithModel();
            model.in_book_refrence = new Models.In_Book_Refrence();
            model.old_refrence = new Models.Old_refrence();
            model.arabicHTML = obj.arabicHTML;
            model.arabicText = obj.arabicText;
            model.englishHTML = obj.englishHTML;
            model.englishText = obj.englishText;
            model.id = obj.id;


            model.number = stringToInt(""+obj.number);

            model.in_book_refrence.book = obj.mini_new_refrence.book;
            model.in_book_refrence.hadith = stringToInt(obj.mini_new_refrence.hadith);
            model.in_book_refrence.tag = obj.in_book_refrence;

            model.old_refrence.vol =  muslimVol(stringToInt(obj.old_refrence.book)) ;
            model.old_refrence.book = stringToInt(obj.old_refrence.book);
            model.old_refrence.hadith = stringToInt(obj.old_refrence.hadith);


            return model;
        }

    }//Class

    public class InBookRefrence
    {
        public string book { get; set; }
        public string hadith { get; set; }
    }

    public class OldRefrence
    {
        public string vol { get; set; } = "0";
        public string book { get; set; }
        public string hadith { get; set; }

    }

    //Extracted hadith Object Class
    public class JHadithObject
    {
        public int id { get; set; }
        public string number { get; set; }
        public InBookRefrence in_book_refrence { get; set; }
        public OldRefrence old_refrence { get; set; }
        public string arabicText { get; set; }
        public string arabicHTML { get; set; }
        public string englishText { get; set; }
        public string englishHTML { get; set; }

    }

    //Special case for muslim ketab

    public class MiniNewRefrence
    {
        public int book { get; set; }
        public string hadith { get; set; }
    }
    public class JMuslimHadithObject
    {
        public int id { get; set; }
        public string number { get; set; }
        public string in_book_refrence { get; set; }
        public MiniNewRefrence mini_new_refrence { get; set; }
        public OldRefrence old_refrence { get; set; }
        public string arabicText { get; set; }
        public string arabicHTML { get; set; }
        public string englishText { get; set; }
        public string englishHTML { get; set; }
    }
}//Namespace

/*
            //Validations
            if (!ModelState.IsValid) { return BadRequest(ModelState); }//validate
            if (grocery.Name == "") { return Content("No Name"); }//validate
            if (grocery == null) { return NotFound(); }//validate


            if (req == "add")
            {
                //--------------add logic-------------//
                //PostValidation
                if (GroceryExistsName(grocery.Name)) { return Ok("name already exists"); }//validate

                grocery.MoreInformations = UpdateInformationsAdd(grocery.MoreInformations);

db.Grocery.Add(grocery);
                await db.SaveChangesAsync();

                return Ok("Added");
            }
*/
