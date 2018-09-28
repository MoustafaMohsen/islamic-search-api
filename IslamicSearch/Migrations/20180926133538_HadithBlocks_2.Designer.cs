﻿// <auto-generated />
using System;
using IslamicSearch.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IslamicSearch.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180926133538_HadithBlocks_2")]
    partial class HadithBlocks_2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("IslamicSearch.Models.Lib3.HadithBlocks", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("number");

                    b.Property<int>("src");

                    b.HasKey("id");

                    b.ToTable("HadithBlocks");
                });

            modelBuilder.Entity("IslamicSearch.Models.Lib3.Refrence", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("HadithBlocksid");

                    b.Property<string>("Refrencetype");

                    b.Property<string>("name");

                    b.Property<string>("tag1");

                    b.Property<string>("tag2");

                    b.Property<int>("value1");

                    b.Property<int>("value2");

                    b.Property<int>("value3");

                    b.Property<int>("value4");

                    b.HasKey("id");

                    b.HasIndex("HadithBlocksid");

                    b.ToTable("Refrences");
                });

            modelBuilder.Entity("IslamicSearch.Models.Lib3.Value", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("HadithBlocksid");

                    b.Property<int?>("HadithBlocksid1");

                    b.Property<string>("name");

                    b.Property<string>("value");

                    b.HasKey("id");

                    b.HasIndex("HadithBlocksid");

                    b.HasIndex("HadithBlocksid1");

                    b.ToTable("Value");
                });

            modelBuilder.Entity("IslamicSearch.Models.Lib3.Refrence", b =>
                {
                    b.HasOne("IslamicSearch.Models.Lib3.HadithBlocks")
                        .WithMany("Refrences")
                        .HasForeignKey("HadithBlocksid");
                });

            modelBuilder.Entity("IslamicSearch.Models.Lib3.Value", b =>
                {
                    b.HasOne("IslamicSearch.Models.Lib3.HadithBlocks")
                        .WithMany("content")
                        .HasForeignKey("HadithBlocksid");

                    b.HasOne("IslamicSearch.Models.Lib3.HadithBlocks")
                        .WithMany("sources")
                        .HasForeignKey("HadithBlocksid1");
                });
#pragma warning restore 612, 618
        }
    }
}
