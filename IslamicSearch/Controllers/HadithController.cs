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
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IslamicSearch.Controllers
{
    [Route("api/[controller]/")]
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

            /*
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
            */
            List<HadithModel> model = new List<HadithModel>() {
                new HadithModel() { id = 0, arabicText = "test hadith" }
            };
        
            return model;
        }

        // GET api/<controller>/5
        [HttpGet("up")]
        public HadithModel UpdateDatabase()
        {


            //read muslim file to var
            List<HadithModel> model = new List<HadithModel>();
            int NumberOfFiles = 51;
            string ketab = "nasai";
            int boks;
            int rowss;
            //Read Folder
            for (int i = 1; i < NumberOfFiles + 1; i++)
            {
                //Read File
                var fileName = "Json/" + ketab + "/" + i + ".json";
                var fileString = JsonfileReader(fileName);
                //var hadithObj = JsonConvert.DeserializeObject<List<HadithModel>>(fileString);
                var hadithObj = JsonConvert.DeserializeObject<List<NasaiHadithModel>>(fileString);
                for (int i2 = 0; i2 < hadithObj.Count; i2++)
                {
                    var in_book_refr = new In_Book_Refrence()
                    {
                        hadith = (int)hadithObj[i2].in_book_refrence.hadith,
                        book = (int)hadithObj[i2].in_book_refrence.book,
                        vol = (int)hadithObj[i2].in_book_refrence.vol,
                        tag = hadithObj[i2].grade
                    };

                    var Old__refr = new Old_refrence()
                    {
                        hadith = !isNull(hadithObj[i2].old_refrence.hadith) ? (int)hadithObj[i2].old_refrence.hadith : -1,
                        book = !isNull(hadithObj[i2].old_refrence.book) ? (int)hadithObj[i2].old_refrence.book : -1,
                        vol = !isNull(hadithObj[i2].old_refrence.vol) ? (int)hadithObj[i2].old_refrence.vol : -1,

                    };

                    var nasai = new HadithModel()
                    {
                        id = hadithObj[i2].id,
                        number = hadithObj[i2].number,
                        arabicHTML = hadithObj[i2].arabicHTML,
                        arabicText = hadithObj[i2].arabicText,
                        englishHTML = hadithObj[i2].englishHTML,
                        englishText = hadithObj[i2].englishText,
                        old_refrence = Old__refr,
                        in_book_refrence = in_book_refr,

                    };

                    var row = nasai;
                    row.src = 3;
                    model.Add(row);
                    db.HadithModel.Add(row);

                    rowss = i2;
                }
                boks = i;
            }




            db.SaveChanges();
            return db.HadithModel.FirstOrDefault();

            return null;
        }
        // GET api/<controller>/5
        [HttpGet("tag")]
        public List<tagindex> getTags()
        {
            var model = db.HadithModel.Select(a =>
                     new HadithModel
                     {
                         src = a.src,
                         id = a.id,
                         arabicText = a.arabicText,
                         arabicHTML = a.arabicHTML,
                         englishText = a.englishText,
                         englishHTML = a.englishHTML,
                         number = a.number,
                         in_book_refrence = a.in_book_refrence,
                         old_refrence = a.old_refrence,
                     }
                ).ToList();

            model = model.Where(m => m.src == 2).ToList();
            List<tagindex> tagindex = new List<tagindex>() { };
            foreach (var item in model)
            {
                if (!isNull(item.in_book_refrence))
                {
                    tagindex tag = new tagindex()
                    {
                        tag = item.in_book_refrence.tag
                    };
                    tagindex.Add(tag);
                }
            }
            return tagindex;

        }//gettag

        // GET api/<controller>/5
        [HttpGet("newb")]
        public bukhariindex newb()
        {
            var model = db.HadithModel.Select(a =>
                     new HadithModel
                     {
                         src = a.src,
                         id = a.id,
                         //arabicText = a.arabicText,
                         //arabicHTML = a.arabicHTML,
                         //englishText = a.englishText,
                        // englishHTML = a.englishHTML,
                        // number = a.number,
                         in_book_refrence = a.in_book_refrence,
                         old_refrence = a.old_refrence,
                     }
                ).ToList();

            model = model.Where(
                m => m.src == 2
                ).ToList();
            



            //count each chapter in new
            List<bnewIndex> newindx = new List<bnewIndex>() { };            
            for (int i = 1; i < 100; i++)
            {
                if ( 
                    isNullOr0( model.Where(m => m.in_book_refrence.book == i).Count() ) 
                    )
                {
                    break;
                }

                var nhd = 0;
                //get all hadiths in i chapter
                var chaptermodel = model.Where(m => m.in_book_refrence.book == i).ToList();

                //get biggest hadith number and save it to nhd
                foreach (var hadith in chaptermodel)
                {
                    if (hadith.in_book_refrence.hadith >= nhd)
                    {
                        nhd = hadith.in_book_refrence.hadith;
                    }
                }

                //add this chapter index to the list of newixdex
                var bnew = new bnewIndex() { nc = i, nh = nhd };
                newindx.Add(bnew);
            }


            //count in old refrence 
            List<boldIndex> oldindx = new List<boldIndex>() { };
            for (int i = 1; i < 100; i++)
            {
                if (isNullOr0( model.Where(m => m.old_refrence.book == i).Count() ) )
                {
                    break;
                }

                var ohd = 0;
                //get all hadiths in i chapter
                var chaptermodel = model.Where(m => m.old_refrence.book == i).ToList();

                //get biggest hadith number and save it to nhd
                foreach (var hadith in chaptermodel)
                {
                    if (hadith.old_refrence.hadith >= ohd)
                    {
                        ohd = hadith.old_refrence.hadith;
                    }
                }

                //add this chapter index to the list of newixdex
                var boldIndex = new boldIndex() { oc = i, oh = ohd };
                oldindx.Add(boldIndex);
            }

            bukhariindex bindex = new bukhariindex() {bnew= newindx, bold= oldindx };
            return bindex;

        }//gettag

        public bool IndexExsists(HadithModel hadithModel, List<bnewIndex> newindx)
        {
            bool nonexsist = false;
            foreach (var item in newindx)
            {
                if (
                    item.nc == hadithModel.in_book_refrence.book
                    )
                {
                    nonexsist = true;
                }
            }
            return nonexsist;
        }
        public bool IndexExsists(HadithModel hadithModel, List<boldIndex> oldindx)
        {
            bool nonexsist = false;
            foreach (var item in oldindx)
            {
                if (
                    item.oc == hadithModel.old_refrence.book
                    )
                {
                    nonexsist = true;
                }
            }
            return nonexsist;
        }

        // GET api/<controller>/5
        [HttpGet("mod")]
        public HadithModel modDatabase()
        {
            

            var queryable = db.HadithModel.Select(a =>
                     new HadithModel
                     {
                         src = a.src,
                         id = a.id,
                         arabicText = a.arabicText,
                         arabicHTML = a.arabicHTML,
                         englishText = a.englishText,
                         englishHTML = a.englishHTML,
                         number = a.number,
                         in_book_refrence = a.in_book_refrence,
                         old_refrence = a.old_refrence,
                     }
                );

            foreach (var item in queryable)
            {
                if (item.src == 2)
                {
                    var newtag = cleanwhitespaces(item.in_book_refrence.tag);
                    var editItem = db.HadithModel.FirstOrDefault(m => m.id == item.id);
                    editItem.in_book_refrence.tag = newtag;
                    db.Entry(editItem).State = EntityState.Modified;

                }
            }




            db.SaveChanges();
            return db.HadithModel.FirstOrDefault();

            return null;
        }

        public async void edittag(HadithModel item)
        {

        }

        public string cleanwhitespaces(string srt)
        {
            return Regex.Replace(srt, @"\s+", "");
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ActionResult<HadithModel> GetById(int id)
        {
            HadithModel hadith = null;
            return hadith;
        }


        // POST api/<controller>
        [HttpPost("request/{source}")]
        public ActionResult<HadithModel> Post([FromBody] HadithRequest request,string source)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            HadithModel hadith = null;

            if (source == "hadith")
            {
                var queryable = db.HadithModel.Select(a =>
                     new HadithModel
                     {
                         src = a.src,
                         id = a.id,
                         arabicText = a.arabicText,
                         arabicHTML = a.arabicHTML,
                         englishText = a.englishText,
                         englishHTML = a.englishHTML,
                         number = a.number,
                         in_book_refrence = a.in_book_refrence,
                         old_refrence = a.old_refrence,
                     }
                );
                hadith = searchRequest(request,queryable);
                //hadith = queryable.FirstOrDefault(m => m.src == request.src && m.number == request.number );
                /*
                hadith = queryable.FirstOrDefault(
                m => m.src == request.src &&
                m.in_book_refrence.book == request.in_book_refrence.book &&
                m.in_book_refrence.hadith == request.in_book_refrence.hadith);
                */
            }


            if (hadith == null)
            {
                return NotFound();
            }

            return hadith;
        }




        [HttpPost("test")]
        public ActionResult<string> PostTEST([FromQuery]HadithRequest request)
        {
            return "sucess";
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


        public HadithModel searchRequest(HadithRequest request, IQueryable<HadithModel> queryable)
        {
            HadithModel hadith= new HadithModel { };
            string number = !isNullOr0(request.number)? "number" : "";
            string newCh = !isNullOr0(request.in_book_refrence.book) && !isNullOr0(request.in_book_refrence.hadith) ? "newCh" : "";
            string newVol = !isNullOr0(request.in_book_refrence.book) && !isNullOr0(request.in_book_refrence.hadith) && !isNullOr0(request.in_book_refrence.vol) ? "newVol" : "";
            string oldCh = !isNullOr0(request.old_refrence.book) && !isNullOr0(request.old_refrence.hadith) ? "oldCh" : "";
            string oldVol = !isNullOr0(request.old_refrence.book) && !isNullOr0(request.old_refrence.hadith) && !isNullOr0(request.old_refrence.vol) ? "oldVol" : "";
            string tag = !isNullOr0(request.in_book_refrence.tag) ? "tag" : "";
            List<string> Selectors = new List<string> {
                number,newCh,newVol,oldCh, oldVol, tag 
            };
            foreach (var selector in Selectors)
            {   
               var Break = false; 
                switch (selector)
                {
                    case "number":
                        { hadith = queryable.FirstOrDefault(m => m.number == request.number && m.src == request.src); Break = true; }
                        break;
                    case "newCh":
                        { hadith = queryable.FirstOrDefault(m => m.src == request.src && m.in_book_refrence.book == request.in_book_refrence.book && m.in_book_refrence.hadith == request.in_book_refrence.hadith); Break = true; }
                        break;
                    case "newVol":
                        { hadith = queryable.FirstOrDefault(m => m.src == request.src && m.in_book_refrence.vol == request.in_book_refrence.vol && m.in_book_refrence.book == request.in_book_refrence.book && m.in_book_refrence.hadith == request.in_book_refrence.hadith); Break = true; }
                        break;
                    case "oldCh":
                        { hadith = queryable.FirstOrDefault(m => m.src == request.src && m.old_refrence.book == request.old_refrence.book && m.old_refrence.hadith == request.old_refrence.hadith); Break = true; }
                        break;
                    case "oldVol":
                        { hadith = queryable.FirstOrDefault(m => m.src == request.src && m.old_refrence.vol == request.old_refrence.vol && m.old_refrence.book == request.old_refrence.book && m.old_refrence.hadith == request.old_refrence.hadith); Break = true; }
                        break;
                    case "tag":
                        {
                            hadith = queryable.Where(m => equaltag(m.in_book_refrence.tag,request.in_book_refrence.tag)).FirstOrDefault();
                            Break = true; }
                        break;


                    default:
                        break;
                }
                if(Break) break;
            }

            return hadith;
        }

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

        private bool isNull<T>(T Object)
        {
            if (Object == null)
            {
                return true;
            }
            return false;
        }

        private bool isNullOr0<T>(T Object)
        {

            if (Object == null || Object.Equals(0) )
            {
                return true;
            }
            return false;
        }

        public bool equaltag(string rg,string val)
        {
            //string pattern = @"(\d+[a-z])";
            string pattern = @"(\d+[a-z])|(\d+)";
            var re = Regex.Matches(rg, pattern);
            foreach (var item in re)
            {
                if (item.ToString() == val)
                {
                    return true;
                }
            }
            return false;
        }
    }//Class

    internal class RestRequest
    {
        private object gET;

        public RestRequest(object gET)
        {
            this.gET = gET;
        }
    }

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



    public class SelectClass
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

    public class tagindex
    {
        public string tag { get; set; }
    }

    public class bnewIndex
    {
        public int nh { get; set; }
        public int nc { get; set; }
    }
    public class boldIndex
    {
        public int oh { get; set; }
        public int oc { get; set; }
    }
    public class bukhariindex
    {
        public List<bnewIndex> bnew { get; set; }
        public List<boldIndex> bold { get; set; }
    }
    public class muslimindex
    {
        public int nh { get; set; }
        public int nc { get; set; }
        public int oh { get; set; }
        public int oc { get; set; }
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

/*
List<HadithModel> model = new List<HadithModel>();

//Read File
var fileName = "Json/.Bukhari.modelJson";
var fileString = JsonfileReader(fileName);
var hadithObj = JsonConvert.DeserializeObject<List<JMuslimHadithObject>>(fileString);

//convert json to hadith
for (int i2 = 0; i2 < hadithObj.Count; i2++)
{
var row = MuslimConvertHadithObject(hadithObj[i2]);
model.Add(row);

}
*/
