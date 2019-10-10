﻿// <auto-generated />
using System;
using CodeCampRESTfulApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeCampRESTfulApi.Migrations
{
    [DbContext(typeof(CodeCampContext))]
    partial class CodeCampContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodeCampRESTfulApi.Data.Entities.Camp", b =>
                {
                    b.Property<int>("CampId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EventDate");

                    b.Property<int>("Length");

                    b.Property<int?>("LocationId");

                    b.Property<string>("Moniker");

                    b.Property<string>("Name");

                    b.HasKey("CampId");

                    b.HasIndex("LocationId");

                    b.ToTable("Camps");

                    b.HasData(
                        new
                        {
                            CampId = 1,
                            EventDate = new DateTime(2019, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Length = 1,
                            LocationId = 1,
                            Moniker = "AZ2019",
                            Name = "Arizona Code Camp"
                        });
                });

            modelBuilder.Entity("CodeCampRESTfulApi.Data.Entities.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("Address3");

                    b.Property<string>("CityTown");

                    b.Property<string>("Country");

                    b.Property<string>("PostalCode");

                    b.Property<string>("StateProvince");

                    b.Property<string>("VenueName");

                    b.HasKey("LocationId");

                    b.ToTable("Location");

                    b.HasData(
                        new
                        {
                            LocationId = 1,
                            Address1 = "123 Main Street",
                            CityTown = "Arizona",
                            Country = "USA",
                            PostalCode = "12345",
                            StateProvince = "AZ",
                            VenueName = "Arizona Convention Center"
                        });
                });

            modelBuilder.Entity("CodeCampRESTfulApi.Data.Entities.Speaker", b =>
                {
                    b.Property<int>("SpeakerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BlogUrl");

                    b.Property<string>("Company");

                    b.Property<string>("CompanyUrl");

                    b.Property<string>("FirstName");

                    b.Property<string>("GitHub");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Twitter");

                    b.HasKey("SpeakerId");

                    b.ToTable("Speakers");

                    b.HasData(
                        new
                        {
                            SpeakerId = 1,
                            BlogUrl = "http://viamel.net",
                            Company = "Viamel LLC",
                            CompanyUrl = "http://viamel.net",
                            FirstName = "Riad",
                            GitHub = "jaradehr1",
                            LastName = "Jaradeh",
                            Twitter = "jaradehr1"
                        },
                        new
                        {
                            SpeakerId = 2,
                            BlogUrl = "http://viamel.net",
                            Company = "Viamel LLC",
                            CompanyUrl = "http://viamel.net",
                            FirstName = "Elias",
                            GitHub = "syrianadus",
                            LastName = "Yousef",
                            Twitter = "syrianadus"
                        });
                });

            modelBuilder.Entity("CodeCampRESTfulApi.Data.Entities.Talk", b =>
                {
                    b.Property<int>("TalkId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abstract");

                    b.Property<int>("CampId");

                    b.Property<int>("Level");

                    b.Property<int?>("SpeakerId");

                    b.Property<string>("Title");

                    b.HasKey("TalkId");

                    b.HasIndex("CampId");

                    b.HasIndex("SpeakerId");

                    b.ToTable("Talks");

                    b.HasData(
                        new
                        {
                            TalkId = 1,
                            Abstract = "Entity Framework from scratch in an hour. Probably cover it all",
                            CampId = 1,
                            Level = 100,
                            SpeakerId = 1,
                            Title = "Entity Framework From Scratch - Core 2.2"
                        },
                        new
                        {
                            TalkId = 2,
                            Abstract = "Mobile/Web Development with .Net Core 2.2 API",
                            CampId = 1,
                            Level = 200,
                            SpeakerId = 2,
                            Title = "React Native From Scratch"
                        });
                });

            modelBuilder.Entity("CodeCampRESTfulApi.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CodeCampRESTfulApi.Data.Entities.Camp", b =>
                {
                    b.HasOne("CodeCampRESTfulApi.Data.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("CodeCampRESTfulApi.Data.Entities.Talk", b =>
                {
                    b.HasOne("CodeCampRESTfulApi.Data.Entities.Camp", "Camp")
                        .WithMany("Talks")
                        .HasForeignKey("CampId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeCampRESTfulApi.Data.Entities.Speaker", "Speaker")
                        .WithMany()
                        .HasForeignKey("SpeakerId");
                });
#pragma warning restore 612, 618
        }
    }
}
