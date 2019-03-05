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
using Microsoft.Extensions.Options;
using IslamicSearch.Helpers;

namespace IslamicSearch.Controllers
{
    [Route("api/[controller]")]
    public class PropertiesController : Controller
    {
        private AppDbContext db;
        private AppSettings appSettings;
        private string AdminPassword;

        public PropertiesController(AppDbContext _db,IOptions<AppSettings> _options)
        {
            db = _db;
            appSettings = _options.Value;
            AdminPassword = _options.Value.AdminPass;
        }

        [HttpGet("")]
        public async Task<IActionResult> get([FromQuery]string pass = "worng")
        {
            if (AdminPassword != pass)
                return Unauthorized();
            else
                return Ok("Welcome Admin");
        }

        //=================Preparing Methods===================//

        //Backup Database
        [HttpGet("DownloadDb/")]
        public async Task<ActionResult<HadithBlocks>> DownloadDb([FromQuery]string pass="worng")
        {
            if (pass != AdminPassword)
            {
                return Unauthorized();
            }
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

        [HttpGet("uploadjson/")]
        public IActionResult uploadjson([FromQuery]string filePath, [FromQuery]string pass = "worng")
        {
            if (pass != AdminPassword)
            {
                return Unauthorized();
            }
            var NewBlocks = ReadJsonObject<List<HadithBlocks>>(filePath);
            var Blocksdb = db.HadithBlocks.Select(
                    block => new HadithBlocks()
                    {
                        content = block.content,
                        //id = block.id,
                        Refrences = block.Refrences,
                        sources = block.sources,
                        src = block.src,
                        number = block.number
                    }
                ).Where(x => x.src == 1).ToList();
            for (int i = 0; i < NewBlocks.Count; i++)
            {
                var block = NewBlocks[i];
                var list1 = new List<Refrence>();
                for (int i2 = 0; i2 < block.Refrences.Count; i2++)
                {
                    var refre = block.Refrences[i2];
                    refre.id = 0;
                    list1.Add(refre);
                }
                NewBlocks[i].Refrences = list1;

                var list2 = new List<Value>();
                for (int i2 = 0; i2 < block.sources.Count; i2++)
                {
                    var refre1 = block.sources[i2];
                    refre1.id = 0;
                    list2.Add(refre1);
                }
                NewBlocks[i].sources = list2;

                var list3 = new List<Value>();
                for (int i2 = 0; i2 < block.content.Count; i2++)
                {
                    var refre = block.content[i2];
                    refre.id = 0;
                    list3.Add(refre);
                }
                NewBlocks[i].content = list3;
            }
            db.HadithBlocks.AddRange(NewBlocks);
            db.SaveChanges();
            return Ok("Uploaded");
        }

        [HttpGet("DownloadRefs")]
        public async Task<ActionResult<HadithBlocks>> DownloadDbrefs([FromQuery]string pass = "worng")
        {
            if (pass != AdminPassword)
            {
                return Unauthorized();
            }
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

            var model = queryable.Where(x => x.src == 1).ToList();
            return Ok(model);
        }

        [HttpGet("getadresses/{src}")]
        public async Task<ActionResult<string>> GetAdress(int src, [FromQuery]string pass = "worng")
        {
            if (pass != AdminPassword)
            {
                return Unauthorized();
            }
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

            var newChapters = model.Where(x => x.name == name1)
                .OrderBy(o => o.value1)
                .ThenBy(o => o.value2)
                .ToList()
                ;


            var oldChapters = model.Where(x => x.name == name2)
                .OrderBy(o => o.value1)
                .ThenBy(o => o.value2)
                //.ThenBy(o => o.value3)
                .ToList()
                ;

            var newList = new List<(int, int)>();
            for (int i = -3; i < 110; i++)
            {
                var chapterCollec = newChapters.Where(x => x.value1 == i).ToList();
                if (chapterCollec.Any())
                {
                    var element = newChapters[i];
                    var (chapter, lastHaith) = (i, chapterCollec.Last().value2);
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

            return Ok(StringfyObject(new { newList, oldList }));
        }

        [HttpGet("uprfBukhari")]
        public IActionResult UploadRefrences([FromQuery]string pass = "worng")
        {
            if (pass != AdminPassword)
            {
                return Unauthorized();
            }
            var filePath = "Json/refrences/BukhariHadiths_Blocks_withFulRefrences.json";
            var refrencesArray = ReadJsonObject<List<List<Refrence>>>(filePath);

            //ReplaceRefrences
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
            var Bukharis = queryable.Where(x => x.src == 1).ToList();

            for (int i = 0; i < Bukharis.Count; i++)
            {
                var block = Bukharis[i];

                var blockDar = block.Refrences.FirstOrDefault(x => x.name == "DarusSalam");
                var blockInbook = block.Refrences.FirstOrDefault(x => x.name == "In-Book");

                var NewRefs = refrencesArray.Where(x =>
                 x.Exists(y => y.name == blockDar.name && y.value1 == blockDar.value1)
                //&&x.Exists(y => y.name == blockInbook.name && y.value1 == blockInbook.value1 && y.value2 == blockInbook.value2)
                )
                .ToList();

                if (NewRefs == null || NewRefs.Count < 1)
                {
                    var stop = 0;
                }
            }

            return Ok(refrencesArray.Count);
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
    }

}
