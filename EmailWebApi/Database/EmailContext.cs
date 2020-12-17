using EmailWebApi.Models;
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
        public EmailContext(DbContextOptions settings) : base(settings)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
