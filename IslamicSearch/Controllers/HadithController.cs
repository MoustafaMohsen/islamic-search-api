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
using System.Xml;
using IslamicSearch.Models.Collections;
using IslamicSearch.Models.Lib3;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IslamicSearch.Controllers
{
    [Route("api/[controller]/")]
    public class HadithController : Controller
    {
        private AppDbContext db;
        public HadithController(AppDbContext _db)
        {
            db = _db;
        }

        // GET: api/<controller>
        [HttpGet("")]
        public async Task<ActionResult> Get()
        {
            /*
            var model =db.HadithBlocks.Select(
                block => new HadithBlocks()
                {
                    content = block.content,
                    id = block.id,
                    Refrences = block.Refrences,
                    sources = block.sources,
                    src = block.src
                }
                );
            var test = await model.FirstOrDefaultAsync();
            */
            return Content("Welcome");
        }

        //Request Hadith by src and id
        [HttpGet("id/{src}/{id}")]
        public ActionResult<HadithBlocks> GetByidAndSrc(int id, int src)
        {
            var queryable = db.HadithBlocks.Select(
                block => new HadithBlocks()
                {
                    content = block.content,
                    id = block.id,
                    Refrences = block.Refrences,
                    sources = block.sources,
                    src = block.src
                }
                );
            var model = queryable.FirstOrDefault(b => b.id == id && b.src == src);
            return model;
        }

        //Request Hadith using Request Object
        [HttpPost("requestb/")]
        public async Task<ActionResult<HadithBlocks>> GetByRequest([FromBody] IncomingRequest request)
        {
            var queryable = db.HadithBlocks.Select(
                    block => new HadithBlocks()
                    {
                        content = block.content,
                        id = block.id,
                        Refrences = block.Refrences,
                        sources = block.sources,
                        src = block.src,
                        number = block.number
                    }
                );

            var model = await SearchHadiths(queryable, request);

            if (isNull(model))
                return NotFound();

            return Ok(model);
        }

        [HttpPost("requestbs/")]
        public ActionResult<HadithBlocks> GetManyByRequest([FromBody] IncomingRequest request)
        {
            var queryable = db.HadithBlocks.Select(
                    block => new HadithBlocks()
                    {
                        content = block.content,
                        id = block.id,
                        Refrences = block.Refrences,
                        sources = block.sources,
                        src = block.src,
                        number = block.number
                    }
                );
            List<HadithBlocks> list = new List<HadithBlocks>();
            if (request.Method == 5)
            {

                list = queryable.Where(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.Refrencetype == request.Refrencetype &&
                                            refr.value4 == request.value4 //&&
                                                                          //refr.tag1 == request.tag1
                                        )
                            )
                            .OrderBy(o => o.number)
                            .ToList()
                ;
            }

            if (isNullOr0(list) || isNullOr0(list.Count))
                return NotFound();

            return Ok(list);
        }


        //===================Using Json only================
        [HttpGet("id/json/{src}/{id}")]
        public ActionResult<HadithBlocks> GetByidAndSrcjson(int id, int src)
        {
            var Blocks = ReadJsonObject<List<HadithBlocks>>("Json/Hadiths/fulldb/bukhari_musli_fulldb.json");

            var model = Blocks.FirstOrDefault(b => b.id == id && b.src == src);
            return model;
        }

        [HttpPost("requestbs/json/")]
        public ActionResult<HadithBlocks> GetManyByRequestjson([FromBody] IncomingRequest request)
        {
            var Blocks = ReadJsonObject<List<HadithBlocks>>("Json/Hadiths/fulldb/bukhari_musli_fulldb.json");

            List<HadithBlocks> list = new List<HadithBlocks>();
            if(request.Method == 5)
            {

            list = Blocks.Where(
                            b =>
                                b.src == request.src
                                &&
                                b.Refrences.Any(
                                    refr =>
                                        refr.Refrencetype == request.Refrencetype &&
                                        refr.value4 == request.value4 //&&
                                                                      //refr.tag1 == request.tag1
                                    )
                        )
                        .OrderBy(o => o.number)
                        .ToList()
            ;
            }

            if (isNullOr0(list) ||isNullOr0(list.Count))
                return NotFound();

            return Ok(list);
        }

        //Request Hadith using Request Object
        [HttpPost("requestb/json/")]
        public  ActionResult<HadithBlocks> GetByRequestjson([FromBody] IncomingRequest request)
        {
            var Blocks = ReadJsonObject<List<HadithBlocks>>("Json/Hadiths/fulldb/bukhari_musli_fulldb.json");


            var model = SearchHadithsList(Blocks, request);

            if (isNull(model))
                return NotFound();

            return Ok(model);
        }


        //=================Preparing Methods===================//
        /*
        //download Sorted Hadiths
        [HttpGet("Sorted/{src}")]
        public ActionResult SortHadith(int src)
        {
            var queryable = db.HadithBlocks.Select(
                block => new HadithBlocks()
                {
                    content = block.content,
                    id = block.id,
                    Refrences = block.Refrences,// = block.Refrences,
                    sources = block.sources,
                    src = block.src
                }
                );
            var models = queryable.Where(
                            b =>
                                b.src == src
                                &&
                                b.Refrences.Any()
                        ).ToList();
            //new Refrence() {value1=r.value1 }
            var name = "USC-MSA";
            var ordered = models
                .OrderBy(o => o.Refrences.First(r=> r.name== name).value1 )
                .ThenBy(o => o.Refrences.First(r => r.name == name).value2)
                .ThenBy(o => o.Refrences.First(r => r.name == name).value3)
                ;
            var list = ordered.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    for (int i2 = 0; i2 < list[i].Refrences.Count; i2++)
                    {
                        var pastitem = list[i - 1];
                        var item = list[i];
                        var pastRedrence = list[i-1].Refrences.Where( r=> r.name == name).First();
                        var refrence = list[i].Refrences.Where(r => r.name == name).First();//[i2];
                       // if (refrence.name == name)
                        {
                                if (pastRedrence.value3 == refrence.value3)
                                    //if(pastRedrence.tag1 == refrence.tag1)
                                        throw null;
                        }
                    }
                }
            }


            return Ok(StringfyObject( ordered ));
        }
        */
        //Backup Database
        [HttpGet("DownloadDb")]
        public async Task<ActionResult<HadithBlocks>> DownloadDb()
        {
            var queryable = db.HadithBlocks.Select(
                    block => new HadithBlocks()
                    {
                        content = block.content,
                        id = block.id,
                        Refrences = block.Refrences,
                        sources = block.sources,
                        src = block.src,
                        number = block.number
                    }
                );

            var model = queryable
                //.Where(x => x.Refrences.Any()).Where(x => x.content.Any()).Where( x => x.sources.Any() )
                .ToList()
                ;

            if (isNull(model))
                return NotFound();

            return Ok(model);
        }
        [HttpGet("getadresses/{src}")]
        public string GetAdress(int src)
        {
            var queryable = db.HadithBlocks.Select(
                    block => new HadithBlocks()
                    {
                        content = block.content,
                        id = block.id,
                        Refrences = block.Refrences,
                        sources = block.sources,
                        src = block.src,
                        number = block.number
                    }
                )
                .Where(x => x.src == src)
                ;

            var name1 = "In-Book";
            var name2 = "USC-MSA";
            var model = queryable.SelectMany(x => x.Refrences).ToList();

            var newChapters = model.Where(x => x.name== name1)
                .OrderBy(o=>o.value1)
                .ThenBy(o => o.value2)
                .ToList()
                ;


            var oldChapters = model.Where(x => x.name == name2)
                .OrderBy(o=>o.value1)
                .ThenBy(o => o.value2)
                //.ThenBy(o => o.value3)
                .ToList()
                ;

            var newList = new List<(int, int)>();
            for (int i = -3; i < 110 ; i++)
            {
                var chapterCollec = newChapters.Where(x => x.value1 == i).ToList();
                if (chapterCollec.Any()  )
                {
                    var element = newChapters[i];
                    var ( chapter , lastHaith ) = (i, chapterCollec.Last().value2);
                    newList.Add((chapter, lastHaith));

                }
            }

            var oldList = new List<(int, int, int)>();
            for (int i = -3; i < 110; i++)
            {
                var chapterCollec = oldChapters.Where(x => x.value1 == i).ToList();
                if (chapterCollec.Any())
                {
                    var element = newChapters[i];
                    var (chapter, firstHadith, lastHaith) = (i, chapterCollec.First().value2, chapterCollec.Last().value2);
                    oldList.Add((chapter, firstHadith, lastHaith));

                }
            }
            
            return StringfyObject(new { newList,oldList });
        }

        /*
        //Upload to Database
        //[HttpGet("newup/{filename}/")]
        public List<HadithBlocks> ShowData(string filename)
        {
            var hadithObj = ReadJsonObject<HadithCollection>("Json/Hadiths/" + filename);
            var hadiths = new List<Hadith>();
            var BlockCollection = new List<HadithBlocks>();

            for (int i = 0; i < hadithObj.hadiths.hadith.Count; i++)
            {
                var hadith = hadithObj.hadiths.hadith[i];

                //Add To HadithBlock

                var contentList = new List<Value>();
                int countar = 1;
                foreach (var item in hadith.arabic)
                {
                    var content = new Models.Lib3.Value
                    {
                        name = "ar:" + countar,
                        value = item.astring
                    };
                    contentList.Add(content);
                    countar++;
                }

                int counteng = 1;
                foreach (var item in hadith.english)
                {
                    var content = new Models.Lib3.Value
                    {
                        name = "en:" + counteng,
                        value = item.astring
                    };
                    contentList.Add(content);
                    counteng++;
                }

                var Refrences = new List<Models.Lib3.Refrence>();
                foreach (var Hadithrefrence in hadith.references)
                {
                    var refrence = new Models.Lib3.Refrence();

                    //only if bukhari
                    if (false)
                        switch (Hadithrefrence.code)
                        {
                            case "DarusSalam":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value1 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.Refrencetype = "hadith";
                                    break;
                                }

                            case "In-Book":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value1 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.value2 = Int32.Parse(Hadithrefrence.parts[1].astring);
                                    refrence.Refrencetype = "book hadith";
                                    break;
                                }

                            case "USC-MSA":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value1 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.value2 = Int32.Parse(Hadithrefrence.parts[1].astring);
                                    refrence.value3 = Int32.Parse(Hadithrefrence.parts[2].astring);
                                    refrence.Refrencetype = "vol book hadith";
                                    break;
                                }

                            default:
                                throw null;
                                break;
                        }

                    //only if Muslim
                    if (false)
                        switch (Hadithrefrence.code)
                        {
                            case "In-Book":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value1 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.value2 = Int32.Parse(Hadithrefrence.parts[1].astring);
                                    refrence.Refrencetype = "book hadith";
                                    break;
                                }

                            case "Reference":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value4 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.tag1 = Hadithrefrence.suffix;
                                    refrence.Refrencetype = "muslim tag";
                                    break;
                                }

                            case "USC-MSA":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value1 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.value2 = Int32.Parse(Hadithrefrence.parts[1].astring);
                                    refrence.Refrencetype = "book hadith";
                                    break;
                                }

                            default:
                                throw null;
                                break;
                        }

                    //only if AbuDawud
                    if (true)
                        switch (Hadithrefrence.code)
                        {
                            case "DarusSalam":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value1 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.tag1 = Hadithrefrence.suffix;
                                    refrence.Refrencetype = "hadith";
                                    break;
                                }

                            case "Hasan":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value1 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.tag1 = Hadithrefrence.suffix;
                                    refrence.Refrencetype = "hadith";
                                    break;
                                }

                            case "USC-MSA":
                                {
                                    refrence.name = Hadithrefrence.code;
                                    refrence.value1 = Int32.Parse(Hadithrefrence.parts[0].astring);
                                    refrence.value2 = Int32.Parse(Hadithrefrence.parts[1].astring);
                                    refrence.Refrencetype = "book hadith";
                                    break;
                                }

                            default:
                                throw null;
                                break;
                        }
                    Refrences.Add(refrence);
                }


                var sources = new List<Models.Lib3.Value>();
                int countSource = 1;
                foreach (var item in hadith.verseReferences)
                {
                    if (item.chapter != "-1")
                    {
                        var source = new Models.Lib3.Value();
                        source.name = "quran:" + countSource;
                        source.value = "ch:" + item.chapter + ",firstverse:" + item.firstVerse + ",lastverse:" + item.lastVerse;
                        sources.Add(source);
                        countSource++;
                    }
                }



                var block = new HadithBlocks()
                {
                    content = contentList,
                    Refrences = Refrences,
                    sources = sources,
                    src = hadith.src

                };

                db.Add(block);
            }


            //db.SaveChanges();
            return BlockCollection;
        }
        
        
        //Upload to Database
        //[HttpGet("upblock/{filename}/")]
        public ActionResult UploadToDb2(string filename)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var hadithBlocks = ReadJsonObject<List<HadithBlocks>>("Json/Hadiths/" + filename);
            //check duplicates
            var name = "In-Book";
            var list = hadithBlocks
                .OrderBy(o => o.Refrences.First(r => r.name == name).value1)
                .ThenBy(o => o.Refrences.First(r => r.name == name).value2)
                //.ThenBy(o => o.Refrences.First(r => r.name == name).value3)
                .ThenBy(o => o.Refrences.First(r => r.Refrencetype == "muslim tag").tag1)
                .ToList()
                ;
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    for (int i2 = 0; i2 < list[i].Refrences.Count; i2++)
                    {
                        var pastitem = list[i - 1];
                        var item = list[i];
                        var pastRedrence = list[i - 1].Refrences.Where(r => r.name == name).First();
                        var refrence = list[i].Refrences.Where(r => r.name == name).First();//[i2];
                        if (refrence.value1 == refrence.value1)
                        {
                            if (pastRedrence.value2 == refrence.value2)
                                if (pastRedrence.value3 == refrence.value3)
                                {
                                   // throw null;
                                }
                        }
                        if (isNull(refrence))
                            throw null;
                    }
                }
            }
            var dbBlocks = new List<HadithBlocks>();
            for (int i = 0; i < list.Count; i++)
            {
                //muslim start from 92 for introduction hadiths
                var number = i + 1 +92;
                var block = list[i];
                block.number = number;
                dbBlocks.Add(block);

            }
            for (int i = 0; i < dbBlocks.Count; i++)
            {
                var block = dbBlocks[i];
                db.HadithBlocks.Add(block);
            }

           // db.SaveChanges();
            watch.Stop();
            return Ok("Done in:"+ watch.ElapsedMilliseconds);
        }

        //Show HadithCollection from Files
        [HttpGet("show/{filename}/{number}")]
        public HadithCollection ShowData(int number, string filename)
        {
            var filepath = "Json/Hadiths/" + filename;
            //string fileString = JsonfileReader(filepath);


            var hadithObj = ReadJsonObject<HadithCollection>(filepath); //JsonConvert.DeserializeObject<HadithCollection>(fileString);
            var hadiths = new List<Models.Collections.Hadith>();
            for (int i = 0; i < 1; i++)
            {
                var getnumber = number + i;
                var hadith = hadithObj.hadiths.hadith[getnumber];
                hadiths.Add(hadith);
            }
            hadithObj.hadiths.hadith = hadiths;
            return hadithObj;
        }

        //Convert Xml to Json
        [HttpGet("convertxml/{name}")]
        public string convertXmlToJson(string name)
        {
            var filepath = "Json/Hadiths/xml/" + name;
            string xml = Readfile(filepath);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string json = JsonConvert.SerializeXmlNode(doc);

            return json;
        }

        [HttpGet("tag")]
        public ActionResult TagGet()
        {
            var queryable = db.HadithBlocks.Select(
                    block => new HadithBlocks()
                    {
                        content = block.content,
                        id = block.id,
                        Refrences = block.Refrences,
                        sources = block.sources,
                        src = block.src,
                        number = block.number
                    }
                );
            var model= queryable.Where(x => x.src == 2)
                .SelectMany( x=> x.Refrences)
                .Where(x => x.name == "Reference")
                //.OrderBy(o=> o.Refrences.First(x=> x.name== "Reference").value4  )
                //.ThenBy(o=> o.Refrences.First(x=> x.name== "Reference").tag1 )
                .ToList()
                ;
            var tags = new List< string > ();
            for (int i = 0; i < model.Count; i++)
            {
                var refes = model.Where(x => x.value4 == i).ToList(); ;
                var holdnum = 0;
                string holdTags = "";
                for (int i2 = 0; i2 < refes.Count; i2++)
                {
                    var refe = refes[i2];
                    var tagnum = refe.value4;

                    if(holdnum != tagnum)
                    {
                        holdnum = tagnum;

                    }
                    holdTags = holdTags +  refe.tag1;
                }

                //cinstructring string
                var tag = "" + holdnum + ":" + ReverseString(holdTags) ;
                if (!isNullOr0(refes.Count))
                    tags.Add(tag);
            }
            return Ok(""+StringfyObject(tags));
        }
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        */

        //============= Helper Method ==============
        public bool equaltag(string rg, string val)
        {
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

        // Numbers[ Vol? , Book?, Hadith?]
        // Method [ 1=hadith, 2= book hadith, 3= vol book hadith 4=????? ]
        public async Task<HadithBlocks> SearchHadiths(IQueryable<HadithBlocks> queryable, /* int src , int Method , int[] Numbers , string[] Values ,*/ IncomingRequest request)
        {
            HadithBlocks model = new HadithBlocks();
            if(request.src == 1)
            switch (request.Method)
            {
                case 1:
                    {
                        model = await queryable.FirstOrDefaultAsync(
                            b =>
                                b.src == request.src
                                &&
                                b.Refrences.Any(
                                    refr =>
                                        refr.name == "DarusSalam" &&
                                        refr.value1 == request.value1
                                    )
                        );
                    }
                    break;
                case 2:
                    {
                        model = await queryable.FirstOrDefaultAsync(
                            b =>
                                b.src == request.src
                                &&
                                b.Refrences.Any(
                                    refr =>
                                        refr.name == "In-Book" &&
                                        refr.value1 == request.value1 &&
                                        refr.value2 == request.value2
                                    )
                        );
                    }
                    break;
                case 3:
                    {
                        model = await queryable.FirstOrDefaultAsync(
                            b =>
                                b.src == request.src
                                &&
                                b.Refrences.Any(
                                    refr =>
                                        refr.name == "USC-MSA" &&
                                        //refr.value1 == request.value1 &&
                                        refr.value2 == request.value2 &&
                                        refr.value3 == request.value3
                                    )
                        );
                    }
                    break;
                case 4:
                    {
                        model = await queryable.FirstOrDefaultAsync(
                            b =>
                                b.src == request.src
                                &&
                                b.Refrences.Any(
                                    refr =>
                                        refr.Refrencetype == request.Refrencetype &&
                                        refr.value4 == request.value4 &&
                                        refr.tag1 == request.tag1
                                    )
                        );
                    }
                    break;

                default:
                    break;
            }

            if (request.src == 2)
                switch (request.Method)
                {
                    case 2:
                        {
                            model = await queryable.FirstOrDefaultAsync(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.name== "In-Book" &&
                                            refr.value1 == request.value1 &&
                                            refr.value2 == request.value2
                                        )
                            );
                        }
                        break;
                    case 3:
                        {
                            model = await queryable.FirstOrDefaultAsync(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.name == "USC-MSA" &&
                                            refr.value1 == request.value2 &&
                                            refr.value2 == request.value3 
                                        )
                            );
                        }
                        break;
                    case 4:
                        {
                            model = await queryable.FirstOrDefaultAsync(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.Refrencetype == "muslim tag" &&
                                            refr.value4 == request.value4 &&
                                            refr.tag1 == request.tag1
                                        )
                            );
                        }
                        break;

                    default:
                        break;
                }

            return model;
        }

        public async Task<HadithBlocks> SearchHadithsList(List<HadithBlocks> List, /* int src , int Method , int[] Numbers , string[] Values ,*/ IncomingRequest request)
        {
            HadithBlocks model = new HadithBlocks();
            if (request.src == 1)
                switch (request.Method)
                {
                    case 1:
                        {
                            model =  List.FirstOrDefault(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.name == "DarusSalam" &&
                                            refr.value1 == request.value1
                                        )
                            );
                        }
                        break;
                    case 2:
                        {
                            model = List.FirstOrDefault(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.name == "In-Book" &&
                                            refr.value1 == request.value1 &&
                                            refr.value2 == request.value2
                                        )
                            );
                        }
                        break;
                    case 3:
                        {
                            model = List.FirstOrDefault(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.name == "USC-MSA" &&
                                            //refr.value1 == request.value1 &&
                                            refr.value2 == request.value2 &&
                                            refr.value3 == request.value3
                                        )
                            );
                        }
                        break;
                    case 4:
                        {
                            model = List.FirstOrDefault(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.Refrencetype == request.Refrencetype &&
                                            refr.value4 == request.value4 &&
                                            refr.tag1 == request.tag1
                                        )
                            );
                        }
                        break;

                    default:
                        break;
                }

            if (request.src == 2)
                switch (request.Method)
                {
                    case 2:
                        {
                            model = List.FirstOrDefault(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.name == "In-Book" &&
                                            refr.value1 == request.value1 &&
                                            refr.value2 == request.value2
                                        )
                            );
                        }
                        break;
                    case 3:
                        {
                            model = List.FirstOrDefault(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.name == "USC-MSA" &&
                                            refr.value1 == request.value2 &&
                                            refr.value2 == request.value3
                                        )
                            );
                        }
                        break;
                    case 4:
                        {
                            model = List.FirstOrDefault(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.Refrencetype == "muslim tag" &&
                                            refr.value4 == request.value4 &&
                                            refr.tag1 == request.tag1
                                        )
                            );
                        }
                        break;

                    default:
                        break;
                }

            return model;
        }




        ///             ==================================================================================================
        /// ================================================= General Helper Methods =================================================///
        ///             ==================================================================================================


        public string Readfile(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                return r.ReadToEnd();
            }
        }
        public T ReadJsonObject<T>(string FilePath)
        {
            using (System.IO.StreamReader r = new System.IO.StreamReader(FilePath))
            {
                var fileString = r.ReadToEnd();
                var Object = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fileString);
                return Object;
            }
        }
        public int StringToInt(string str)
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
        public bool isNull<T>(T Object)
        {
            if (Object == null)
            {
                return true;
            }
            return false;
        }
        public bool isNullOr0<T>(T Object)
        {

            if (Object == null || Object.Equals(0))
            {
                return true;
            }
            return false;
        }
        public string StringfyObject<T>(T Object)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(Object);
        }

    }//Class

    public class HadithIndex
    {
        public int c { get; set; }
        public int fh { get; set; }
        public int lh { get; set; }

    }
}
/*
var watch = System.Diagnostics.Stopwatch.StartNew();
watch.Stop();
watch.ElapsedMilliseconds;
*/
