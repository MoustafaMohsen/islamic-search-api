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

        //take book and Return volume number
        public int muslimVol(int b) { return b<5?1:b<6?2:b<9?3:b<21?4:b<30?5:b<40?6:b<44?7:0;}

        public int stringToInt(string str )
        {
            int x = 0;
            for (System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(str, @"\d+"); match.Success; match = match.NextMatch())
            {
                x =  int.Parse(match.Value, System.Globalization.NumberFormatInfo.InvariantInfo); 
            }

            return x;
        }


        public HadithModel ConvertHadithObject(JHadithObject obj,string ketab)
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

            model.in_book_refrence.book = stringToInt( obj.in_book_refrence.book ) ;
            model.in_book_refrence.hadith = stringToInt(obj.in_book_refrence.hadith);

            model.old_refrence.vol = ketab.ToLower()=="muslim"? muslimVol( stringToInt( obj.old_refrence.book ) ): stringToInt(obj.old_refrence.vol);
            model.old_refrence.book = stringToInt( obj.old_refrence.book );
            model.old_refrence.hadith = stringToInt( obj.old_refrence.hadith ) ;

            return model;
        }

        private string  fileReader(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName) )
            {
                 return r.ReadToEnd();
            }
        }

        // GET: api/<controller>
        [HttpGet]
        public List<HadithModel> Get()
        {

            //Json to array 
            var fileName = "1.json";
            var fileString =  fileReader(fileName);
            var hadithObj = JsonConvert.DeserializeObject<List<JHadithObject>>(fileString);

            List<HadithModel> model= new List<HadithModel>();
            for (int i = 0; i < hadithObj.Count; i++)
            {
                var row =ConvertHadithObject(hadithObj[i],"bukhari");
                model.Add(row);
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
