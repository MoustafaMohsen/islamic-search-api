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
using IslamicSearch.Services;


namespace IslamicSearch.Controllers
{
    [Route("api/[controller]/")]
    public class HadithController : Controller
    {
        private AppDbContext db;
        private IBlockService blockService;
        public HadithController(AppDbContext _db, IBlockService _blockService)
        {
            db = _db;
            blockService = _blockService;
        }

        // GET: api/<controller>
        [HttpGet("")]
        public async Task<ActionResult> Get()
        {
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

            var model = await blockService.SearchHadiths(queryable, request);

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

        [HttpPost("requestblocks/")]
        public ActionResult<HadithBlocks> GetManyByAccurateRequest([FromBody] IncomingRequest request)
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
            if (request.Method == 6)
            {

                list = queryable.Where(
                                b =>
                                    b.src == request.src
                                    &&
                                    b.Refrences.Any(
                                        refr =>
                                            refr.Refrencetype == request.Refrencetype &&
                                            refr.name == request.name &&
                                            refr.value1 == request.value1 &&
                                            refr.value2 == request.value2 &&
                                            refr.value3 == request.value3 &&
                                            refr.value4 == request.value4 &&
                                            refr.tag1 == request.tag1&&
                                            refr.tag2 == request.tag2
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
