﻿using Microsoft.EntityFrameworkCore;
using TryParse.Models;

namespace TryParse.ModelContext
{
    public class ModelContext : DbContext
    {
        public DbSet<CardModel> Models { get; set; } = null!;

        public ModelContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Models;Trusted_Connection=True");
        }
    }
}
