using IslamicSearch.Models;
using IslamicSearch.Models.Collections;
using IslamicSearch.Models.Lib3;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IslamicSearch.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {

        }
        /*
        public DbSet<HadithModel> HadithModel { get; set; }
        public DbSet<Old_refrence> Old_refrence { get; set; }
        public DbSet<In_Book_Refrence> In_Book_Refrence  { get; set; }
        */
        //public DbSet<HadithCollection> HadithCollection { get; set; }

        //public DbSet<Book> Books { get; set; }

        public DbSet<HadithBlocks> HadithBlocks { get; set; }
        public DbSet<Models.Lib3.Refrence> Refrences { get; set; }
    }
}
