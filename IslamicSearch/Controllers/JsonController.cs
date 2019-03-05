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
using IslamicSearch.Services;

namespace IslamicSearch.Controllers
{
    [Route("api/[controller]")]
    public class JsonController : Controller
    {
        private AppDbContext db;
        private string AdminPassword;
        private string JsonApi;
        IBlockService blockService;

        public JsonController(AppDbContext _db, IOptions<AppSettings> _options, IBlockService _blockService)
        {
            db = _db;
            AdminPassword = _options.Value.AdminPass;
            JsonApi = _options.Value.JsonApi;
            blockService = _blockService;
        }

        // GET: api/<controller>
        [HttpGet("")]
        public async Task<ActionResult> Get([FromQuery]string jsonapi="wrong")
        {
            if (jsonapi != JsonApi)
            {
                return Unauthorized();
            }
            return Content("Welcome, this is json");
        }
        //===================Using Json only================
        [HttpGet("id/json/{src}/{id}")]
        public ActionResult<HadithBlocks> GetByidAndSrcjson(int id, int src,[FromQuery]string jsonapi = "wrong")
        {
            if (jsonapi != JsonApi)
            {
                return Unauthorized();
            }

            var Blocks = ReadJsonObject<List<HadithBlocks>>("Json/Hadiths/fulldb/bukhari_musli_fulldb.json");

            var model = Blocks.FirstOrDefault(b => b.id == id && b.src == src);
            return model;
        }

        [HttpPost("requestbs/json/")]
        public ActionResult<HadithBlocks> GetManyByRequestjson([FromBody] IncomingRequest request, [FromQuery]string jsonapi = "wrong")
        {
            if (jsonapi != JsonApi)
            {
                return Unauthorized();
            }

            var Blocks = ReadJsonObject<List<HadithBlocks>>("Json/Hadiths/fulldb/bukhari_musli_fulldb.json");

            List<HadithBlocks> list = new List<HadithBlocks>();
            if (request.Method == 5)
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

            if (isNullOr0(list) || isNullOr0(list.Count))
                return NotFound();

            return Ok(list);
        }

        //Request Hadith using Request Object
        [HttpPost("requestb/json/")]
        public ActionResult<HadithBlocks> GetByRequestjson([FromBody] IncomingRequest request, [FromQuery]string jsonapi = "wrong")
        {
            if (jsonapi != JsonApi)
            {
                return Unauthorized();
            }

            var Blocks = ReadJsonObject<List<HadithBlocks>>("Json/Hadiths/fulldb/bukhari_musli_fulldb.json");


            var model = blockService.SearchHadithsList(Blocks, request);

            if (isNull(model))
                return NotFound();

            return Ok(model);
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
