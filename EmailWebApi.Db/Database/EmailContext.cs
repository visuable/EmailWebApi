﻿using EmailWebApi.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmailWebApi.Db.Database
{
    public sealed class EmailContext : DbContext
    {
        public EmailContext(DbContextOptions settings) : base(settings)
        {
            Database.EnsureCreated();
        }

        public DbSet<Email> Emails { get; set; }
    }
}