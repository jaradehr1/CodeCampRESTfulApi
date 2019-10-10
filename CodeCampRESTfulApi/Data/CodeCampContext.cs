using CodeCampRESTfulApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeCampRESTfulApi.Data {
    public class CodeCampContext : DbContext {
        private readonly IConfiguration _config;

        public CodeCampContext(DbContextOptions options, IConfiguration config) : base(options) {
            _config = config;
        }

        public DbSet<Camp> Camps { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Talk> Talks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("CodeCamp"));
        }

        protected override void OnModelCreating(ModelBuilder bldr) {
            bldr.Entity<Camp>()
              .HasData(new {
                  CampId = 1,
                  Moniker = "AZ2019",
                  Name = "Arizona Code Camp",
                  EventDate = new DateTime(2019, 10, 18),
                  LocationId = 1,
                  Length = 1
              });

            bldr.Entity<Location>()
              .HasData(new {
                  LocationId = 1,
                  VenueName = "Arizona Convention Center",
                  Address1 = "123 Main Street",
                  CityTown = "Arizona",
                  StateProvince = "AZ",
                  PostalCode = "12345",
                  Country = "USA"
              });

            bldr.Entity<Talk>()
              .HasData(new {
                  TalkId = 1,
                  CampId = 1,
                  SpeakerId = 1,
                  Title = "Entity Framework From Scratch - Core 2.2",
                  Abstract = "Entity Framework from scratch in an hour. Probably cover it all",
                  Level = 100
              },
              new {
                  TalkId = 2,
                  CampId = 1,
                  SpeakerId = 2,
                  Title = "React Native From Scratch",
                  Abstract = "Mobile/Web Development with .Net Core 2.2 API",
                  Level = 200
              });

            bldr.Entity<Speaker>()
              .HasData(new {
                  SpeakerId = 1,
                  FirstName = "Riad",
                  LastName = "Jaradeh",
                  BlogUrl = "http://viamel.net",
                  Company = "Viamel LLC",
                  CompanyUrl = "http://viamel.net",
                  GitHub = "jaradehr1",
                  Twitter = "jaradehr1"
              }, new {
                  SpeakerId = 2,
                  FirstName = "Elias",
                  LastName = "Yousef",
                  BlogUrl = "http://viamel.net",
                  Company = "Viamel LLC",
                  CompanyUrl = "http://viamel.net",
                  GitHub = "syrianadus",
                  Twitter = "syrianadus"
              });

            /* Define referential constraints */

            // One To Many Relationship Configuration
            // Camp has many talks and a talk belongs to one camp - Deleting the principal will delete the dependents
            bldr.Entity<Camp>()
                .HasMany(c => c.Talks)
                .WithOne(t => t.Camp)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // One To One Relationship Configuration
            // Camp has one location and location can hold one camp at a time
            

        }
    }
}
