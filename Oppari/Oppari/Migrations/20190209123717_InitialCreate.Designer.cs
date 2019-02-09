﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oppari.Models;

namespace Oppari.Migrations
{
    [DbContext(typeof(WatchDogErrorContext))]
    [Migration("20190209123717_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Oppari.Models.WatchDogErrorModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ErrorMessage");

                    b.Property<string>("MethodName");

                    b.Property<string>("Parameter1");

                    b.Property<string>("Parameter2");

                    b.Property<string>("Parameter3");

                    b.Property<string>("Parameter4");

                    b.Property<string>("Parameter5");

                    b.Property<int>("Status");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("WatchDogErrors");
                });
#pragma warning restore 612, 618
        }
    }
}