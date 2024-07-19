﻿
using GamingStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GamingStore.Services
{
    public class ApplicationDbContext : DbContext

    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {


        }
        public DbSet<Product> Products { get; set; }



    }

}
