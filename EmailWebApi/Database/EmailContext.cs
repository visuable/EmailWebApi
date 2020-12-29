using System;
using EmailWebApi.Entities;
using EmailWebApi.Entities.Dto;
using Microsoft.EntityFrameworkCore;

namespace EmailWebApi.Database
{
    public class EmailContext : DbContext
    {
        public EmailContext(DbContextOptions settings) : base(settings)
        {
            Database.EnsureCreated();
        }

        public DbSet<Email> Emails { get; set; }
        public DbSet<EmailState> States { get; set; }
        public DbSet<EmailInfo> Infos { get; set; }
        public DbSet<EmailContent> Contents { get; set; }
        public DbSet<EmailBody> Bodies { get; set; }
        //public DbSet<ThrottlingStateDto> ThrottlingStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
//            modelBuilder.Entity<ThrottlingStateDto>().HasData(new ThrottlingStateDto
//            {
//                Counter = 0,
//                EndPoint = DateTime.Now.AddSeconds(60),
//                LastAddress = string.Empty,
//                LastAddressCounter = 0,
//                Id = 1
//            });
        }
    }
}