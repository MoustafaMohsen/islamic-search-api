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

namespace IslamicSearch.Services
{
    public interface IBlockService
    {
        bool equaltag(string rg, string val);
        Task<HadithBlocks> SearchHadiths(IQueryable<HadithBlocks> queryable, /* int src , int Method , int[] Numbers , string[] Values ,*/ IncomingRequest request);
        Task<HadithBlocks> SearchHadithsList(List<HadithBlocks> List, /* int src , int Method , int[] Numbers , string[] Values ,*/ IncomingRequest request);
        List<List<Refrence>> EditAndAddRefrencesBukhari(List<List<Refrence>> refrencesArray);
    }
    public class BlockService : IBlockService
    {
        private AppDbContext db;
        private string AdminPassword;
        private string JsonApi;
        private AppSettings appSettings;
        public BlockService(AppDbContext _db, IOptions<AppSettings> _options)
        {
            db = _db;
            AdminPassword = _options.Value.AdminPass;
            JsonApi = _options.Value.JsonApi;
            appSettings = _options.Value;
        }

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
            if (request.src == 1)
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
                            model = List.FirstOrDefault(
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


        public List<List<Refrence>> EditAndAddRefrencesBukhari(List<List<Refrence>> refrencesArray)
        {
            for (int i = 0; i < refrencesArray.Count; i++)
            {
                var refrence = refrencesArray[i];
                int Naser = -1;
                int dar = 0;
                for (int i2 = 0; i2 < refrence.Count; i2++)
                {
                    var refe = refrence[i2];
                    if (refe.name == "Number-Naser")
                    {
                        Naser = refe.value1;
                    }
                    if (refe.name == "DarusSalam")
                    {
                        dar = refe.value1;
                    }
                    if (refe.value1 == -1)
                    {
                        refrence[i2].value1 = -2;
                    }
                    if (refe.value2 == -1)
                    {
                        refrence[i2].value2 = -2;
                    }
                    if (refe.value3 == -1)
                    {
                        refrence[i2].value3 = -2;
                    }
                    if (refe.value3 == -1)
                    {
                        refrence[i2].value3 = -2;
                    }
                    if (refe.value4 == -1)
                    {
                        refrence[i2].value4 = -2;
                    }

                }//for
                //Replace Naser with DarSalam
                if (dar != Naser)
                {

                    var Daruslam = refrence.Where(x => x.name == "DarusSalam").ToList();
                    var NaserNumberList = refrence.Where(x => x.name == "Number-Naser").ToList();
                    //Stop is invalid (probably will never stop)
                    if (Daruslam.Count > 1 || NaserNumberList.Count > 1)
                    {
                        var Stop1 = 0;

                    }

                    var NaserNumber = NaserNumberList[0];
                    refrence.Remove(NaserNumber);

                    //edit the NaserNumber to Darusalam
                    var refDar = NaserNumber;
                    refDar.name = "DarusSalam";
                    refrence.Add(refDar);

                }
                else
                {
                    var NaserNumberList = refrence.Where(x => x.name == "Number-Naser").ToList();
                    var NaserNumber = NaserNumberList[0];
                    refrence.Remove(NaserNumber);
                }
                refrencesArray[i] = refrence;
            }//for
            return refrencesArray;
        }

    }//class
}//namespace
