using EmailWebApi.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Database
{
    public class EmailContext : DbContext
    {
        public DbSet<Email> Emails { get; set; }
        public DbSet<ThrottlingState> ThrottlingStates { get; set; }
        public EmailContext(DbContextOptions settings) : base(settings)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThrottlingState>().HasData(new ThrottlingState()
            {
                Counter = 0,
                EndPoint = DateTime.Now.AddSeconds(60),
                LastAddress = string.Empty,
                LastAddressCounter = 0,
                Id = 1
            });
        }
    }
}
